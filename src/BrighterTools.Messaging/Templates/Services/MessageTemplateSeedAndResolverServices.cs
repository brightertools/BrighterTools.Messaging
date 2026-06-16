using BrighterTools.Messaging.Templates.Abstractions;
using BrighterTools.Messaging.Templates.Models;
using BrighterTools.Messaging.Templates.Seeds;

namespace BrighterTools.Messaging.Templates.Services;

public sealed class MessageTemplateSeedService : IMessageTemplateSeedService
{
    private readonly IMessageTemplateDefinitionStore _definitionStore;
    private readonly IMessageTemplateContentStore _contentStore;

    public MessageTemplateSeedService(IMessageTemplateDefinitionStore definitionStore, IMessageTemplateContentStore contentStore)
    {
        _definitionStore = definitionStore;
        _contentStore = contentStore;
    }

    public async Task SeedAsync(IEnumerable<MessageTemplateSeedDefinition> seeds, CancellationToken cancellationToken = default)
    {
        foreach (var seed in seeds)
        {
            var existingDefinition = await _definitionStore.GetByKeyAsync(seed.Key, cancellationToken);
            var definition = existingDefinition ?? new MessageTemplateDefinition { Key = seed.Key, Guid = Guid.NewGuid(), CreatedAtUtc = DateTimeOffset.UtcNow };
            definition.Name = seed.Name;
            definition.Description = seed.Description;
            definition.Category = seed.Category;
            definition.Channel = MessageTemplateChannel.Email;
            definition.IsSystem = seed.IsSystem;
            definition.IsEditable = seed.IsEditable;
            definition.IsTenantOverrideAllowed = seed.IsTenantOverrideAllowed;
            definition.DefaultCulture = seed.DefaultCulture;
            definition.RequiredVariables = seed.RequiredVariables;
            definition.AvailableVariables = seed.AvailableVariables;
            definition.DefaultVersion = Math.Max(definition.DefaultVersion, seed.DefaultVersion);
            definition.LastUpdatedAtUtc = DateTimeOffset.UtcNow;
            await _definitionStore.UpsertAsync(definition, cancellationToken);

            var defaultContent = await _contentStore.GetAsync(seed.Key, seed.DefaultCulture, MessageTemplateScope.LibraryDefault, null, cancellationToken);
            if (defaultContent is null)
            {
                defaultContent = new MessageTemplateContent
                {
                    DefinitionKey = seed.Key,
                    Culture = seed.DefaultCulture,
                    Scope = MessageTemplateScope.LibraryDefault,
                    TenantId = null,
                    Version = seed.DefaultVersion,
                    BasedOnDefaultVersion = seed.DefaultVersion,
                    IsCustomized = false,
                    IsPublished = true,
                    CreatedAtUtc = DateTimeOffset.UtcNow
                };
            }

            defaultContent.Subject = seed.Subject;
            defaultContent.HtmlContent = seed.HtmlContent;
            defaultContent.TextContent = seed.TextContent;
            defaultContent.DesignContent = seed.DesignContent;
            defaultContent.SourceFormat = seed.SourceFormat;
            defaultContent.BaseTemplateKey = seed.BaseTemplateKey;
            defaultContent.Version = seed.DefaultVersion;
            defaultContent.BasedOnDefaultVersion = seed.DefaultVersion;
            defaultContent.LastUpdatedAtUtc = DateTimeOffset.UtcNow;
            await _contentStore.SaveAsync(defaultContent, cancellationToken);
        }
    }
}

public sealed class MessageTemplateResolver : IMessageTemplateResolver
{
    private readonly IMessageTemplateDefinitionStore _definitionStore;
    private readonly IMessageTemplateContentStore _contentStore;

    public MessageTemplateResolver(IMessageTemplateDefinitionStore definitionStore, IMessageTemplateContentStore contentStore)
    {
        _definitionStore = definitionStore;
        _contentStore = contentStore;
    }

    public async Task<(MessageTemplateDefinition Definition, MessageTemplateContent Content)> ResolveAsync(string key, string? culture = null, string? tenantId = null, CancellationToken cancellationToken = default)
    {
        var definition = await _definitionStore.GetByKeyAsync(key, cancellationToken) ?? throw new InvalidOperationException($"Message template definition '{key}' was not found.");
        var requestedCulture = string.IsNullOrWhiteSpace(culture) ? definition.DefaultCulture : culture!;

        var candidates = new List<(MessageTemplateScope Scope, string Culture, string? TenantId)>();
        if (definition.IsTenantOverrideAllowed && !string.IsNullOrWhiteSpace(tenantId))
        {
            candidates.Add((MessageTemplateScope.Tenant, requestedCulture, tenantId));
            candidates.Add((MessageTemplateScope.Tenant, definition.DefaultCulture, tenantId));
        }

        candidates.Add((MessageTemplateScope.Host, requestedCulture, null));
        candidates.Add((MessageTemplateScope.Host, definition.DefaultCulture, null));
        candidates.Add((MessageTemplateScope.LibraryDefault, requestedCulture, null));
        candidates.Add((MessageTemplateScope.LibraryDefault, definition.DefaultCulture, null));

        foreach (var candidate in candidates.Distinct())
        {
            var content = await _contentStore.GetAsync(key, candidate.Culture, candidate.Scope, candidate.TenantId, cancellationToken);
            if (content is not null && content.IsPublished)
            {
                return (definition, content);
            }
        }

        throw new InvalidOperationException($"Message template content '{key}' was not found.");
    }
}