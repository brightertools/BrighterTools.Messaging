namespace BrighterTools.Messaging.Postmark.Options;

/// <summary>
/// Represents configuration options for Postmark Email Sender.
/// </summary>
public class PostmarkEmailSenderOptions
{
    /// <summary>
    /// Gets the section Name value.
    /// </summary>
    public const string SectionName = "Postmark";

    /// <summary>
    /// Gets or sets the Server Token.
    /// </summary>
    public string ServerToken { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the Track Opens.
    /// </summary>
    public bool TrackOpens { get; set; } = true;
}

