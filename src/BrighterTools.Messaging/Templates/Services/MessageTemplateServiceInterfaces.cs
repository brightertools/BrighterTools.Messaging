using BrighterTools.Messaging.Models;
using BrighterTools.Messaging.Templates.Models;
using BrighterTools.Messaging.Templates.Seeds;

namespace BrighterTools.Messaging.Templates.Services;

public interface IMessageTemplateSeedService
{
    Task SeedAsync(IEnumerable<MessageTemplateSeedDefinition> seeds, CancellationToken cancellationToken = default);
}

public interface IMessageTemplateResolver
{
    Task<(MessageTemplateDefinition Definition, MessageTemplateContent Content)> ResolveAsync(string key, string? culture = null, string? tenantId = null, CancellationToken cancellationToken = default);
}

public interface IMessageTemplateAdminService
{
    Task<IReadOnlyList<MessageTemplateSummary>> ListAsync(MessageTemplateQuery query, CancellationToken cancellationToken = default);
    Task<MessageTemplateDetail> GetAsync(string key, MessageTemplateScopeQuery query, CancellationToken cancellationToken = default);
    Task<MessageTemplateDetail> SaveAsync(MessageTemplateSaveRequest request, string? userId = null, CancellationToken cancellationToken = default);
    Task<MessageTemplatePreviewResult> PreviewAsync(MessageTemplatePreviewRequest request, CancellationToken cancellationToken = default);
    Task<MessageTemplateDetail> RevertAsync(string key, MessageTemplateScopeQuery query, string? userId = null, CancellationToken cancellationToken = default);
    Task<MessageTemplateDetail> PublishAsync(string key, MessageTemplateScopeQuery query, string? userId = null, CancellationToken cancellationToken = default);
}

public interface ISystemMessageSender
{
    Task<bool> SendEmailAsync(string key, EmailRecipientList recipients, IReadOnlyDictionary<string, string> mergeFields, string? culture = null, string? tenantId = null, string? fromAddress = null, string? fromName = null, IEnumerable<EmailAttachment>? attachments = null, CancellationToken cancellationToken = default);
}