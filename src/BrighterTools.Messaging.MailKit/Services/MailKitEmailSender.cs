using BrighterTools.Messaging.Abstractions;
using BrighterTools.Messaging.MailKit.Options;
using BrighterTools.Messaging.Models;
using BrighterTools.Messaging.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Polly;

namespace BrighterTools.Messaging.MailKit.Services;

/// <summary>
/// Represents Mail Kit Email Sender.
/// </summary>
public class MailKitEmailSender(
    IOptions<EmailTransportOptions> transportOptions,
    IOptions<MailKitEmailSenderOptions> providerOptions) : IEmailSender
{
    /// <summary>
    /// Sends the operation.
    /// </summary>
    public async Task SendAsync(string toName, string toEmail, string subject, string htmlBody, string textBody, string? fromName, string? fromAddress, IEnumerable<EmailAttachment>? attachments, CancellationToken cancellationToken = default)
    {
        var transport = transportOptions.Value;
        var provider = providerOptions.Value;
        var socketOptions = Enum.TryParse<SecureSocketOptions>(provider.SecureSocketOption, true, out var parsedSocketOptions)
            ? parsedSocketOptions
            : SecureSocketOptions.StartTls;

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(fromName ?? transport.FromName, fromAddress ?? transport.FromAddress));
        message.To.Add(new MailboxAddress(toName, toEmail));
        message.Subject = subject;

        var builder = new BodyBuilder
        {
            HtmlBody = htmlBody,
            TextBody = textBody
        };

        if (attachments != null)
        {
            foreach (var attachment in attachments.OrderBy(x => x.Order))
            {
                if (string.IsNullOrWhiteSpace(attachment.ContentId))
                {
                    builder.Attachments.Add(attachment.Name, attachment.Content, ContentType.Parse(attachment.ContentType));
                    continue;
                }

                var linked = builder.LinkedResources.Add(attachment.Name, attachment.Content, ContentType.Parse(attachment.ContentType));
                linked.ContentId = attachment.ContentId;
            }
        }

        message.Body = builder.ToMessageBody();

        var retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(4, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));
        await retryPolicy.ExecuteAsync(async token =>
        {
            using var client = new SmtpClient();
            await client.ConnectAsync(provider.Host, provider.Port, socketOptions, token);

            if (!string.IsNullOrWhiteSpace(provider.Username))
            {
                await client.AuthenticateAsync(provider.Username, provider.Password, token);
            }

            await client.SendAsync(message, token);
            await client.DisconnectAsync(true, token);
        }, cancellationToken);
    }
}

