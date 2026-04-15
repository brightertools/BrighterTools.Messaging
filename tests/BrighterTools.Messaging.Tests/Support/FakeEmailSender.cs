using BrighterTools.Messaging.Abstractions;
using BrighterTools.Messaging.Models;

namespace BrighterTools.Messaging.Tests.Support;

internal sealed class FakeEmailSender : IEmailSender
{
    public List<(string ToEmail, string Subject)> SentMessages { get; } = [];

    public Task SendAsync(string toName, string toEmail, string subject, string htmlBody, string textBody, string? fromName, string? fromAddress, IEnumerable<EmailAttachment>? attachments, CancellationToken cancellationToken = default)
    {
        SentMessages.Add((toEmail, subject));
        return Task.CompletedTask;
    }
}
