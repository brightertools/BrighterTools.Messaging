namespace BrighterTools.Messaging.Models;
/// <summary>
/// Represents Email Attachment.
/// </summary>
public class EmailAttachment
{
    /// <summary>
    /// Gets or sets the Name.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the Content.
    /// </summary>
    public byte[] Content { get; set; } = [];
    /// <summary>
    /// Gets or sets the Content Type.
    /// </summary>
    public string ContentType { get; set; } = "application/octet-stream";
    /// <summary>
    /// Gets or sets the Content ID.
    /// </summary>
    public string? ContentId { get; set; }
    /// <summary>
    /// Gets or sets the Order.
    /// </summary>
    public int Order { get; set; }
}

