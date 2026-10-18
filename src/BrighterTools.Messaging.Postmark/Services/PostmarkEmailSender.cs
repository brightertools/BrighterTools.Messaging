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

        IAsyncPolicy retryPolicy = provider.EnableAutomaticRetries
            ? Policy.Handle<Exception>().WaitAndRetryAsync(4, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)))
            : Policy.NoOpAsync();
        await retryPolicy.ExecuteAsync(async token =>
        {
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

            await SendMessageAsync(message, token);
        }, cancellationToken);
    }

    /// <summary>Single provider attempt. Hosts must not infer non-delivery from a timeout.</summary>
    protected virtual async Task SendMessageAsync(PostmarkMessage message, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var client = new PostmarkClient(providerOptions.Value.ServerToken);
        var result = await client.SendMessageAsync(message).WaitAsync(cancellationToken);
        // These documented validation/authentication codes explicitly reject the send.
        // Server errors and unknown responses remain uncertain.
        if (result.Status == PostmarkStatus.UserError && result.ErrorCode is 10 or 300 or 402 or 403 or 406 or 410 or 411 or 412 or 413 or 422 or 1235)
            throw new PostmarkRejectedException(result.ErrorCode);
        if (result.Status != PostmarkStatus.Success)
            throw new InvalidOperationException($"Postmark failed: {result.Message}");
    }
}

