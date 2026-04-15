using BrighterTools.Messaging.Enums;
namespace BrighterTools.Messaging.Models;
/// <summary>
/// Represents Notification Message.
/// </summary>
public class NotificationMessage : EntityBase
{
    /// <summary>
    /// Gets or sets the TO Name.
    /// </summary>
    public string ToName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the TO Email.
    /// </summary>
    public string? ToEmail { get; set; }
    /// <summary>
    /// Gets or sets the TO Mobile.
    /// </summary>
    public string? ToMobile { get; set; }
    /// <summary>
    /// Gets or sets the Subject.
    /// </summary>
    public string? Subject { get; set; }
    /// <summary>
    /// Gets or sets the HTML.
    /// </summary>
    public string? Html { get; set; }
    /// <summary>
    /// Gets or sets the Text.
    /// </summary>
    public string? Text { get; set; }
    /// <summary>
    /// Gets or sets the From Name.
    /// </summary>
    public string? FromName { get; set; }
    /// <summary>
    /// Gets or sets the From Address.
    /// </summary>
    public string? FromAddress { get; set; }
    /// <summary>
    /// Gets or sets the From Mobile.
    /// </summary>
    public string? FromMobile { get; set; }
    /// <summary>
    /// Gets or sets the Failure Count.
    /// </summary>
    public int FailureCount { get; set; }
    /// <summary>
    /// Gets or sets the Last Failed Date.
    /// </summary>
    public DateTimeOffset? LastFailedDate { get; set; }
    /// <summary>
    /// Gets or sets the Last Failed Message.
    /// </summary>
    public string? LastFailedMessage { get; set; }
    /// <summary>
    /// Gets or sets the Send Date.
    /// </summary>
    public DateTimeOffset SendDate { get; set; } = DateTimeOffset.UtcNow;
    /// <summary>
    /// Gets or sets the Sent.
    /// </summary>
    public bool Sent { get; set; }
    /// <summary>
    /// Gets or sets the Sent Date.
    /// </summary>
    public DateTimeOffset? SentDate { get; set; }
    /// <summary>
    /// Gets or sets the Notification Message Type.
    /// </summary>
    public NotificationMessageType NotificationMessageType { get; set; }
    /// <summary>
    /// Gets or sets the Attachments.
    /// </summary>
    public List<NotificationMessageAttachment> Attachments { get; set; } = [];
}

