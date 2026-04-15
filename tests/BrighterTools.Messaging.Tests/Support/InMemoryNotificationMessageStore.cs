using BrighterTools.Messaging.Abstractions;
using BrighterTools.Messaging.Models;

namespace BrighterTools.Messaging.Tests.Support;

internal sealed class InMemoryNotificationMessageStore : INotificationMessageStore
{
    private readonly List<NotificationMessage> _messages = [];

    public IReadOnlyList<NotificationMessage> Messages => _messages;

    public Task<NotificationMessage> CreateAsync(NotificationMessage message, CancellationToken cancellationToken = default)
    {
        message.Id = _messages.Count == 0 ? 1 : _messages.Max(x => x.Id) + 1;
        _messages.Add(message);
        return Task.FromResult(message);
    }

    public Task<NotificationMessage?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_messages.SingleOrDefault(x => x.Id == id));
    }

    public Task UpdateAsync(NotificationMessage message, CancellationToken cancellationToken = default)
    {
        var existingIndex = _messages.FindIndex(x => x.Id == message.Id);
        if (existingIndex >= 0)
        {
            _messages[existingIndex] = message;
        }

        return Task.CompletedTask;
    }
}
