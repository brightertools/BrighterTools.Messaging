namespace BrighterTools.Messaging.Templates.Models;

public sealed record MessageTemplateQuery(
    string? TenantId = null,
    string? Culture = null,
    string? Category = null,
    bool? IncludeSystem = null,
    bool? IncludeFeature = null,
    string? Query = null);

public sealed record MessageTemplateScopeQuery(string? TenantId = null, string? Culture = null);

public sealed record MessageTemplateSummary(
    string Key,
    string Name,
    string? Description,
    string? Category,
    string Culture,
    string Scope,
    bool IsSystem,
    bool IsEditable,
    bool IsCustomized,
    bool IsTenantOverrideAllowed,
    string SourceFormat,
    DateTimeOffset? UpdatedAtUtc);

public sealed record MessageTemplateDetail(
    string Key,
    string Name,
    string? Description,
    string? Category,
    string Culture,
    string Scope,
    bool IsSystem,
    bool IsEditable,
    bool IsCustomized,
    bool IsTenantOverrideAllowed,
    string SourceFormat,
    DateTimeOffset? UpdatedAtUtc,
    string Subject,
    string HtmlContent,
    string TextContent,
    string? DesignContent,
    IReadOnlyList<MessageTemplateVariable> RequiredVariables,
    IReadOnlyList<MessageTemplateVariable> AvailableVariables,
    int? BasedOnDefaultVersion,
    int? CurrentDefaultVersion);

public sealed record MessageTemplateSaveRequest(
    string Key,
    string? Culture,
    string? TenantId,
    string Subject,
    string HtmlContent,
    string TextContent,
    string? DesignContent,
    string SourceFormat,
    string? ChangeReason = null);

public sealed record MessageTemplatePreviewRequest(
    string Key,
    string? Culture,
    string? TenantId,
    string? Subject,
    string? HtmlContent,
    string? TextContent,
    string? DesignContent,
    Dictionary<string, string> MergeFields);

public sealed record MessageTemplatePreviewResult(string Subject, string Html, string Text);

public sealed record MessageTemplateAssetUploadRequest(string FileName, string ContentType, Stream Content, string? TemplateKey = null, string? TenantId = null);

public sealed record MessageTemplateAssetUploadResult(string Url);