using BrighterTools.Messaging.Templates.Models;

namespace BrighterTools.Messaging.Templates.Seeds;

public sealed record MessageTemplateSeedDefinition(
    string Key,
    string Name,
    string Description,
    string Category,
    string Subject,
    string HtmlContent,
    string TextContent,
    IReadOnlyList<MessageTemplateVariable> RequiredVariables,
    IReadOnlyList<MessageTemplateVariable> AvailableVariables,
    string DefaultCulture = "en-GB",
    bool IsSystem = true,
    bool IsEditable = true,
    bool IsTenantOverrideAllowed = false,
    string? BaseTemplateKey = "layout.base",
    MessageTemplateSourceFormat SourceFormat = MessageTemplateSourceFormat.Html,
    string? DesignContent = null,
    int DefaultVersion = 1);