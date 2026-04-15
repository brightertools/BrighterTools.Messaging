namespace BrighterTools.Messaging.Options;

/// <summary>
/// Represents configuration options for SMS Transport.
/// </summary>
public class SmsTransportOptions
{
    /// <summary>
    /// Gets the section Name value.
    /// </summary>
    public const string SectionName = "SmsMessage";

    /// <summary>
    /// Gets or sets the Test Mode.
    /// </summary>
    public bool TestMode { get; set; } = true;
    /// <summary>
    /// Gets or sets the Test Mode Number List.
    /// </summary>
    public List<string> TestModeNumberList { get; set; } = [];
    /// <summary>
    /// Gets or sets the From Name.
    /// </summary>
    public string FromName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the From Phone Number.
    /// </summary>
    public string? FromPhoneNumber { get; set; }
}

