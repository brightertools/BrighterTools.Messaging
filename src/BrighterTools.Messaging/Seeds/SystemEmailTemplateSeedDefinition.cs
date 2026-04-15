using BrighterTools.Messaging.Enums;
namespace BrighterTools.Messaging.Seeds;
/// <summary>
/// Represents System Email Template Seed Definition.
/// </summary>
public sealed record SystemEmailTemplateSeedDefinition(
    EmailType EmailType,
    string Name,
    string Subject,
    string Description,
    string RequiredFields,
    string AvailableFields,
    string HtmlContent,
    string TextContent,
    bool SingleTemplate = true,
    bool ReadOnly = true,
    bool SendAsUser = false,
    bool IsSystemTemplate = true,
    string DesignContent = "",
    EmailType? BaseTemplateEmailType = EmailType.BaseTemplate
);

