using BrighterTools.Messaging.Abstractions;
using BrighterTools.Messaging.DependencyInjection;
using BrighterTools.Messaging.MailKit.DependencyInjection;
using BrighterTools.Messaging.Postmark.DependencyInjection;
using BrighterTools.Messaging.SendGrid.DependencyInjection;
using BrighterTools.Messaging.Services;
using BrighterTools.Messaging.Twilio.DependencyInjection;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BrighterTools.Messaging.Tests;

public class RegistrationTests
{
    [Fact]
    public void AddBrighterToolsMessaging_RegistersCoreServicesWithoutTransportImplementations()
    {
        var configuration = new ConfigurationBuilder().Build();
        var services = new ServiceCollection();

        services.AddBrighterToolsMessaging(configuration);

        services.Should().ContainSingle(x => x.ServiceType == typeof(ITemplateRenderer));
        services.Should().ContainSingle(x => x.ServiceType == typeof(INotificationMessageService));
        services.Should().ContainSingle(x => x.ServiceType == typeof(ISystemEmailTemplateService));
        services.Should().NotContain(x => x.ServiceType == typeof(IEmailSender));
        services.Should().NotContain(x => x.ServiceType == typeof(ISmsSender));
    }

    [Fact]
    public void AddBrighterToolsMailKitEmailSender_RegistersEmailSender()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Smtp:Host"] = "smtp.example.com",
                ["Smtp:Port"] = "587",
                ["Smtp:SecureSocketOption"] = "StartTls",
                ["Email:FromName"] = "System",
                ["Email:FromAddress"] = "noreply@example.com"
            })
            .Build();

        var services = new ServiceCollection();
        services.AddBrighterToolsMessaging(configuration);
        services.AddBrighterToolsMailKitEmailSender(configuration);

        var provider = services.BuildServiceProvider();
        provider.GetRequiredService<IEmailSender>().Should().NotBeNull();
    }

    [Fact]
    public void AddBrighterToolsPostmarkEmailSender_RegistersEmailSender()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Postmark:ServerToken"] = "token",
                ["Email:FromName"] = "System",
                ["Email:FromAddress"] = "noreply@example.com"
            })
            .Build();

        var services = new ServiceCollection();
        services.AddBrighterToolsMessaging(configuration);
        services.AddBrighterToolsPostmarkEmailSender(configuration);

        var provider = services.BuildServiceProvider();
        provider.GetRequiredService<IEmailSender>().Should().NotBeNull();
    }

    [Fact]
    public void AddBrighterToolsSendGridEmailSender_RegistersEmailSender()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["SendGrid:ApiKey"] = "api-key",
                ["Email:FromName"] = "System",
                ["Email:FromAddress"] = "noreply@example.com"
            })
            .Build();

        var services = new ServiceCollection();
        services.AddBrighterToolsMessaging(configuration);
        services.AddBrighterToolsSendGridEmailSender(configuration);

        var provider = services.BuildServiceProvider();
        provider.GetRequiredService<IEmailSender>().Should().NotBeNull();
    }
    [Fact]
    public void AddBrighterToolsTwilioSmsSender_RegistersSmsSender()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Twilio:AccountId"] = "account-id",
                ["Twilio:AuthToken"] = "auth-token"
            })
            .Build();

        var services = new ServiceCollection();
        services.AddBrighterToolsMessaging(configuration);
        services.AddBrighterToolsTwilioSmsSender(configuration);

        var provider = services.BuildServiceProvider();
        provider.GetRequiredService<ISmsSender>().Should().NotBeNull();
    }
}
