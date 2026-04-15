using BrighterTools.Messaging.Enums;

namespace BrighterTools.Messaging.Seeds;

/// <summary>
/// Represents System Email Template Seed Catalog.
/// </summary>
public static class SystemEmailTemplateSeedCatalog
{
    /// <summary>
    /// Gets Core Templates.
    /// </summary>
    public static IReadOnlyList<SystemEmailTemplateSeedDefinition> GetCoreTemplates()
    {
        return
        [
            new(
                EmailType.BaseTemplate,
                "Base Email Layout",
                "Base Layout",
                "Outer email layout used by content templates.",
                "{{ContentTemplate}}",
                "{{ContentTemplate}}",
                "<html><body style=\"font-family: Arial, sans-serif;\"><div>{{ContentTemplate}}</div></body></html>",
                "{{ContentTemplate}}",
                BaseTemplateEmailType: null),
            Create(EmailType.Generic, "Generic Email", "Message from {{SiteName}}", "Generic email template.", "{{Message}}", "{{Heading}}, {{Message}}, {{SiteName}}", "<p>{{Message}}</p>"),
            Create(EmailType.SystemNotification, "System Notification", "System notification", "Administrative system notification.", "{{Message}}", "{{Heading}}, {{Message}}, {{SiteName}}", "<p>{{Message}}</p>"),
            Create(EmailType.EmailVerification, "Email Verification", "Verify your email address", "Verification email sent by code.", "{{RecipientName}}, {{RecipientEmail}}, {{VerificationCode}}", "{{RecipientName}}, {{RecipientEmail}}, {{VerificationCode}}, {{SiteUrl}}", "<p>Hi {{RecipientName}}, your verification code is <strong>{{VerificationCode}}</strong>.</p>"),
            Create(EmailType.PhoneVerification, "Phone Verification", "Verify your mobile phone number", "SMS/phone verification companion template.", "{{RecipientName}}, {{RecipientPhone}}, {{VerificationCode}}", "{{RecipientName}}, {{RecipientPhone}}, {{VerificationCode}}, {{SiteUrl}}", "<p>Hi {{RecipientName}}, your mobile verification code is <strong>{{VerificationCode}}</strong>.</p>"),
            Create(EmailType.AccountActivation, "Account Activation", "Activate your account", "Email sent after signup to activate an account.", "{{RecipientFirstName}}, {{RecipientEmail}}, {{ActivationUrl}}", "{{RecipientFirstName}}, {{RecipientLastName}}, {{RecipientEmail}}, {{ActivationUrl}}, {{SiteUrl}}", "<p>Hi {{RecipientFirstName}}, activate your account here: <a href=\"{{ActivationUrl}}\">{{ActivationUrl}}</a>.</p>"),
            Create(EmailType.AccountActivatedConfirmation, "Account Activated Confirmation", "Your account is now activated", "Confirmation after account activation.", "{{RecipientFirstName}}, {{RecipientEmail}}, {{LoginUrl}}", "{{RecipientFirstName}}, {{RecipientLastName}}, {{RecipientEmail}}, {{LoginUrl}}, {{SiteUrl}}", "<p>Hi {{RecipientFirstName}}, your account is now active. Log in at <a href=\"{{LoginUrl}}\">{{LoginUrl}}</a>.</p>"),
            Create(EmailType.AccountCreatedConfirmation, "Account Created Confirmation", "Account created", "Confirmation after an account is created.", "{{RecipientFirstName}}, {{RecipientEmail}}, {{LoginUrl}}", "{{RecipientFirstName}}, {{RecipientLastName}}, {{RecipientEmail}}, {{LoginUrl}}, {{SiteUrl}}, {{TenantName}}", "<p>Hi {{RecipientFirstName}}, your account has been created. Log in at <a href=\"{{LoginUrl}}\">{{LoginUrl}}</a>.</p>"),
            Create(EmailType.EmailConfirmationRequest, "Email Confirmation Request", "Confirm your email address", "Email confirmation request.", "{{RecipientFirstName}}, {{RecipientEmail}}, {{ConfirmationUrl}}", "{{RecipientFirstName}}, {{RecipientLastName}}, {{RecipientEmail}}, {{ConfirmationUrl}}, {{SiteUrl}}", "<p>Hi {{RecipientFirstName}}, confirm your email address here: <a href=\"{{ConfirmationUrl}}\">{{ConfirmationUrl}}</a>.</p>"),
            Create(EmailType.EmailConfirmedNotification, "Email Confirmed Notification", "Email address confirmed", "Notification that email confirmation completed.", "{{RecipientFirstName}}, {{RecipientEmail}}", "{{RecipientFirstName}}, {{RecipientLastName}}, {{RecipientEmail}}, {{SiteUrl}}", "<p>Hi {{RecipientFirstName}}, your email address has been confirmed.</p>"),
            Create(EmailType.RequestPasswordReset, "Password Reset Request", "Reset your password", "Password reset email.", "{{RecipientFirstName}}, {{RecipientEmail}}, {{PasswordResetUrl}}", "{{RecipientFirstName}}, {{RecipientLastName}}, {{RecipientEmail}}, {{PasswordResetUrl}}, {{PasswordResetUrlExpiryDate}}, {{PasswordResetUrlExpiryInHours}}", "<p>Hi {{RecipientFirstName}}, reset your password here: <a href=\"{{PasswordResetUrl}}\">{{PasswordResetUrl}}</a>. The link expires on {{PasswordResetUrlExpiryDate}}.</p>"),
            Create(EmailType.PasswordResetConfirmation, "Password Reset Confirmation", "Password updated", "Password reset confirmation.", "{{RecipientFirstName}}, {{RecipientEmail}}, {{LoginUrl}}", "{{RecipientFirstName}}, {{RecipientLastName}}, {{RecipientEmail}}, {{LoginUrl}}", "<p>Hi {{RecipientFirstName}}, your password has been updated. Log in at <a href=\"{{LoginUrl}}\">{{LoginUrl}}</a>.</p>"),
            Create(EmailType.UserInvitation, "User Invitation", "User account invitation", "User invitation email.", "{{InvitationRecipientFirstName}}, {{InvitationRecipientEmail}}, {{SenderFullName}}, {{TenantName}}, {{InvitationUrl}}", "{{InvitationRecipientFirstName}}, {{InvitationRecipientLastName}}, {{InvitationRecipientEmail}}, {{SenderFullName}}, {{SenderEmail}}, {{TenantName}}, {{InvitationUrl}}, {{ExpiryDate}}, {{SiteUrl}}, {{Message}}", "<p>Hi {{InvitationRecipientFirstName}}, {{SenderFullName}} has invited you to {{TenantName}}. Accept here: <a href=\"{{InvitationUrl}}\">{{InvitationUrl}}</a>.</p><p>{{Message}}</p>"),
            Create(EmailType.UserInvitationResponseNotification, "User Invitation Response", "User invitation response", "Response notification to the sender.", "{{InvitationRecipientFirstName}}, {{InvitationRecipientEmail}}, {{SenderFullName}}, {{ResponseStatus}}", "{{InvitationRecipientFirstName}}, {{InvitationRecipientLastName}}, {{InvitationRecipientEmail}}, {{SenderFullName}}, {{SenderEmail}}, {{TenantName}}, {{SiteUrl}}, {{ResponseStatus}}, {{ResponseMessage}}", "<p>Hi {{SenderFullName}}, {{InvitationRecipientFirstName}} has responded: {{ResponseStatus}}.</p><p>{{ResponseMessage}}</p>")
        ];
    }

    /// <summary>
    /// Gets World Drinks Awards Templates.
    /// </summary>
    public static IReadOnlyList<SystemEmailTemplateSeedDefinition> GetWorldDrinksAwardsTemplates()
    {
        return
        [
            Create(EmailType.AdHocInvoiceInvoiced, "Ad Hoc Invoice Invoiced", "Your invoice is ready", "WDA ad hoc invoice email.", "{{FirstName}}, {{CompetitionName}}", "{{FirstName}}, {{CompetitionName}}, {{FeatureList}}, {{ContactUsEmailAddress}}", "<p>Hi {{FirstName}}, your invoice for {{CompetitionName}} is ready.</p><p>{{FeatureList}}</p><p>Contact us: {{ContactUsEmailAddress}}</p>"),
            Create(EmailType.AdHocInvoicePaid, "Ad Hoc Invoice Paid", "Your invoice has been paid", "WDA ad hoc invoice paid email.", "{{RecipientFirstName}}, {{ActivationLink}}", "{{RecipientFirstName}}, {{ActivationLink}}, {{ExpiryDate}}", "<p>Hi {{RecipientFirstName}}, your invoice has been paid. Activate here: <a href=\"{{ActivationLink}}\">{{ActivationLink}}</a> before {{ExpiryDate}}.</p>"),
            Create(EmailType.CreditControlChaseEmail, "Credit Control Chase", "Payment reminder", "WDA credit control reminder.", "{{RecipientFirstName}}, {{ResetLink}}", "{{RecipientFirstName}}, {{ResetLink}}, {{SupportEmail}}", "<p>Hi {{RecipientFirstName}}, this is a payment reminder. Details: <a href=\"{{ResetLink}}\">{{ResetLink}}</a>. Support: {{SupportEmail}}</p>"),
            Create(EmailType.CreditNote, "Credit Note", "Credit note available", "WDA credit note notification.", "{{RecipientFirstName}}, {{ConfirmationLink}}", "{{RecipientFirstName}}, {{ConfirmationLink}}", "<p>Hi {{RecipientFirstName}}, your credit note is ready. Review it here: <a href=\"{{ConfirmationLink}}\">{{ConfirmationLink}}</a>.</p>"),
            Create(EmailType.DrinksEntryInvoiceEmailToAdmin, "Drinks Entry Invoice To Admin", "New drinks entry invoice", "Admin invoice alert.", "{{FirstName}}, {{CompetitionName}}", "{{FirstName}}, {{CompetitionName}}, {{JudgingDate}}, {{Venue}}", "<p>{{FirstName}} has created a drinks entry invoice for {{CompetitionName}}.</p><p>Judging date: {{JudgingDate}}. Venue: {{Venue}}</p>"),
            Create(EmailType.DrinksEntryOrderConfirmation, "Drinks Entry Order Confirmation", "Your order has been received", "Drinks entry order confirmation.", "{{FirstName}}, {{CompetitionName}}", "{{FirstName}}, {{CompetitionName}}, {{ResultsLink}}", "<p>Hi {{FirstName}}, your order for {{CompetitionName}} has been received.</p><p>Results link: <a href=\"{{ResultsLink}}\">{{ResultsLink}}</a></p>"),
            Create(EmailType.DrinksEntryOrderInvoiceInvoiced, "Drinks Entry Order Invoice Invoiced", "Invoice issued for your order", "Drinks entry invoice issued.", "{{FirstName}}, {{CompetitionName}}", "{{FirstName}}, {{CompetitionName}}, {{OrderedItemsHtml}}, {{ContactUsEmailAddress}}", "<p>Hi {{FirstName}}, your invoice for {{CompetitionName}} is ready.</p><div>{{OrderedItemsHtml}}</div><p>Contact us: {{ContactUsEmailAddress}}</p>"),
            Create(EmailType.DrinksEntryOrderInvoicePaidOffline, "Drinks Entry Order Invoice Paid Offline", "Offline payment received", "Drinks entry invoice paid offline.", "{{FirstName}}, {{InvoiceNumber}}", "{{FirstName}}, {{InvoiceNumber}}, {{CompetitionName}}", "<p>Hi {{FirstName}}, offline payment for invoice {{InvoiceNumber}} has been recorded for {{CompetitionName}}.</p>"),
            Create(EmailType.DrinksEntryOrderInvoicePaidOnline, "Drinks Entry Order Invoice Paid Online", "Online payment received", "Drinks entry invoice paid online.", "{{FirstName}}, {{CompetitionName}}", "{{FirstName}}, {{CompetitionName}}, {{OrderedItemsHtml}}, {{ContactUsEmailAddress}}", "<p>Hi {{FirstName}}, your online payment for {{CompetitionName}} has been received.</p><div>{{OrderedItemsHtml}}</div><p>Contact us: {{ContactUsEmailAddress}}</p>"),
            Create(EmailType.DrinksEntryReceivedConfirmation, "Drinks Entry Received Confirmation", "Entry received", "Drinks entry received confirmation.", "{{FirstName}}, {{EntryId}}", "{{FirstName}}, {{EntryId}}, {{ReceivedDate}}", "<p>Hi {{FirstName}}, entry {{EntryId}} was received on {{ReceivedDate}}.</p>"),
            Create(EmailType.GenericEmailWithLink, "Generic Email With Link", "{{Heading}}", "Generic message template with CTA link.", "{{Heading}}, {{Message}}, {{LinkUrl}}", "{{Heading}}, {{Name}}, {{Message}}, {{LinkUrl}}, {{LinkText}}, {{FromName}}", "<h2>{{Heading}}</h2><p>Hi {{Name}},</p><p>{{Message}}</p><p><a href=\"{{LinkUrl}}\">{{LinkText}}</a></p><p>{{FromName}}</p>"),
            Create(EmailType.IconsEntryInvoiceInvoiced, "Icons Entry Invoice Invoiced", "Icons entry invoice issued", "Icons entry invoice issued.", "{{FirstName}}, {{CompetitionName}}", "{{FirstName}}, {{CompetitionName}}, {{OrderedItemsHtml}}, {{FromEmailAddress}}", "<p>Hi {{FirstName}}, your icons entry invoice for {{CompetitionName}} is ready.</p><div>{{OrderedItemsHtml}}</div><p>{{FromEmailAddress}}</p>"),
            Create(EmailType.IconsEntryInvoicePaid, "Icons Entry Invoice Paid", "Icons entry invoice paid", "Icons entry invoice paid.", "{{FirstName}}, {{CompetitionName}}", "{{FirstName}}, {{CompetitionName}}, {{OrderedItemsHtml}}, {{FromEmailAddress}}", "<p>Hi {{FirstName}}, payment for your icons entry invoice for {{CompetitionName}} has been received.</p><div>{{OrderedItemsHtml}}</div><p>{{FromEmailAddress}}</p>"),
            Create(EmailType.PromoOrderInvoiceInvoiced, "Promo Order Invoice Invoiced", "Promo order invoice issued", "Promo order invoice issued.", "{{FirstName}}", "{{FirstName}}, {{OrderedItemsHtml}}, {{ContactUsEmailAddress}}", "<p>Hi {{FirstName}}, your promo order invoice is ready.</p><div>{{OrderedItemsHtml}}</div><p>{{ContactUsEmailAddress}}</p>"),
            Create(EmailType.PromoOrderInvoicePaidOffline, "Promo Order Invoice Paid Offline", "Promo order offline payment received", "Promo order invoice paid offline.", "{{FirstName}}, {{InvoiceNumber}}", "{{FirstName}}, {{InvoiceNumber}}, {{PaidAmount}}, {{PaymentDate}}", "<p>Hi {{FirstName}}, invoice {{InvoiceNumber}} has been paid offline. Amount: {{PaidAmount}} on {{PaymentDate}}.</p>"),
            Create(EmailType.PromoOrderInvoicePaidOnline, "Promo Order Invoice Paid Online", "Promo order online payment received", "Promo order invoice paid online.", "{{FirstName}}", "{{FirstName}}, {{OrderedItemsHtml}}, {{ContactUsEmailAddress}}", "<p>Hi {{FirstName}}, your promo order payment has been received.</p><div>{{OrderedItemsHtml}}</div><p>{{ContactUsEmailAddress}}</p>"),
            Create(EmailType.ShipmentOrderAdminEmail, "Shipment Order Admin Email", "New shipment order", "Shipment order admin notification.", "{{CompanyName}}, {{CompetitionName}}", "{{CompanyName}}, {{CompetitionName}}, {{InvoiceToName}}, {{InvoiceToAddress}}, {{DeliverToAddress}}", "<p>New shipment order for {{CompanyName}} / {{CompetitionName}}.</p><p>Invoice to: {{InvoiceToName}}, {{InvoiceToAddress}}</p><p>Deliver to: {{DeliverToAddress}}</p>"),
            Create(EmailType.ShipmentOrderInvoiceInvoiced, "Shipment Order Invoice Invoiced", "Shipment order invoice issued", "Shipment order invoice issued.", "{{FirstName}}, {{CompetitionName}}", "{{FirstName}}, {{CompetitionName}}, {{OrderedItemsHtml}}, {{ContactUsEmailAddress}}, {{CompetitionTypeName}}", "<p>Hi {{FirstName}}, your shipment order invoice for {{CompetitionName}} ({{CompetitionTypeName}}) is ready.</p><div>{{OrderedItemsHtml}}</div><p>{{ContactUsEmailAddress}}</p>"),
            Create(EmailType.ShipmentOrderInvoicePaidOffline, "Shipment Order Invoice Paid Offline", "Shipment order offline payment received", "Shipment order invoice paid offline.", "{{FirstName}}, {{InvoiceNumber}}", "{{FirstName}}, {{InvoiceNumber}}, {{PaidAmount}}, {{PaymentDate}}", "<p>Hi {{FirstName}}, invoice {{InvoiceNumber}} has been paid offline. Amount: {{PaidAmount}} on {{PaymentDate}}.</p>"),
            Create(EmailType.ShipmentOrderInvoicePaidOnline, "Shipment Order Invoice Paid Online", "Shipment order online payment received", "Shipment order invoice paid online.", "{{FirstName}}, {{CompetitionName}}", "{{FirstName}}, {{CompetitionName}}, {{OrderedItemsHtml}}, {{ContactUsEmailAddress}}, {{CompetitionTypeName}}", "<p>Hi {{FirstName}}, your shipment order payment for {{CompetitionName}} ({{CompetitionTypeName}}) has been received.</p><div>{{OrderedItemsHtml}}</div><p>{{ContactUsEmailAddress}}</p>"),
            Create(EmailType.InvoicePaid, "Invoice Paid", "Invoice paid", "WDA invoice paid template.", "{{RecipientFirstName}}, {{InvoiceReference}}", "{{RecipientFirstName}}, {{InvoiceReference}}, {{InvoiceAmount}}, {{SiteUrl}}", "<p>Hi {{RecipientFirstName}}, invoice {{InvoiceReference}} has been paid. Amount: {{InvoiceAmount}}.</p>"),
            Create(EmailType.InvoicePaymentPending, "Invoice Payment Pending", "Invoice payment pending", "WDA invoice pending template.", "{{RecipientFirstName}}, {{InvoiceReference}}", "{{RecipientFirstName}}, {{InvoiceReference}}, {{PaymentLink}}, {{InvoiceAmount}}", "<p>Hi {{RecipientFirstName}}, invoice {{InvoiceReference}} is awaiting payment.</p><p>Amount: {{InvoiceAmount}}</p><p>Pay here: <a href=\"{{PaymentLink}}\">{{PaymentLink}}</a></p>"),
            Create(EmailType.InvoiceConfirmOrder, "Invoice Confirm Order", "Order confirmed", "WDA invoice order confirmation template.", "{{RecipientFirstName}}, {{OrderReference}}", "{{RecipientFirstName}}, {{OrderReference}}, {{InvoiceAmount}}, {{PaymentLink}}", "<p>Hi {{RecipientFirstName}}, order {{OrderReference}} has been confirmed.</p><p>Amount: {{InvoiceAmount}}</p><p>{{PaymentLink}}</p>")
        ];
    }

    private static SystemEmailTemplateSeedDefinition Create(
        EmailType emailType,
        string name,
        string subject,
        string description,
        string requiredFields,
        string availableFields,
        string htmlBody)
    {
        var textBody = htmlBody
            .Replace("<p>", string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("</p>", Environment.NewLine, StringComparison.OrdinalIgnoreCase)
            .Replace("<div>", string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("</div>", Environment.NewLine, StringComparison.OrdinalIgnoreCase)
            .Replace("<h2>", string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("</h2>", Environment.NewLine, StringComparison.OrdinalIgnoreCase)
            .Replace("<strong>", string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("</strong>", string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("<a href=\"", string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("\">", " ", StringComparison.OrdinalIgnoreCase)
            .Replace("</a>", string.Empty, StringComparison.OrdinalIgnoreCase);

        return new SystemEmailTemplateSeedDefinition(
            emailType,
            name,
            subject,
            description,
            requiredFields,
            availableFields,
            htmlBody,
            textBody);
    }
}

