using BrighterTools.Messaging.Templates.Models;

namespace BrighterTools.Messaging.Templates.Abstractions;

public interface IMessageTemplateDefinitionStore
{
    Task<MessageTemplateDefinition?> GetByKeyAsync(string key, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MessageTemplateDefinition>> ListAsync(CancellationToken cancellationToken = default);
    Task UpsertAsync(MessageTemplateDefinition definition, CancellationToken cancellationToken = default);
}

public interface IMessageTemplateContentStore
{
    Task<MessageTemplateContent?> GetAsync(string definitionKey, string culture, MessageTemplateScope scope, string? tenantId = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MessageTemplateContent>> ListAsync(CancellationToken cancellationToken = default);
    Task<MessageTemplateContent> SaveAsync(MessageTemplateContent content, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}

public interface IMessageTemplateRevisionStore
{
    Task SaveAsync(MessageTemplateRevision revision, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MessageTemplateRevision>> ListAsync(int templateContentId, CancellationToken cancellationToken = default);
    Task<MessageTemplateRevision?> GetLatestAsync(int templateContentId, CancellationToken cancellationToken = default);
}

public interface IMessageTemplateAssetStore
{
    Task<MessageTemplateAssetUploadResult> UploadAsync(MessageTemplateAssetUploadRequest request, CancellationToken cancellationToken = default);
}