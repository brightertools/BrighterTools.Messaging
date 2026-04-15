namespace BrighterTools.Messaging.Abstractions;
/// <summary>
/// Defines operations for Notification Dispatcher.
/// </summary>
public interface INotificationDispatcher
{
    /// <summary>
    /// Executes the enqueue Async operation.
    /// </summary>
    /// <param name="notificationMessageId">The notificationMessageId value.</param>
    /// <param name="cancellationToken">The cancellationToken value.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task EnqueueAsync(int notificationMessageId, CancellationToken cancellationToken = default);
}

