using BrighterTools.Messaging.Options;
using BrighterTools.Messaging.Services;
using BrighterTools.Messaging.Templates.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BrighterTools.Messaging.DependencyInjection;

/// <summary>
/// Provides extension methods for Messaging Service Collection.
/// </summary>
public static class MessagingServiceCollectionExtensions
{
    /// <summary>
    /// Adds Brighter Tools Messaging.
    /// </summary>
    public static IServiceCollection AddBrighterToolsMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailTransportOptions>(configuration.GetSection(EmailTransportOptions.SectionName));
        services.Configure<SmsTransportOptions>(configuration.GetSection(SmsTransportOptions.SectionName));
        services.AddScoped<ITemplateRenderer, TemplateRenderer>();
        services.AddScoped<ISystemEmailTemplateService, SystemEmailTemplateService>();
        services.AddScoped<INotificationMessageService, NotificationMessageService>();
        services.AddScoped<IMessageTemplateSeedService, MessageTemplateSeedService>();
        services.AddScoped<IMessageTemplateResolver, MessageTemplateResolver>();
        services.AddScoped<IMessageTemplateAdminService, MessageTemplateAdminService>();
        services.AddScoped<ISystemMessageSender, SystemMessageSender>();
        return services;
    }
}

