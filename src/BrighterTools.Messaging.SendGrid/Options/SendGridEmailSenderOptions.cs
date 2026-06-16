namespace BrighterTools.Messaging.SendGrid.Options;

/// <summary>
/// Represents configuration options for the SendGrid email sender.
/// </summary>
public class SendGridEmailSenderOptions
{
    /// <summary>
    /// Gets the section name value.
    /// </summary>
    public const string SectionName = "SendGrid";

    /// <summary>
    /// Gets or sets the SendGrid API key.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;
}