using BrighterTools.Messaging.Abstractions;
using BrighterTools.Messaging.Postmark.Options;
using BrighterTools.Messaging.Postmark.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BrighterTools.Messaging.Postmark.DependencyInjection;

/// <summary>
/// Provides extension methods for Postmark Messaging Service Collection.
/// </summary>
public static class PostmarkMessagingServiceCollectionExtensions
{
    /// <summary>
    /// Adds Brighter Tools Postmark Email Sender.
    /// </summary>
    public static IServiceCollection AddBrighterToolsPostmarkEmailSender(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<PostmarkEmailSenderOptions>()
            .Bind(configuration.GetSection(PostmarkEmailSenderOptions.SectionName))
            .Validate(options => !string.IsNullOrWhiteSpace(options.ServerToken), "Postmark:ServerToken is required.")
            .ValidateOnStart();

        services.AddTransient<IEmailSender, PostmarkEmailSender>();
        return services;
    }
}

