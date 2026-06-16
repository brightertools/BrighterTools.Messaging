namespace BrighterTools.Messaging.Templates.Models;

public sealed record MessageTemplateVariable(
    string Key,
    string? Label = null,
    string? Description = null,
    bool Required = false,
    bool IsHtml = false,
    string? SampleValue = null);

public sealed class MessageTemplateDefinition
{
    public int Id { get; set; }
    public Guid Guid { get; set; } = Guid.NewGuid();
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public MessageTemplateChannel Channel { get; set; } = MessageTemplateChannel.Email;
    public bool IsSystem { get; set; } = true;
    public bool IsEditable { get; set; } = true;
    public bool IsTenantOverrideAllowed { get; set; }
    public string DefaultCulture { get; set; } = "en-GB";
    public IReadOnlyList<MessageTemplateVariable> RequiredVariables { get; set; } = [];
    public IReadOnlyList<MessageTemplateVariable> AvailableVariables { get; set; } = [];
    public int DefaultVersion { get; set; } = 1;
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset LastUpdatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}

public sealed class MessageTemplateContent
{
    public int Id { get; set; }
    public Guid Guid { get; set; } = Guid.NewGuid();
    public string DefinitionKey { get; set; } = string.Empty;
    public string Culture { get; set; } = "en-GB";
    public MessageTemplateScope Scope { get; set; } = MessageTemplateScope.Host;
    public string? TenantId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string HtmlContent { get; set; } = string.Empty;
    public string TextContent { get; set; } = string.Empty;
    public string? DesignContent { get; set; }
    public MessageTemplateSourceFormat SourceFormat { get; set; } = MessageTemplateSourceFormat.Html;
    public string? BaseTemplateKey { get; set; }
    public int Version { get; set; } = 1;
    public int? BasedOnDefaultVersion { get; set; }
    public bool IsCustomized { get; set; }
    public bool IsPublished { get; set; } = true;
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset LastUpdatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public string? LastUpdatedByUserId { get; set; }
}

public sealed class MessageTemplateRevision
{
    public int Id { get; set; }
    public Guid Guid { get; set; } = Guid.NewGuid();
    public int TemplateContentId { get; set; }
    public string DefinitionKey { get; set; } = string.Empty;
    public string Culture { get; set; } = "en-GB";
    public MessageTemplateScope Scope { get; set; } = MessageTemplateScope.Host;
    public string? TenantId { get; set; }
    public int Version { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string HtmlContent { get; set; } = string.Empty;
    public string TextContent { get; set; } = string.Empty;
    public string? DesignContent { get; set; }
    public MessageTemplateSourceFormat SourceFormat { get; set; } = MessageTemplateSourceFormat.Html;
    public string? SavedByUserId { get; set; }
    public DateTimeOffset SavedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public string? ChangeReason { get; set; }
}