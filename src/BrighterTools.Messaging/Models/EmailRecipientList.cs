namespace BrighterTools.Messaging.Models;
/// <summary>
/// Represents Email Recipient List.
/// </summary>
public class EmailRecipientList : List<EmailRecipient>
{
    /// <summary>
    /// Executes Email Recipient List.
    /// </summary>
    public EmailRecipientList() { }
    /// <summary>
    /// Executes Email Recipient List.
    /// </summary>
    public EmailRecipientList(string name, string emailAddress)
    {
        Add(new EmailRecipient(name, emailAddress));
    }
}

