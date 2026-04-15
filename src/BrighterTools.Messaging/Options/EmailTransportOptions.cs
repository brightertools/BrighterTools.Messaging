namespace BrighterTools.Messaging.Options;

/// <summary>
/// Represents configuration options for Email Transport.
/// </summary>
public class EmailTransportOptions
{
    /// <summary>
    /// Gets the section Name value.
    /// </summary>
    public const string SectionName = "Email";

    /// <summary>
    /// Gets or sets the From Name.
    /// </summary>
    public string FromName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the From Address.
    /// </summary>
    public string FromAddress { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the Subject Prefix.
    /// </summary>
    public string SubjectPrefix { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the Test Mode.
    /// </summary>
    public bool TestMode { get; set; } = true;
    /// <summary>
    /// Gets or sets the Test Mode Addresses.
    /// </summary>
    public List<string> TestModeAddresses { get; set; } = [];
}

