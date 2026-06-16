using BrighterTools.Messaging.Abstractions;
using BrighterTools.Messaging.SendGrid.Options;
using BrighterTools.Messaging.SendGrid.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BrighterTools.Messaging.SendGrid.DependencyInjection;

/// <summary>
/// Provides extension methods for SendGrid Messaging Service Collection.
/// </summary>
public static class SendGridMessagingServiceCollectionExtensions
{
    /// <summary>
    /// Adds the Brighter Tools SendGrid email sender.
    /// </summary>
    public static IServiceCollection AddBrighterToolsSendGridEmailSender(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<SendGridEmailSenderOptions>()
            .Bind(configuration.GetSection(SendGridEmailSenderOptions.SectionName))
            .Validate(options => !string.IsNullOrWhiteSpace(options.ApiKey), "SendGrid:ApiKey is required.")
            .ValidateOnStart();

        services.AddTransient<IEmailSender, SendGridEmailSender>();
        return services;
    }
}