namespace BrighterTools.Messaging.MailKit.Options;

/// <summary>
/// Represents configuration options for Mail Kit Email Sender.
/// </summary>
public class MailKitEmailSenderOptions
{
    /// <summary>
    /// Gets the section Name value.
    /// </summary>
    public const string SectionName = "Smtp";

    /// <summary>
    /// Gets or sets the Host.
    /// </summary>
    public string Host { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the Port.
    /// </summary>
    public int Port { get; set; } = 587;
    /// <summary>
    /// Gets or sets the Username.
    /// </summary>
    public string? Username { get; set; }
    /// <summary>
    /// Gets or sets the Password.
    /// </summary>
    public string? Password { get; set; }
    /// <summary>
    /// Gets or sets the Secure Socket Option.
    /// </summary>
    public string SecureSocketOption { get; set; } = "StartTls";
}

