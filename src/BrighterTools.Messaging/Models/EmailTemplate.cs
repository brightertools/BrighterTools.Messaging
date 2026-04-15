using BrighterTools.Messaging.Enums;
namespace BrighterTools.Messaging.Models;
/// <summary>
/// Represents Email Template.
/// </summary>
public class EmailTemplate : EntityBase
{
    /// <summary>
    /// Gets or sets the User ID.
    /// </summary>
    public long? UserId { get; set; }
    /// <summary>
    /// Gets or sets the Email Type.
    /// </summary>
    public EmailType EmailType { get; set; }
    /// <summary>
    /// Gets or sets the Email Template Type ID.
    /// </summary>
    public int EmailTemplateTypeId { get; set; }
    /// <summary>
    /// Gets or sets the Email Template Type.
    /// </summary>
    public EmailTemplateType? EmailTemplateType { get; set; }
    /// <summary>
    /// Gets or sets the Base Template ID.
    /// </summary>
    public int? BaseTemplateId { get; set; }
    /// <summary>
    /// Gets or sets the Base Template.
    /// </summary>
    public EmailTemplate? BaseTemplate { get; set; }
    /// <summary>
    /// Gets or sets the Subject.
    /// </summary>
    public string Subject { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the Design Content.
    /// </summary>
    public string DesignContent { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the HTML Content.
    /// </summary>
    public string HtmlContent { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the Text Content.
    /// </summary>
    public string TextContent { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the Merge Fields.
    /// </summary>
    public string MergeFields { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the Archived.
    /// </summary>
    public bool Archived { get; set; }
    /// <summary>
    /// Gets or sets the Sub Templates.
    /// </summary>
    public ICollection<EmailTemplate> SubTemplates { get; set; } = [];
}

