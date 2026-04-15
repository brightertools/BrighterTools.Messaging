using BrighterTools.Messaging.Abstractions;
using BrighterTools.Messaging.Models;

namespace BrighterTools.Messaging.Tests.Support;

internal sealed class InMemoryNotificationAttachmentStore : INotificationAttachmentStore
{
    private readonly Dictionary<int, List<EmailAttachment>> _attachments = [];

    public Task SaveAsync(int notificationMessageId, IEnumerable<EmailAttachment> attachments, CancellationToken cancellationToken = default)
    {
        _attachments[notificationMessageId] = attachments
            .Select(x => new EmailAttachment
            {
                Name = x.Name,
                Content = x.Content,
                ContentType = x.ContentType,
                ContentId = x.ContentId,
                Order = x.Order
            })
            .ToList();

        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<EmailAttachment>> GetAsync(int notificationMessageId, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<EmailAttachment> attachments = _attachments.TryGetValue(notificationMessageId, out var stored)
            ? stored
            : [];

        return Task.FromResult(attachments);
    }
}
