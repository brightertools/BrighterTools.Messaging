namespace BrighterTools.Messaging.Models;
/// <summary>
/// Represents Notification Message Attachment.
/// </summary>
public class NotificationMessageAttachment
{
    /// <summary>
    /// Gets or sets the File Name.
    /// </summary>
    public string FileName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the Content Type.
    /// </summary>
    public string ContentType { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the Content.
    /// </summary>
    public byte[] Content { get; set; } = [];
    /// <summary>
    /// Gets or sets the Content ID.
    /// </summary>
    public string? ContentId { get; set; }
    /// <summary>
    /// Gets or sets the Order.
    /// </summary>
    public int Order { get; set; }
}

