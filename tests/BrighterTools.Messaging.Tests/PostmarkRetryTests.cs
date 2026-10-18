using BrighterTools.Messaging.Options;
using BrighterTools.Messaging.Postmark.Options;
using BrighterTools.Messaging.Postmark.Services;
using BrighterTools.Messaging.Postmark.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PostmarkDotNet;

namespace BrighterTools.Messaging.Tests;

public class PostmarkRetryTests
{
    [Fact]
    public void Default_preserves_existing_retry_behaviour() => Assert.True(new PostmarkEmailSenderOptions().EnableAutomaticRetries);

    [Theory]
    [InlineData(null, true)]
    [InlineData("true", true)]
    [InlineData("false", false)]
    public void Registration_binds_explicit_option_and_preserves_omitted_default(string? value, bool expected)
    {
        var values = new Dictionary<string, string?> { ["Postmark:ServerToken"] = "synthetic-not-a-real-token" };
        if (value is not null) values["Postmark:EnableAutomaticRetries"] = value;
        var services = new ServiceCollection();
        services.AddBrighterToolsPostmarkEmailSender(new ConfigurationBuilder().AddInMemoryCollection(values).Build());
        using var provider = services.BuildServiceProvider();
        Assert.Equal(expected, provider.GetRequiredService<IOptions<PostmarkEmailSenderOptions>>().Value.EnableAutomaticRetries);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Successful_delivery_is_not_repeated(bool retries)
    {
        var sender = new FakeSender(retries, failFirst: false);
        await Send(sender);
        Assert.Equal(1, sender.Attempts);
    }

    [Fact]
    public async Task Disabled_retries_make_one_attempt_and_preserve_unknown_outcome()
    {
        var sender = new FakeSender(false);
        await Assert.ThrowsAsync<TimeoutException>(() => Send(sender));
        Assert.Equal(1, sender.Attempts);
    }
    [Fact]
    public async Task Enabled_retries_still_recover_after_transient_failure()
    {
        var sender = new FakeSender(true);
        await Send(sender);
        Assert.Equal(2, sender.Attempts);
    }
    [Fact]
    public async Task Cancellation_before_send_makes_no_provider_attempt()
    {
        var sender = new FakeSender(false);
        using var cancellation = new CancellationTokenSource(); cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => sender.SendAsync("", "to@example.test", "Test", "Test", "Test", null, null, null, cancellation.Token));
        Assert.Equal(0, sender.Attempts);
    }
    private static Task Send(FakeSender sender) => sender.SendAsync("", "to@example.test", "Test", "Test", "Test", null, null, null);
    private sealed class FakeSender(bool retries, bool failFirst = true) : PostmarkEmailSender(
        Microsoft.Extensions.Options.Options.Create(new EmailTransportOptions { FromAddress = "from@example.test" }),
        Microsoft.Extensions.Options.Options.Create(new PostmarkEmailSenderOptions { ServerToken = "synthetic", EnableAutomaticRetries = retries }))
    {
        public int Attempts { get; private set; }
        protected override Task SendMessageAsync(PostmarkMessage message, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            if (++Attempts == 1 && failFirst) throw new TimeoutException("Synthetic unknown result");
            return Task.CompletedTask;
        }
    }
}
