using BrighterTools.Messaging.Abstractions;
using BrighterTools.Messaging.Enums;
using BrighterTools.Messaging.Models;
using BrighterTools.Messaging.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BrighterTools.Messaging.Services;

/// <summary>
/// Provides Notification Message operations.
/// </summary>
public class NotificationMessageService(
    INotificationMessageStore notificationMessageStore,
    ISystemEmailTemplateService systemEmailTemplateService,
    IEmailTemplateStore emailTemplateStore,
    ITemplateRenderer templateRenderer,
    IEmailSender emailSender,
    ISmsSender smsSender,
    IOptions<EmailTransportOptions> emailOptions,
    IOptions<SmsTransportOptions> smsOptions,
    ILogger<NotificationMessageService> logger,
    INotificationDispatcher? dispatcher = null,
    INotificationAttachmentStore? attachmentStore = null) : INotificationMessageService
{
    /// <summary>
    /// Sends Email.
    /// </summary>
    public async Task<bool> SendEmailAsync(EmailRecipientList recipients, string subject, string bodyHtml, string bodyText, string? fromAddress = null, string? fromName = null, IEnumerable<EmailAttachment>? attachments = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var configuredEmail = emailOptions.Value;
            var deDuplicatedRecipients = recipients.GroupBy(x => x.EmailAddress, StringComparer.OrdinalIgnoreCase).Select(x => x.First()).ToList();
            if (configuredEmail.TestMode && configuredEmail.TestModeAddresses.Count > 0)
            {
                deDuplicatedRecipients = [.. deDuplicatedRecipients.SelectMany(recipient => configuredEmail.TestModeAddresses.Select(testAddress => new EmailRecipient(recipient.Name, testAddress)))];
                subject = $"[Test] {subject}";
            }

            foreach (var recipient in deDuplicatedRecipients)
            {
                var message = new NotificationMessage
                {
                    ToName = recipient.Name,
                    ToEmail = recipient.EmailAddress,
                    Subject = subject,
                    Html = bodyHtml.Replace("{{RecipientEmailAddress}}", recipient.EmailAddress, StringComparison.Ordinal).Trim(),
                    Text = bodyText.Replace("{{RecipientEmailAddress}}", recipient.EmailAddress, StringComparison.Ordinal).Trim(),
                    FromName = fromName ?? configuredEmail.FromName,
                    FromAddress = fromAddress ?? configuredEmail.FromAddress,
                    NotificationMessageType = NotificationMessageType.Email,
                    Attachments = attachmentStore == null
                        ? attachments?.OrderBy(x => x.Order).Select(x => new NotificationMessageAttachment
                        {
                            FileName = x.Name,
                            Content = x.Content,
                            ContentType = x.ContentType,
                            ContentId = x.ContentId,
                            Order = x.Order
                        }).ToList() ?? []
                        : []
                };

                var createdMessage = await notificationMessageStore.CreateAsync(message, cancellationToken);
                if (attachmentStore != null && attachments != null)
                {
                    await attachmentStore.SaveAsync(createdMessage.Id, attachments, cancellationToken);
                }

                if (dispatcher != null)
                {
                    await dispatcher.EnqueueAsync(createdMessage.Id, cancellationToken);
                }
            }

            return true;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error queueing email notification");
            return false;
        }
    }

    /// <summary>
    /// Sends SMS.
    /// </summary>
    public async Task<bool> SendSmsAsync(IEnumerable<string> recipients, string bodyText, string? fromNumber, string? fromName = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var configuredSms = smsOptions.Value;
            var deDuplicatedRecipients = recipients.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            if (configuredSms.TestMode && configuredSms.TestModeNumberList.Count > 0)
            {
                deDuplicatedRecipients = configuredSms.TestModeNumberList.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            }
            foreach (var recipient in deDuplicatedRecipients)
            {
                var message = new NotificationMessage
                {
                    ToName = string.Empty,
                    ToMobile = recipient,
                    Text = bodyText.Trim(),
                    FromName = fromName ?? configuredSms.FromName,
                    FromMobile = string.IsNullOrWhiteSpace(fromNumber) ? configuredSms.FromPhoneNumber : fromNumber,
                    NotificationMessageType = NotificationMessageType.Sms
                };
                var createdMessage = await notificationMessageStore.CreateAsync(message, cancellationToken);
                if (dispatcher != null)
                {
                    await dispatcher.EnqueueAsync(createdMessage.Id, cancellationToken);
                }
            }
            return true;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error queueing sms notification");
            return false;
        }
    }

    /// <summary>
    /// Executes Execute Notification.
    /// </summary>
    public async Task<bool> ExecuteNotificationAsync(int id, CancellationToken cancellationToken = default)
    {
        var notification = await notificationMessageStore.GetByIdAsync(id, cancellationToken);
        if (notification == null || notification.Sent)
        {
            return false;
        }
        try
        {
            switch (notification.NotificationMessageType)
            {
                case NotificationMessageType.Email:
                    var emailAttachments = notification.Attachments.Count > 0
                        ? notification.Attachments.Select(x => new EmailAttachment { Name = x.FileName, Content = x.Content, ContentType = x.ContentType, ContentId = x.ContentId, Order = x.Order }).ToList()
                        : attachmentStore != null
                            ? [.. await attachmentStore.GetAsync(id, cancellationToken)]
                            : [];
                    await emailSender.SendAsync(notification.ToName, notification.ToEmail ?? string.Empty, notification.Subject ?? string.Empty, notification.Html ?? string.Empty, notification.Text ?? string.Empty, notification.FromName, notification.FromAddress, emailAttachments, cancellationToken);
                    break;
                case NotificationMessageType.Sms:
                    await smsSender.SendAsync(notification.ToMobile ?? string.Empty, notification.FromMobile ?? string.Empty, notification.Text ?? string.Empty, cancellationToken);
                    break;
            }
            notification.Sent = true;
            notification.SentDate = DateTimeOffset.UtcNow;
            notification.LastUpdatedDate = DateTimeOffset.UtcNow;
            await notificationMessageStore.UpdateAsync(notification, cancellationToken);
            return true;
        }
        catch (Exception exception)
        {
            notification.FailureCount += 1;
            notification.LastFailedDate = DateTimeOffset.UtcNow;
            notification.LastFailedMessage = exception.Message;
            notification.LastUpdatedDate = DateTimeOffset.UtcNow;
            await notificationMessageStore.UpdateAsync(notification, cancellationToken);
            logger.LogError(exception, "Error executing notification {NotificationId}", id);
            throw;
        }
    }

    /// <summary>
    /// Gets System Email Template Content BY Email Type.
    /// </summary>
    public async Task<EmailContent> GetSystemEmailTemplateContentByEmailTypeAsync(EmailType emailType, IReadOnlyDictionary<string, string>? mergeFieldData = null, bool includeSubjectPrefix = true, CancellationToken cancellationToken = default)
    {
        var systemEmailTemplate = await systemEmailTemplateService.GetByEmailTypeAsync(emailType, cancellationToken) ?? throw new InvalidOperationException($"System email template not found for {emailType}.");
        return await BuildTemplateContentAsync(systemEmailTemplate.Subject, systemEmailTemplate.HtmlContent, systemEmailTemplate.TextContent, systemEmailTemplate.BaseTemplateId, mergeFieldData, includeSubjectPrefix, cancellationToken);
    }

    /// <summary>
    /// Gets Email Template Content BY ID.
    /// </summary>
    public async Task<EmailContent> GetEmailTemplateContentByIdAsync(int emailTemplateId, IReadOnlyDictionary<string, string>? mergeFieldData = null, bool includeSubjectPrefix = true, CancellationToken cancellationToken = default)
    {
        var emailTemplate = await emailTemplateStore.GetByIdAsync(emailTemplateId, cancellationToken) ?? throw new InvalidOperationException("Email Template not found.");
        return await BuildTemplateContentAsync(emailTemplate.Subject, emailTemplate.HtmlContent, emailTemplate.TextContent, emailTemplate.BaseTemplateId, mergeFieldData, includeSubjectPrefix, cancellationToken);
    }

    private async Task<EmailContent> BuildTemplateContentAsync(string subject, string htmlContent, string textContent, int? baseTemplateId, IReadOnlyDictionary<string, string>? mergeFieldData, bool includeSubjectPrefix, CancellationToken cancellationToken)
    {
        var content = new EmailContent { Subject = subject, Html = htmlContent, Text = textContent };
        if (baseTemplateId.HasValue)
        {
            var baseTemplate = await systemEmailTemplateService.GetByIdAsync(baseTemplateId.Value, cancellationToken);
            if (baseTemplate != null)
            {
                content = templateRenderer.ApplyBaseTemplate(content, new EmailContent { Subject = baseTemplate.Subject, Html = baseTemplate.HtmlContent, Text = baseTemplate.TextContent });
            }
        }
        return templateRenderer.Render(content, mergeFieldData, includeSubjectPrefix ? emailOptions.Value.SubjectPrefix : null);
    }
}

