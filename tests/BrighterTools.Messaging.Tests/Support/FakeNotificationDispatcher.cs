using BrighterTools.Messaging.Abstractions;

namespace BrighterTools.Messaging.Tests.Support;

internal sealed class FakeNotificationDispatcher : INotificationDispatcher
{
    public List<int> EnqueuedIds { get; } = [];

    public Task EnqueueAsync(int notificationMessageId, CancellationToken cancellationToken = default)
    {
        EnqueuedIds.Add(notificationMessageId);
        return Task.CompletedTask;
    }
}
