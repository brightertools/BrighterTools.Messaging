using BrighterTools.Messaging.Abstractions;

namespace BrighterTools.Messaging.Tests.Support;

internal sealed class FakeSmsSender : ISmsSender
{
    public List<(string ToPhone, string Body)> SentMessages { get; } = [];

    public Task SendAsync(string toPhone, string fromPhone, string body, CancellationToken cancellationToken = default)
    {
        SentMessages.Add((toPhone, body));
        return Task.CompletedTask;
    }
}
