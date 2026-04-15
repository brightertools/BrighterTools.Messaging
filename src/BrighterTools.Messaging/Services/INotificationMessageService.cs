using BrighterTools.Messaging.Enums;
using BrighterTools.Messaging.Models;
namespace BrighterTools.Messaging.Services;
/// <summary>
/// Defines operations for Notification Message Service.
/// </summary>
public interface INotificationMessageService
{
    /// <summary>
    /// Sends the send Email Async.
    /// </summary>
    /// <param name="recipients">The recipients value.</param>
    /// <param name="subject">The subject value.</param>
    /// <param name="bodyHtml">The bodyHtml value.</param>
    /// <param name="bodyText">The bodyText value.</param>
    /// <param name="fromAddress">The fromAddress value.</param>
    /// <param name="fromName">The fromName value.</param>
    /// <param name="attachments">The attachments value.</param>
    /// <param name="cancellationToken">The cancellationToken value.</param>
    /// <returns>A task that represents the asynchronous operation and contains the operation result.</returns>
    Task<bool> SendEmailAsync(EmailRecipientList recipients, string subject, string bodyHtml, string bodyText, string? fromAddress = null, string? fromName = null, IEnumerable<EmailAttachment>? attachments = null, CancellationToken cancellationToken = default);
    /// <summary>
    /// Sends the send Sms Async.
    /// </summary>
    /// <param name="recipients">The recipients value.</param>
    /// <param name="bodyText">The bodyText value.</param>
    /// <param name="fromNumber">The fromNumber value.</param>
    /// <param name="fromName">The fromName value.</param>
    /// <param name="cancellationToken">The cancellationToken value.</param>
    /// <returns>A task that represents the asynchronous operation and contains the operation result.</returns>
    Task<bool> SendSmsAsync(IEnumerable<string> recipients, string bodyText, string? fromNumber, string? fromName = null, CancellationToken cancellationToken = default);
    /// <summary>
    /// Executes the execute Notification Async operation.
    /// </summary>
    /// <param name="id">The id value.</param>
    /// <param name="cancellationToken">The cancellationToken value.</param>
    /// <returns>A task that represents the asynchronous operation and contains the operation result.</returns>
    Task<bool> ExecuteNotificationAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets the get System Email Template Content By Email Type Async.
    /// </summary>
    /// <param name="emailType">The emailType value.</param>
    /// <param name="mergeFieldData">The mergeFieldData value.</param>
    /// <param name="includeSubjectPrefix">The includeSubjectPrefix value.</param>
    /// <param name="cancellationToken">The cancellationToken value.</param>
    /// <returns>A task that represents the asynchronous operation and contains the operation result.</returns>
    Task<EmailContent> GetSystemEmailTemplateContentByEmailTypeAsync(EmailType emailType, IReadOnlyDictionary<string, string>? mergeFieldData = null, bool includeSubjectPrefix = true, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets the get Email Template Content By Id Async.
    /// </summary>
    /// <param name="emailTemplateId">The emailTemplateId value.</param>
    /// <param name="mergeFieldData">The mergeFieldData value.</param>
    /// <param name="includeSubjectPrefix">The includeSubjectPrefix value.</param>
    /// <param name="cancellationToken">The cancellationToken value.</param>
    /// <returns>A task that represents the asynchronous operation and contains the operation result.</returns>
    Task<EmailContent> GetEmailTemplateContentByIdAsync(int emailTemplateId, IReadOnlyDictionary<string, string>? mergeFieldData = null, bool includeSubjectPrefix = true, CancellationToken cancellationToken = default);
}

