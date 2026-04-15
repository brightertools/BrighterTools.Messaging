using BrighterTools.Messaging.Abstractions;
using BrighterTools.Messaging.Twilio.Options;
using BrighterTools.Messaging.Twilio.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BrighterTools.Messaging.Twilio.DependencyInjection;

/// <summary>
/// Provides extension methods for Twilio Messaging Service Collection.
/// </summary>
public static class TwilioMessagingServiceCollectionExtensions
{
    /// <summary>
    /// Adds Brighter Tools Twilio SMS Sender.
    /// </summary>
    public static IServiceCollection AddBrighterToolsTwilioSmsSender(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<TwilioSmsSenderOptions>()
            .Bind(configuration.GetSection(TwilioSmsSenderOptions.SectionName))
            .Validate(options => !string.IsNullOrWhiteSpace(options.AccountId), "Twilio:AccountId is required.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.AuthToken), "Twilio:AuthToken is required.")
            .ValidateOnStart();

        services.AddTransient<ISmsSender, TwilioSmsSender>();
        return services;
    }
}

