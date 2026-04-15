using BrighterTools.Messaging.Enums;
namespace BrighterTools.Messaging.Models;
/// <summary>
/// Represents Email Template Type.
/// </summary>
public class EmailTemplateType : EntityBase
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
    /// Gets or sets the Name.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the Subject.
    /// </summary>
    public string Subject { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the Description.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the Single Template.
    /// </summary>
    public bool SingleTemplate { get; set; }
    /// <summary>
    /// Gets or sets the Required Fields.
    /// </summary>
    public string RequiredFields { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the Available Fields.
    /// </summary>
    public string AvailableFields { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the Read Only.
    /// </summary>
    public bool ReadOnly { get; set; }
    /// <summary>
    /// Gets or sets the Send AS User.
    /// </summary>
    public bool SendAsUser { get; set; }
    /// <summary>
    /// Gets or sets the S System Template.
    /// </summary>
    public bool IsSystemTemplate { get; set; }
}

