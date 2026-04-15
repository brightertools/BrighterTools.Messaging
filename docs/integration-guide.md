# BrighterTools.Messaging Integration

## Overview

`BrighterTools.Messaging` is the transport-agnostic messaging core.

It owns:
- notification orchestration
- template rendering
- queued notification execution
- test-mode recipient substitution
- attachment resolution through abstractions

It does not own:
- EF persistence
- Hangfire or scheduler selection
- host-specific queue tables
- transport provider credentials
- transport provider registration
- business-specific email composition

Transport implementations are registered explicitly through companion packages:
- `BrighterTools.Messaging.MailKit`
- `BrighterTools.Messaging.Postmark`
- `BrighterTools.Messaging.Twilio`

## Core Registration

Register the core package first, then register one or more transport packages, then register the host adapters.

```csharp
services.AddBrighterToolsMessaging(configuration);
services.AddBrighterToolsPostmarkEmailSender(configuration);
services.AddBrighterToolsTwilioSmsSender(configuration);

services.AddScoped<INotificationMessageStore, MyNotificationMessageStore>();
services.AddScoped<ISystemEmailTemplateStore, MySystemEmailTemplateStore>();
services.AddScoped<IEmailTemplateStore, MyEmailTemplateStore>();
services.AddScoped<INotificationDispatcher, MyNotificationDispatcher>();
services.AddScoped<INotificationAttachmentStore, MyNotificationAttachmentStore>();
```

If a host does not need one of the optional integrations:
- omit `INotificationDispatcher` to queue without auto-dispatch
- omit `INotificationAttachmentStore` if attachments are stored inline in the host store model

## Required Host Abstractions

### `INotificationMessageStore`

The host store is responsible for:
- creating queued notification records
- retrieving a notification by id for execution
- updating send and failure state

### `ISystemEmailTemplateStore`

Used by the library to load system email templates by `EmailType` and by id.

### `IEmailTemplateStore`

Used by the library to load host-managed email templates by id.

### `INotificationDispatcher`

Optional queue-dispatch abstraction.

Typical implementations:
- Hangfire enqueue
- Quartz enqueue
- no-op implementation when a poller processes the queue

### `INotificationAttachmentStore`

Optional attachment persistence abstraction.

Use this when attachments should be stored outside the queued notification record itself.

## Transport Packages

### Postmark

```csharp
services.AddBrighterToolsPostmarkEmailSender(configuration);
```

```json
{
  "Postmark": {
    "ServerToken": "pm-token",
    "TrackOpens": true
  }
}
```

### SMTP via MailKit

```csharp
services.AddBrighterToolsMailKitEmailSender(configuration);
```

```json
{
  "Smtp": {
    "Host": "smtp.example.com",
    "Port": 587,
    "Username": "smtp-user",
    "Password": "smtp-password",
    "SecureSocketOption": "StartTls"
  }
}
```

### Twilio

```csharp
services.AddBrighterToolsTwilioSmsSender(configuration);
```

```json
{
  "Twilio": {
    "AccountId": "twilio-account-id",
    "AuthToken": "twilio-auth-token"
  }
}
```

## Test Mode

The core library supports test mode for both email and SMS.

Email settings:
- `Email:TestMode`
- `Email:TestModeAddresses`

SMS settings:
- `SmsMessage:TestMode`
- `SmsMessage:TestModeNumberList`

When email test mode is enabled, recipients are replaced with the configured test recipients before queue persistence.
When SMS test mode is enabled, recipient numbers are replaced with the configured test numbers before queue persistence.

## Attachment Strategy

Recommended production approach:
1. Store queued message metadata in the host database.
2. Store attachment content in file or blob storage.
3. Store attachment references in a host table.
4. Resolve attachment bytes only when the queued message is executed.

## Business Email Composition

Keep business-specific email composition outside the library.
Examples:
- user invitations
- password reset emails
- account verification emails
- onboarding emails

Those should live in an app-level orchestration service that depends on `INotificationMessageService`.

## Minimal Integration Checklist

1. Add the core package and the required transport package.
2. Call `AddBrighterToolsMessaging(configuration)`.
3. Call the required transport registration methods.
4. Implement and register `INotificationMessageStore`.
5. Implement and register the template stores.
6. Implement and register `INotificationDispatcher` if the app uses a scheduler.
7. Implement and register `INotificationAttachmentStore` if attachments are stored externally.
8. Configure transport settings and optional test-mode settings.
9. Create an app-level service for business email and SMS composition.
