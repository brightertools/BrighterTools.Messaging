using BrighterTools.Messaging.Enums;
using BrighterTools.Messaging.Models;
using BrighterTools.Messaging.Options;
using BrighterTools.Messaging.Seeds;
using BrighterTools.Messaging.Services;
using BrighterTools.Messaging.Tests.Support;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace BrighterTools.Messaging.Tests;

public class NotificationMessageServiceTests
{
    [Fact]
    public async Task GetSystemEmailTemplateContentByEmailType_RendersBaseTemplateAndMergeFields()
    {
        var store = new InMemorySystemEmailTemplateStore();
        var templateService = new SystemEmailTemplateService(store);
        await templateService.UpsertSeedDataAsync(SystemEmailTemplateSeedCatalog.GetCoreTemplates());

        var service = CreateService(
            notificationStore: new InMemoryNotificationMessageStore(),
            systemTemplateService: templateService,
            emailTemplateStore: new InMemoryEmailTemplateStore(),
            emailSender: new FakeEmailSender(),
            smsSender: new FakeSmsSender(),
            emailOptions: new EmailTransportOptions
            {
                SubjectPrefix = "[WDA]",
                FromAddress = "noreply@example.com",
                FromName = "System",
                TestMode = false
            },
            smsOptions: new SmsTransportOptions());

        var content = await service.GetSystemEmailTemplateContentByEmailTypeAsync(
            EmailType.EmailVerification,
            new Dictionary<string, string>
            {
                ["{{RecipientName}}"] = "Chris",
                ["{{VerificationCode}}"] = "123456",
                ["{{RecipientEmail}}"] = "chris@example.com",
                ["{{SiteUrl}}"] = "https://example.com"
            });

        content.Subject.Should().StartWith("[WDA]");
        content.Html.Should().Contain("123456");
        content.Html.Should().Contain("<html>");
    }

    [Fact]
    public async Task SendEmailAndExecuteNotification_PersistsAndDispatches()
    {
        var notificationStore = new InMemoryNotificationMessageStore();
        var dispatcher = new FakeNotificationDispatcher();
        var emailSender = new FakeEmailSender();

        var service = CreateService(
            notificationStore: notificationStore,
            systemTemplateService: new SystemEmailTemplateService(new InMemorySystemEmailTemplateStore()),
            emailTemplateStore: new InMemoryEmailTemplateStore(),
            emailSender: emailSender,
            smsSender: new FakeSmsSender(),
            emailOptions: new EmailTransportOptions
            {
                FromAddress = "noreply@example.com",
                FromName = "System",
                TestMode = false
            },
            smsOptions: new SmsTransportOptions(),
            dispatcher: dispatcher);

        var queued = await service.SendEmailAsync(
            new EmailRecipientList("Pat", "pat@example.com"),
            "Subject",
            "<p>Body</p>",
            "Body");

        queued.Should().BeTrue();
        notificationStore.Messages.Should().ContainSingle();
        dispatcher.EnqueuedIds.Should().ContainSingle().Which.Should().Be(notificationStore.Messages.Single().Id);

        var executed = await service.ExecuteNotificationAsync(notificationStore.Messages.Single().Id);

        executed.Should().BeTrue();
        emailSender.SentMessages.Should().ContainSingle(x => x.ToEmail == "pat@example.com" && x.Subject == "Subject");
        notificationStore.Messages.Single().Sent.Should().BeTrue();
    }

    [Fact]
    public async Task SendEmailAsync_UsesConfiguredTestAddresses()
    {
        var notificationStore = new InMemoryNotificationMessageStore();

        var service = CreateService(
            notificationStore: notificationStore,
            systemTemplateService: new SystemEmailTemplateService(new InMemorySystemEmailTemplateStore()),
            emailTemplateStore: new InMemoryEmailTemplateStore(),
            emailSender: new FakeEmailSender(),
            smsSender: new FakeSmsSender(),
            emailOptions: new EmailTransportOptions
            {
                FromAddress = "noreply@example.com",
                FromName = "System",
                TestMode = true,
                TestModeAddresses = ["test1@example.com", "test2@example.com"]
            },
            smsOptions: new SmsTransportOptions());

        var queued = await service.SendEmailAsync(
            new EmailRecipientList("Pat", "pat@example.com"),
            "Subject",
            "<p>Hello {{RecipientEmailAddress}}</p>",
            "Hello {{RecipientEmailAddress}}");

        queued.Should().BeTrue();
        notificationStore.Messages.Select(x => x.ToEmail).Should().BeEquivalentTo(["test1@example.com", "test2@example.com"]);
        notificationStore.Messages.Should().OnlyContain(x => x.Subject == "[Test] Subject");
    }

    [Fact]
    public async Task SendSmsAsync_UsesConfiguredTestNumbers()
    {
        var notificationStore = new InMemoryNotificationMessageStore();

        var service = CreateService(
            notificationStore: notificationStore,
            systemTemplateService: new SystemEmailTemplateService(new InMemorySystemEmailTemplateStore()),
            emailTemplateStore: new InMemoryEmailTemplateStore(),
            emailSender: new FakeEmailSender(),
            smsSender: new FakeSmsSender(),
            emailOptions: new EmailTransportOptions(),
            smsOptions: new SmsTransportOptions
            {
                FromName = "System",
                FromPhoneNumber = "+1000000000",
                TestMode = true,
                TestModeNumberList = ["+447700900001", "+447700900002"]
            });

        var queued = await service.SendSmsAsync(["+123", "+123", "+456"], "Body", null);

        queued.Should().BeTrue();
        notificationStore.Messages.Select(x => x.ToMobile).Should().BeEquivalentTo(["+447700900001", "+447700900002"]);
        notificationStore.Messages.Should().OnlyContain(x => x.FromMobile == "+1000000000");
    }

    private static NotificationMessageService CreateService(
        InMemoryNotificationMessageStore notificationStore,
        SystemEmailTemplateService systemTemplateService,
        InMemoryEmailTemplateStore emailTemplateStore,
        FakeEmailSender emailSender,
        FakeSmsSender smsSender,
        EmailTransportOptions emailOptions,
        SmsTransportOptions smsOptions,
        FakeNotificationDispatcher? dispatcher = null,
        InMemoryNotificationAttachmentStore? attachmentStore = null)
    {
        return new NotificationMessageService(
            notificationStore,
            systemTemplateService,
            emailTemplateStore,
            new TemplateRenderer(),
            emailSender,
            smsSender,
            Microsoft.Extensions.Options.Options.Create(emailOptions),
            Microsoft.Extensions.Options.Options.Create(smsOptions),
            NullLogger<NotificationMessageService>.Instance,
            dispatcher,
            attachmentStore);
    }
}
