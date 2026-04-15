namespace BrighterTools.Messaging.Abstractions;
/// <summary>
/// Defines operations for SMS Sender.
/// </summary>
public interface ISmsSender
{
    /// <summary>
    /// Sends the send Async.
    /// </summary>
    /// <param name="toPhone">The toPhone value.</param>
    /// <param name="fromPhone">The fromPhone value.</param>
    /// <param name="body">The body value.</param>
    /// <param name="cancellationToken">The cancellationToken value.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SendAsync(string toPhone, string fromPhone, string body, CancellationToken cancellationToken = default);
}

