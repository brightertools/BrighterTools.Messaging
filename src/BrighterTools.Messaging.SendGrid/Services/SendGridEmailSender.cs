using BrighterTools.Messaging.Abstractions;
using BrighterTools.Messaging.Models;
using BrighterTools.Messaging.Options;
using BrighterTools.Messaging.SendGrid.Options;
using Microsoft.Extensions.Options;
using Polly;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BrighterTools.Messaging.SendGrid.Services;

/// <summary>
/// Sends email through SendGrid.
/// </summary>
public class SendGridEmailSender(
    IOptions<EmailTransportOptions> transportOptions,
    IOptions<SendGridEmailSenderOptions> providerOptions) : IEmailSender
{
    /// <summary>
    /// Sends an email message.
    /// </summary>
    public async Task SendAsync(string toName, string toEmail, string subject, string htmlBody, string textBody, string? fromName, string? fromAddress, IEnumerable<EmailAttachment>? attachments, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(toEmail))
        {
            throw new InvalidOperationException("Recipient email address is required.");
        }

        var transport = transportOptions.Value;
        var provider = providerOptions.Value;
        var resolvedFromName = string.IsNullOrWhiteSpace(fromName) ? transport.FromName : fromName;
        var resolvedFromAddress = string.IsNullOrWhiteSpace(fromAddress) ? transport.FromAddress : fromAddress;

        if (string.IsNullOrWhiteSpace(resolvedFromAddress))
        {
            throw new InvalidOperationException("Email:FromAddress is required.");
        }

        var retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(4, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));
        await retryPolicy.ExecuteAsync(async token =>
        {
            var client = new SendGridClient(provider.ApiKey);
            var message = MailHelper.CreateSingleEmail(
                new EmailAddress(resolvedFromAddress, resolvedFromName),
                new EmailAddress(toEmail, toName),
                subject,
                textBody,
                htmlBody);

            if (attachments != null)
            {
                foreach (var attachment in attachments.OrderBy(x => x.Order))
                {
                    message.AddAttachment(
                        attachment.Name,
                        Convert.ToBase64String(attachment.Content),
                        attachment.ContentType,
                        string.IsNullOrWhiteSpace(attachment.ContentId) ? "attachment" : "inline",
                        attachment.ContentId);
                }
            }

            var response = await client.SendEmailAsync(message, token);
            if (!response.IsSuccessStatusCode)
            {
                var responseBody = response.Body == null ? string.Empty : await response.Body.ReadAsStringAsync(token);
                throw new InvalidOperationException($"SendGrid failed with status {(int)response.StatusCode} {response.StatusCode}. {responseBody}".Trim());
            }
        }, cancellationToken);
    }
}