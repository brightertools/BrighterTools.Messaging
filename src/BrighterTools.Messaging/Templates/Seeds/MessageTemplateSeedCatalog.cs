using BrighterTools.Messaging.Templates.Models;

namespace BrighterTools.Messaging.Templates.Seeds;

public static class MessageTemplateSeedCatalog
{
    public static IReadOnlyList<MessageTemplateSeedDefinition> GetCoreTemplates()
    {
        return
        [
            new(
                "layout.base",
                "Base Email Layout",
                "Outer email layout used by content templates.",
                "Layouts",
                "{{Subject}}",
                "<html><body style=\"font-family: Arial, sans-serif; background:#f4f2f7; margin:0; padding:32px;\"><div style=\"max-width:640px; margin:0 auto; background:#ffffff; border-radius:12px; overflow:hidden;\"><div style=\"background:#403460; color:#ffffff; padding:24px 32px; font-size:28px; font-weight:bold;\">{{SiteName}}</div><div style=\"padding:32px;\">{{ContentTemplate}}</div></div></body></html>",
                "{{ContentTemplate}}",
                [Field("{{ContentTemplate}}", required: true, isHtml: true, sample: "<p>Example content</p>")],
                [Field("{{SiteName}}", sample: "adfast"), Field("{{Subject}}", sample: "Message from adfast"), Field("{{ContentTemplate}}", required: true, isHtml: true, sample: "<p>Example content</p>")],
                BaseTemplateKey: null),
            new(
                "auth.signup.emailVerification",
                "Signup Email Verification",
                "Verification code sent before creating an email/password account.",
                "Auth",
                "Verify your email address",
                "<p>Hi {{RecipientName}},</p><p>Your verification code is <strong>{{VerificationCode}}</strong>.</p><p>This challenge expires in {{ExpiryMinutes}} minutes. If this was not you, you can ignore this email.</p>",
                "Hi {{RecipientName}}, your verification code is {{VerificationCode}}. This challenge expires in {{ExpiryMinutes}} minutes.",
                [Field("{{RecipientEmail}}", required: true), Field("{{VerificationCode}}", required: true)],
                CommonAuthFields()),
            new(
                "auth.account.emailVerification",
                "Account Email Verification",
                "Verification email sent when changing or confirming a login email address.",
                "Auth",
                "Confirm your email address",
                "<p>Hi {{RecipientName}},</p><p>Your verification code is <strong>{{VerificationCode}}</strong>.</p><p>{{VerificationLinkHtml}}</p><p>This challenge expires in {{ExpiryMinutes}} minutes.</p>",
                "Hi {{RecipientName}}, your verification code is {{VerificationCode}}. Link: {{VerificationUrl}}. This challenge expires in {{ExpiryMinutes}} minutes.",
                [Field("{{RecipientEmail}}", required: true)],
                CommonAuthFields().Concat([Field("{{VerificationUrl}}", sample: "https://adfast.example/verify-email?token=sample"), Field("{{VerificationLinkHtml}}", isHtml: true, sample: "<a href=\"https://adfast.example/verify-email?token=sample\">Confirm email</a>")]).ToList()),
            new(
                "auth.loginEmail.changeVerification",
                "Login Email Change Verification",
                "Verification email sent when changing login email.",
                "Auth",
                "Confirm your login email change",
                "<p>Hi {{RecipientName}},</p><p>Your verification code is <strong>{{VerificationCode}}</strong>.</p><p>{{VerificationLinkHtml}}</p><p>This challenge expires in {{ExpiryMinutes}} minutes.</p>",
                "Hi {{RecipientName}}, your verification code is {{VerificationCode}}. Link: {{VerificationUrl}}.",
                [Field("{{RecipientEmail}}", required: true)],
                CommonAuthFields().Concat([Field("{{VerificationUrl}}", sample: "https://adfast.example/verify-email?token=sample"), Field("{{VerificationLinkHtml}}", isHtml: true, sample: "<a href=\"https://adfast.example/verify-email?token=sample\">Confirm email</a>")]).ToList()),
            new(
                "auth.notificationEmail.changeVerification",
                "Notification Email Change Verification",
                "Verification email sent when changing notification email.",
                "Auth",
                "Confirm your notification email change",
                "<p>Hi {{RecipientName}},</p><p>Your verification code is <strong>{{VerificationCode}}</strong>.</p><p>{{VerificationLinkHtml}}</p><p>This challenge expires in {{ExpiryMinutes}} minutes.</p>",
                "Hi {{RecipientName}}, your verification code is {{VerificationCode}}. Link: {{VerificationUrl}}.",
                [Field("{{RecipientEmail}}", required: true)],
                CommonAuthFields().Concat([Field("{{VerificationUrl}}", sample: "https://adfast.example/verify-email?token=sample"), Field("{{VerificationLinkHtml}}", isHtml: true, sample: "<a href=\"https://adfast.example/verify-email?token=sample\">Confirm email</a>")]).ToList()),
            new(
                "auth.password.reset",
                "Password Reset Request",
                "Password reset email for the new app auth flow.",
                "Auth",
                "Reset your password",
                "<h1>Reset your password</h1><p>Hi {{RecipientFirstName}},</p><p>We received a request to reset the password for {{RecipientEmail}}.</p><p><a href=\"{{PasswordResetUrl}}\">Reset password</a></p><p>This link expires on {{PasswordResetUrlExpiryDate}}.</p>",
                "Hi {{RecipientFirstName}}, reset your password here: {{PasswordResetUrl}}. This link expires on {{PasswordResetUrlExpiryDate}}.",
                [Field("{{RecipientEmail}}", required: true), Field("{{PasswordResetUrl}}", required: true)],
                [.. CommonIdentityFields(), Field("{{PasswordResetUrl}}", sample: "https://adfast.example/reset-password?token=sample"), Field("{{PasswordResetUrlExpiryDate}}", sample: "15/06/2026 18:00"), Field("{{PasswordResetUrlExpiryInDays}}", sample: "5")]),
            new(
                "auth.password.updated",
                "Password Updated",
                "Confirmation after a password is changed.",
                "Auth",
                "Your password has been updated",
                "<p>Hi {{RecipientFirstName}},</p><p>Your password has been updated.</p><p><a href=\"{{LoginUrl}}\">Log in</a></p>",
                "Hi {{RecipientFirstName}}, your password has been updated. Log in: {{LoginUrl}}",
                [Field("{{RecipientEmail}}", required: true)],
                [.. CommonIdentityFields(), Field("{{LoginUrl}}", sample: "https://adfast.example/login")]),
            new(
                "auth.passwordless.login",
                "Passwordless Login",
                "One-time code/link email for passwordless login.",
                "Auth",
                "Your one-time login code",
                "<p>Hi {{RecipientName}},</p><p>Your one-time login code is <strong>{{VerificationCode}}</strong>.</p><p>{{VerificationLinkHtml}}</p><p>This challenge expires in {{ExpiryMinutes}} minutes.</p>",
                "Hi {{RecipientName}}, your one-time login code is {{VerificationCode}}. Link: {{VerificationUrl}}.",
                [Field("{{RecipientEmail}}", required: true)],
                CommonAuthFields().Concat([Field("{{VerificationUrl}}", sample: "https://adfast.example/login?passwordlessToken=sample"), Field("{{VerificationLinkHtml}}", isHtml: true, sample: "<a href=\"https://adfast.example/login?passwordlessToken=sample\">Log in</a>")]).ToList()),
            new(
                "tenant.userInvitation",
                "User Invitation",
                "Invitation email for tenant/company users.",
                "Tenancy",
                "You have been invited to {{TenantName}}",
                "<p>Hi {{InvitationRecipientFirstName}},</p><p>{{SenderFullName}} has invited you to {{TenantName}}.</p><p><a href=\"{{InvitationUrl}}\">Accept invitation</a></p><p>{{Message}}</p>",
                "Hi {{InvitationRecipientFirstName}}, {{SenderFullName}} has invited you to {{TenantName}}. Accept: {{InvitationUrl}}. {{Message}}",
                [Field("{{InvitationRecipientEmail}}", required: true), Field("{{InvitationUrl}}", required: true)],
                [Field("{{InvitationRecipientFirstName}}", sample: "Jane"), Field("{{InvitationRecipientLastName}}", sample: "Smith"), Field("{{InvitationRecipientEmail}}", sample: "jane@example.com"), Field("{{SenderFullName}}", sample: "Admin User"), Field("{{SenderEmail}}", sample: "admin@example.com"), Field("{{TenantName}}", sample: "Example Agency"), Field("{{InvitationUrl}}", sample: "https://adfast.example/invitation/sample"), Field("{{ExpiryDate}}", sample: "22/06/2026"), Field("{{SiteUrl}}", sample: "https://adfast.example"), Field("{{Message}}", sample: "Please join our account.")]),
            new(
                "tenant.userInvitationResponse",
                "User Invitation Response",
                "Notification to inviter when an invitation is answered.",
                "Tenancy",
                "User invitation response",
                "<p>Hi {{SenderFullName}},</p><p>{{InvitationRecipientFirstName}} has responded: {{ResponseStatus}}.</p><p>{{ResponseMessage}}</p>",
                "Hi {{SenderFullName}}, {{InvitationRecipientFirstName}} has responded: {{ResponseStatus}}. {{ResponseMessage}}",
                [Field("{{SenderFullName}}", required: true), Field("{{ResponseStatus}}", required: true)],
                [Field("{{InvitationRecipientFirstName}}", sample: "Jane"), Field("{{InvitationRecipientEmail}}", sample: "jane@example.com"), Field("{{SenderFullName}}", sample: "Admin User"), Field("{{TenantName}}", sample: "Example Agency"), Field("{{ResponseStatus}}", sample: "Accepted"), Field("{{ResponseMessage}}", sample: "The invitation was accepted.")]),
            new(
                "system.genericNotification",
                "Generic Notification",
                "Generic notification email.",
                "System",
                "Message from {{SiteName}}",
                "<h1>{{Heading}}</h1><p>{{Message}}</p>",
                "{{Heading}}\n\n{{Message}}",
                [Field("{{Message}}", required: true)],
                [Field("{{Heading}}", sample: "Notification"), Field("{{Message}}", sample: "This is a notification."), Field("{{SiteName}}", sample: "adfast")])
        ];
    }

    private static MessageTemplateVariable Field(string key, string? label = null, string? description = null, bool required = false, bool isHtml = false, string? sample = null)
        => new(key, label ?? key.Trim('{', '}'), description, required, isHtml, sample ?? key.Trim('{', '}'));

    private static IReadOnlyList<MessageTemplateVariable> CommonIdentityFields()
        => [Field("{{RecipientName}}", sample: "Jane Smith"), Field("{{RecipientFirstName}}", sample: "Jane"), Field("{{RecipientLastName}}", sample: "Smith"), Field("{{RecipientEmail}}", sample: "jane@example.com"), Field("{{Username}}", sample: "jane@example.com"), Field("{{SiteUrl}}", sample: "https://adfast.example"), Field("{{SiteName}}", sample: "adfast")];

    private static IReadOnlyList<MessageTemplateVariable> CommonAuthFields()
        => [.. CommonIdentityFields(), Field("{{VerificationCode}}", sample: "123456"), Field("{{ExpiryMinutes}}", sample: "15")];
}