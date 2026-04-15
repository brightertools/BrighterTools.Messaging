namespace BrighterTools.Messaging.Twilio.Options;

/// <summary>
/// Represents configuration options for Twilio SMS Sender.
/// </summary>
public class TwilioSmsSenderOptions
{
    /// <summary>
    /// Gets the section Name value.
    /// </summary>
    public const string SectionName = "Twilio";

    /// <summary>
    /// Gets or sets the Account ID.
    /// </summary>
    public string AccountId { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the Auth Token.
    /// </summary>
    public string AuthToken { get; set; } = string.Empty;
}

