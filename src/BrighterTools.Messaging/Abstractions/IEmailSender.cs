using BrighterTools.Messaging.Models;
namespace BrighterTools.Messaging.Abstractions;
/// <summary>
/// Defines operations for Email Sender.
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// Sends the send Async.
    /// </summary>
    /// <param name="toName">The toName value.</param>
    /// <param name="toEmail">The toEmail value.</param>
    /// <param name="subject">The subject value.</param>
    /// <param name="htmlBody">The htmlBody value.</param>
    /// <param name="textBody">The textBody value.</param>
    /// <param name="fromName">The fromName value.</param>
    /// <param name="fromAddress">The fromAddress value.</param>
    /// <param name="attachments">The attachments value.</param>
    /// <param name="cancellationToken">The cancellationToken value.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SendAsync(string toName, string toEmail, string subject, string htmlBody, string textBody, string? fromName, string? fromAddress, IEnumerable<EmailAttachment>? attachments, CancellationToken cancellationToken = default);
}

