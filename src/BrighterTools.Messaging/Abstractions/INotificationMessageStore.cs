using BrighterTools.Messaging.Models;
namespace BrighterTools.Messaging.Abstractions;
/// <summary>
/// Defines operations for Notification Message Store.
/// </summary>
public interface INotificationMessageStore
{
    /// <summary>
    /// Creates the create Async.
    /// </summary>
    /// <param name="message">The message value.</param>
    /// <param name="cancellationToken">The cancellationToken value.</param>
    /// <returns>A task that represents the asynchronous operation and contains the operation result.</returns>
    Task<NotificationMessage> CreateAsync(NotificationMessage message, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets the get By Id Async.
    /// </summary>
    /// <param name="id">The id value.</param>
    /// <param name="cancellationToken">The cancellationToken value.</param>
    /// <returns>A task that represents the asynchronous operation and contains the operation result.</returns>
    Task<NotificationMessage?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>
    /// Updates the update Async.
    /// </summary>
    /// <param name="message">The message value.</param>
    /// <param name="cancellationToken">The cancellationToken value.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateAsync(NotificationMessage message, CancellationToken cancellationToken = default);
}

