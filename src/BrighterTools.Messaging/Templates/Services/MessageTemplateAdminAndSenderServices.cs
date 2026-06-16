using BrighterTools.Messaging.Models;
using BrighterTools.Messaging.Options;
using BrighterTools.Messaging.Services;
using BrighterTools.Messaging.Templates.Abstractions;
using BrighterTools.Messaging.Templates.Models;
using Microsoft.Extensions.Options;

namespace BrighterTools.Messaging.Templates.Services;

public sealed class MessageTemplateAdminService : IMessageTemplateAdminService
{
    private readonly IMessageTemplateDefinitionStore _definitionStore;
    private readonly IMessageTemplateContentStore _contentStore;
    private readonly IMessageTemplateRevisionStore _revisionStore;
    private readonly IMessageTemplateResolver _resolver;
    private readonly ITemplateRenderer _renderer;
    private readonly EmailTransportOptions _emailOptions;

    public MessageTemplateAdminService(
        IMessageTemplateDefinitionStore definitionStore,
        IMessageTemplateContentStore contentStore,
        IMessageTemplateRevisionStore revisionStore,
        IMessageTemplateResolver resolver,
        ITemplateRenderer renderer,
        IOptions<EmailTransportOptions> emailOptions)
    {
        _definitionStore = definitionStore;
        _contentStore = contentStore;
        _revisionStore = revisionStore;
        _resolver = resolver;
        _renderer = renderer;
        _emailOptions = emailOptions.Value;
    }

    public async Task<IReadOnlyList<MessageTemplateSummary>> ListAsync(MessageTemplateQuery query, CancellationToken cancellationToken = default)
    {
        var definitions = await _definitionStore.ListAsync(cancellationToken);
        var list = new List<MessageTemplateSummary>();
        foreach (var definition in definitions.OrderBy(x => x.Category).ThenBy(x => x.Name))
        {
            if (!string.IsNullOrWhiteSpace(query.Category) && !string.Equals(definition.Category, query.Category, StringComparison.OrdinalIgnoreCase)) continue;
            if (!string.IsNullOrWhiteSpace(query.Query) && !definition.Name.Contains(query.Query, StringComparison.OrdinalIgnoreCase) && !definition.Key.Contains(query.Query, StringComparison.OrdinalIgnoreCase)) continue;
            try
            {
                var (_, content) = await _resolver.ResolveAsync(definition.Key, query.Culture, query.TenantId, cancellationToken);
                list.Add(ToSummary(definition, content));
            }
            catch (InvalidOperationException)
            {
                // A definition without content is not editable yet.
            }
        }

        return list;
    }

    public async Task<MessageTemplateDetail> GetAsync(string key, MessageTemplateScopeQuery query, CancellationToken cancellationToken = default)
    {
        var (definition, content) = await _resolver.ResolveAsync(key, query.Culture, query.TenantId, cancellationToken);
        return ToDetail(definition, content);
    }

    public async Task<MessageTemplateDetail> SaveAsync(MessageTemplateSaveRequest request, string? userId = null, CancellationToken cancellationToken = default)
    {
        var definition = await _definitionStore.GetByKeyAsync(request.Key, cancellationToken) ?? throw new InvalidOperationException($"Message template definition '{request.Key}' was not found.");
        if (!definition.IsEditable) throw new InvalidOperationException($"Message template '{request.Key}' is not editable.");

        var culture = string.IsNullOrWhiteSpace(request.Culture) ? definition.DefaultCulture : request.Culture!;
        var scope = !string.IsNullOrWhiteSpace(request.TenantId) && definition.IsTenantOverrideAllowed ? MessageTemplateScope.Tenant : MessageTemplateScope.Host;
        var existing = await _contentStore.GetAsync(request.Key, culture, scope, scope == MessageTemplateScope.Tenant ? request.TenantId : null, cancellationToken);
        var defaultContent = await _contentStore.GetAsync(request.Key, definition.DefaultCulture, MessageTemplateScope.LibraryDefault, null, cancellationToken);
        var content = existing ?? new MessageTemplateContent
        {
            DefinitionKey = request.Key,
            Culture = culture,
            Scope = scope,
            TenantId = scope == MessageTemplateScope.Tenant ? request.TenantId : null,
            Version = 0,
            CreatedAtUtc = DateTimeOffset.UtcNow,
            IsPublished = true
        };

        if (existing is not null)
        {
            await _revisionStore.SaveAsync(ToRevision(existing, userId, request.ChangeReason), cancellationToken);
        }

        content.Subject = request.Subject;
        content.HtmlContent = request.HtmlContent;
        content.TextContent = request.TextContent;
        content.DesignContent = request.DesignContent;
        content.SourceFormat = ParseSourceFormat(request.SourceFormat);
        content.BaseTemplateKey = defaultContent?.BaseTemplateKey;
        content.Version += 1;
        content.BasedOnDefaultVersion = defaultContent?.Version;
        content.IsCustomized = true;
        content.LastUpdatedAtUtc = DateTimeOffset.UtcNow;
        content.LastUpdatedByUserId = userId;
        var saved = await _contentStore.SaveAsync(content, cancellationToken);
        return ToDetail(definition, saved);
    }

    public async Task<MessageTemplatePreviewResult> PreviewAsync(MessageTemplatePreviewRequest request, CancellationToken cancellationToken = default)
    {
        var (definition, content) = await _resolver.ResolveAsync(request.Key, request.Culture, request.TenantId, cancellationToken);
        var previewContent = new MessageTemplateContent
        {
            DefinitionKey = content.DefinitionKey,
            Culture = content.Culture,
            Scope = content.Scope,
            TenantId = content.TenantId,
            Subject = request.Subject ?? content.Subject,
            HtmlContent = request.HtmlContent ?? content.HtmlContent,
            TextContent = request.TextContent ?? content.TextContent,
            BaseTemplateKey = content.BaseTemplateKey
        };
        ValidateRequiredMergeFields(definition, request.MergeFields);
        var rendered = await RenderAsync(definition, previewContent, request.MergeFields, cancellationToken);
        return new MessageTemplatePreviewResult(rendered.Subject, rendered.Html, rendered.Text);
    }

    public async Task<MessageTemplateDetail> RevertAsync(string key, MessageTemplateScopeQuery query, string? userId = null, CancellationToken cancellationToken = default)
    {
        var definition = await _definitionStore.GetByKeyAsync(key, cancellationToken) ?? throw new InvalidOperationException($"Message template definition '{key}' was not found.");
        var culture = string.IsNullOrWhiteSpace(query.Culture) ? definition.DefaultCulture : query.Culture!;
        var scope = !string.IsNullOrWhiteSpace(query.TenantId) && definition.IsTenantOverrideAllowed ? MessageTemplateScope.Tenant : MessageTemplateScope.Host;
        var existing = await _contentStore.GetAsync(key, culture, scope, scope == MessageTemplateScope.Tenant ? query.TenantId : null, cancellationToken);
        if (existing is not null)
        {
            await _revisionStore.SaveAsync(ToRevision(existing, userId, "Revert"), cancellationToken);
            await _contentStore.DeleteAsync(existing.Id, cancellationToken);
        }

        return await GetAsync(key, query, cancellationToken);
    }

    public async Task<MessageTemplateDetail> PublishAsync(string key, MessageTemplateScopeQuery query, string? userId = null, CancellationToken cancellationToken = default)
    {
        var (definition, content) = await _resolver.ResolveAsync(key, query.Culture, query.TenantId, cancellationToken);
        content.IsPublished = true;
        content.LastUpdatedAtUtc = DateTimeOffset.UtcNow;
        content.LastUpdatedByUserId = userId;
        var saved = await _contentStore.SaveAsync(content, cancellationToken);
        return ToDetail(definition, saved);
    }

    private async Task<EmailContent> RenderAsync(MessageTemplateDefinition definition, MessageTemplateContent content, IReadOnlyDictionary<string, string>? mergeFields, CancellationToken cancellationToken)
    {
        var emailContent = new EmailContent { Subject = content.Subject, Html = content.HtmlContent, Text = content.TextContent };
        if (!string.IsNullOrWhiteSpace(content.BaseTemplateKey))
        {
            var (_, baseContent) = await _resolver.ResolveAsync(content.BaseTemplateKey, content.Culture, content.TenantId, cancellationToken);
            emailContent = _renderer.ApplyBaseTemplate(emailContent, new EmailContent { Subject = baseContent.Subject, Html = baseContent.HtmlContent, Text = baseContent.TextContent });
        }

        return _renderer.Render(emailContent, mergeFields, _emailOptions.SubjectPrefix);
    }

    private static void ValidateRequiredMergeFields(MessageTemplateDefinition definition, IReadOnlyDictionary<string, string>? mergeFields)
    {
        foreach (var variable in definition.RequiredVariables.Where(x => x.Required))
        {
            if (mergeFields is null || !mergeFields.TryGetValue(variable.Key, out var value) || string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException($"Required merge field '{variable.Key}' is missing for template '{definition.Key}'.");
            }
        }
    }

    private static MessageTemplateRevision ToRevision(MessageTemplateContent content, string? userId, string? changeReason)
        => new()
        {
            TemplateContentId = content.Id,
            DefinitionKey = content.DefinitionKey,
            Culture = content.Culture,
            Scope = content.Scope,
            TenantId = content.TenantId,
            Version = content.Version,
            Subject = content.Subject,
            HtmlContent = content.HtmlContent,
            TextContent = content.TextContent,
            DesignContent = content.DesignContent,
            SourceFormat = content.SourceFormat,
            SavedByUserId = userId,
            SavedAtUtc = DateTimeOffset.UtcNow,
            ChangeReason = changeReason
        };

    private static MessageTemplateSourceFormat ParseSourceFormat(string value)
        => string.Equals(value, "react-email-editor", StringComparison.OrdinalIgnoreCase) ? MessageTemplateSourceFormat.ReactEmailEditor : MessageTemplateSourceFormat.Html;

    private static MessageTemplateSummary ToSummary(MessageTemplateDefinition definition, MessageTemplateContent content)
        => new(definition.Key, definition.Name, definition.Description, definition.Category, content.Culture, ToApiScope(content.Scope), definition.IsSystem, definition.IsEditable, content.IsCustomized, definition.IsTenantOverrideAllowed, ToApiSourceFormat(content.SourceFormat), content.LastUpdatedAtUtc);

    private static MessageTemplateDetail ToDetail(MessageTemplateDefinition definition, MessageTemplateContent content)
        => new(definition.Key, definition.Name, definition.Description, definition.Category, content.Culture, ToApiScope(content.Scope), definition.IsSystem, definition.IsEditable, content.IsCustomized, definition.IsTenantOverrideAllowed, ToApiSourceFormat(content.SourceFormat), content.LastUpdatedAtUtc, content.Subject, content.HtmlContent, content.TextContent, content.DesignContent, definition.RequiredVariables, definition.AvailableVariables, content.BasedOnDefaultVersion, definition.DefaultVersion);

    private static string ToApiScope(MessageTemplateScope scope)
        => scope switch { MessageTemplateScope.LibraryDefault => "library-default", MessageTemplateScope.Tenant => "tenant", _ => "host" };

    private static string ToApiSourceFormat(MessageTemplateSourceFormat sourceFormat)
        => sourceFormat == MessageTemplateSourceFormat.ReactEmailEditor ? "react-email-editor" : "html";
}

public sealed class SystemMessageSender : ISystemMessageSender
{
    private readonly IMessageTemplateResolver _resolver;
    private readonly ITemplateRenderer _renderer;
    private readonly INotificationMessageService _notificationMessageService;
    private readonly EmailTransportOptions _emailOptions;

    public SystemMessageSender(IMessageTemplateResolver resolver, ITemplateRenderer renderer, INotificationMessageService notificationMessageService, IOptions<EmailTransportOptions> emailOptions)
    {
        _resolver = resolver;
        _renderer = renderer;
        _notificationMessageService = notificationMessageService;
        _emailOptions = emailOptions.Value;
    }

    public async Task<bool> SendEmailAsync(string key, EmailRecipientList recipients, IReadOnlyDictionary<string, string> mergeFields, string? culture = null, string? tenantId = null, string? fromAddress = null, string? fromName = null, IEnumerable<EmailAttachment>? attachments = null, CancellationToken cancellationToken = default)
    {
        var (definition, content) = await _resolver.ResolveAsync(key, culture, tenantId, cancellationToken);
        ValidateRequiredMergeFields(definition, mergeFields);
        var emailContent = new EmailContent { Subject = content.Subject, Html = content.HtmlContent, Text = content.TextContent };
        if (!string.IsNullOrWhiteSpace(content.BaseTemplateKey))
        {
            var (_, baseContent) = await _resolver.ResolveAsync(content.BaseTemplateKey, culture ?? content.Culture, tenantId, cancellationToken);
            emailContent = _renderer.ApplyBaseTemplate(emailContent, new EmailContent { Subject = baseContent.Subject, Html = baseContent.HtmlContent, Text = baseContent.TextContent });
        }

        var rendered = _renderer.Render(emailContent, mergeFields, _emailOptions.SubjectPrefix);
        return await _notificationMessageService.SendEmailAsync(recipients, rendered.Subject, rendered.Html, rendered.Text, fromAddress, fromName, attachments, cancellationToken);
    }

    private static void ValidateRequiredMergeFields(MessageTemplateDefinition definition, IReadOnlyDictionary<string, string>? mergeFields)
    {
        foreach (var variable in definition.RequiredVariables.Where(x => x.Required))
        {
            if (mergeFields is null || !mergeFields.TryGetValue(variable.Key, out var value) || string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException($"Required merge field '{variable.Key}' is missing for template '{definition.Key}'.");
            }
        }
    }
}