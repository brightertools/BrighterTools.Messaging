using BrighterTools.Messaging.Models;

namespace BrighterTools.Messaging.Abstractions;

/// <summary>
/// Defines operations for Notification Attachment Store.
/// </summary>
public interface INotificationAttachmentStore
{
    /// <summary>
    /// Executes the save Async operation.
    /// </summary>
    /// <param name="notificationMessageId">The notificationMessageId value.</param>
    /// <param name="attachments">The attachments value.</param>
    /// <param name="cancellationToken">The cancellationToken value.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SaveAsync(int notificationMessageId, IEnumerable<EmailAttachment> attachments, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets the get Async.
    /// </summary>
    /// <param name="notificationMessageId">The notificationMessageId value.</param>
    /// <param name="cancellationToken">The cancellationToken value.</param>
    /// <returns>A task that represents the asynchronous operation and contains the operation result.</returns>
    Task<IReadOnlyList<EmailAttachment>> GetAsync(int notificationMessageId, CancellationToken cancellationToken = default);
}

