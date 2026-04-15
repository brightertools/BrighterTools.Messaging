using BrighterTools.Messaging.Abstractions;
using BrighterTools.Messaging.MailKit.Options;
using BrighterTools.Messaging.MailKit.Services;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BrighterTools.Messaging.MailKit.DependencyInjection;

/// <summary>
/// Provides extension methods for Mail Kit Messaging Service Collection.
/// </summary>
public static class MailKitMessagingServiceCollectionExtensions
{
    /// <summary>
    /// Adds Brighter Tools Mail Kit Email Sender.
    /// </summary>
    public static IServiceCollection AddBrighterToolsMailKitEmailSender(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<MailKitEmailSenderOptions>()
            .Bind(configuration.GetSection(MailKitEmailSenderOptions.SectionName))
            .Validate(options => !string.IsNullOrWhiteSpace(options.Host), "Smtp:Host is required.")
            .Validate(options => options.Port > 0, "Smtp:Port must be greater than zero.")
            .Validate(options => Enum.TryParse<SecureSocketOptions>(options.SecureSocketOption, true, out _), "Smtp:SecureSocketOption must be a valid MailKit SecureSocketOptions value.")
            .ValidateOnStart();

        services.AddTransient<IEmailSender, MailKitEmailSender>();
        return services;
    }
}

