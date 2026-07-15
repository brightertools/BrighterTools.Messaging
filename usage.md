# BrighterTools.Messaging Usage

Install the core package:

```powershell
dotnet add package BrighterTools.Messaging
```

Install the transports your app needs:

```powershell
dotnet add package BrighterTools.Messaging.MailKit
dotnet add package BrighterTools.Messaging.Postmark
dotnet add package BrighterTools.Messaging.SendGrid
dotnet add package BrighterTools.Messaging.Twilio
```

Install the React companion package:

```powershell
npm install @brightertools/messaging-react
```

`BrighterTools.Messaging` owns reusable email/SMS abstractions, template rendering, and notification orchestration. The host application owns template persistence, notification persistence, background dispatch scheduling, provider credentials, and business-specific message content.

Use one transport package per provider integration. Keep provider-specific credentials in the host app configuration and register only the transports enabled for that deployment.
