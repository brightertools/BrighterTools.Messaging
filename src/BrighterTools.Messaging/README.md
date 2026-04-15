# BrighterTools.Messaging

`BrighterTools.Messaging` is the transport-agnostic core for queued email and SMS delivery.

This package provides:
- messaging abstractions for stores and dispatch
- template rendering
- notification queue orchestration
- email and SMS test-mode handling

This package does not register a transport implementation by itself.
Register one or more companion transport packages explicitly:
- `BrighterTools.Messaging.MailKit`
- `BrighterTools.Messaging.Postmark`
- `BrighterTools.Messaging.Twilio`

## Package

```powershell
dotnet add package BrighterTools.Messaging
```

## Minimal Registration

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
