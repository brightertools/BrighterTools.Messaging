using BrighterTools.Messaging.Abstractions;
using BrighterTools.Messaging.Models;
using BrighterTools.Messaging.Options;
using BrighterTools.Messaging.Postmark.Options;
using Microsoft.Extensions.Options;
using Polly;
using PostmarkDotNet;
using System.IO;

namespace BrighterTools.Messaging.Postmark.Services;

/// <summary>
/// Represents Postmark Email Sender.
/// </summary>
public class PostmarkEmailSender(
    IOptions<EmailTransportOptions> transportOptions,
    IOptions<PostmarkEmailSenderOptions> providerOptions) : IEmailSender
{
    /// <summary>
    /// Sends the operation.
    /// </summary>
    public async Task SendAsync(string toName, string toEmail, string subject, string htmlBody, string textBody, string? fromName, string? fromAddress, IEnumerable<EmailAttachment>? attachments, CancellationToken cancellationToken = default)
    {
        var transport = transportOptions.Value;
        var provider = providerOptions.Value;

        var retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(4, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));
        await retryPolicy.ExecuteAsync(async token =>
        {
            var client = new PostmarkClient(provider.ServerToken);
            var message = new PostmarkMessage
            {
                From = $"{fromName ?? transport.FromName} <{fromAddress ?? transport.FromAddress}>",
                To = string.IsNullOrWhiteSpace(toName) ? toEmail : $"{toName} <{toEmail}>",
                ReplyTo = fromAddress ?? transport.FromAddress,
                Subject = subject,
                HtmlBody = htmlBody,
                TextBody = textBody,
                TrackOpens = provider.TrackOpens
            };

            if (attachments != null)
            {
                foreach (var attachment in attachments.OrderBy(x => x.Order))
                {
                    message.AddAttachment(new MemoryStream(attachment.Content), attachment.Name, attachment.ContentType, attachment.ContentId);
                }
            }

            var result = await client.SendMessageAsync(message);
            if (result.Status != PostmarkStatus.Success)
            {
                throw new InvalidOperationException($"Postmark failed: {result.Message}");
            }
        }, cancellationToken);
    }
}

