namespace BrighterTools.Messaging.Models;
/// <summary>
/// Represents Email Recipient.
/// </summary>
public class EmailRecipient
{
    /// <summary>
    /// Executes Email Recipient.
    /// </summary>
    public EmailRecipient() { }
    /// <summary>
    /// Executes Email Recipient.
    /// </summary>
    public EmailRecipient(string name, string emailAddress)
    {
        Name = name;
        EmailAddress = emailAddress;
    }
    /// <summary>
    /// Gets or sets the Name.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the Email Address.
    /// </summary>
    public string EmailAddress { get; set; } = string.Empty;
}

