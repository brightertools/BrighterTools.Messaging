using BrighterTools.Messaging.Abstractions;
using BrighterTools.Messaging.Twilio.Options;
using Microsoft.Extensions.Options;
using Polly;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace BrighterTools.Messaging.Twilio.Services;

/// <summary>
/// Represents Twilio SMS Sender.
/// </summary>
public class TwilioSmsSender(IOptions<TwilioSmsSenderOptions> options) : ISmsSender
{
    /// <summary>
    /// Sends the operation.
    /// </summary>
    public async Task SendAsync(string toPhone, string fromPhone, string body, CancellationToken cancellationToken = default)
    {
        var config = options.Value;
        var retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(4, attempt => TimeSpan.FromSeconds(attempt));
        await retryPolicy.ExecuteAsync(async () =>
        {
            TwilioClient.Init(config.AccountId, config.AuthToken);
            var result = await MessageResource.CreateAsync(to: new PhoneNumber(toPhone), from: new PhoneNumber(fromPhone), body: body);
            if (result.Status != MessageResource.StatusEnum.Accepted && result.Status != MessageResource.StatusEnum.Queued && result.Status != MessageResource.StatusEnum.Sent)
            {
                throw new InvalidOperationException($"Twilio failed: {result.Status}");
            }
        });
    }
}

