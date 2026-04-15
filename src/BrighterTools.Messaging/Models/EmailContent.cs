namespace BrighterTools.Messaging.Models;
/// <summary>
/// Represents Email Content.
/// </summary>
public class EmailContent
{
    /// <summary>
    /// Gets or sets the Subject.
    /// </summary>
    public string Subject { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the HTML.
    /// </summary>
    public string Html { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the Text.
    /// </summary>
    public string Text { get; set; } = string.Empty;
}

