# BrighterTools.Messaging.MailKit

Registers `IEmailSender` for `BrighterTools.Messaging` using MailKit and SMTP.

## Package

```powershell
dotnet add package BrighterTools.Messaging.MailKit
```

## Registration

```csharp
services.AddBrighterToolsMessaging(configuration);
services.AddBrighterToolsMailKitEmailSender(configuration);
```

## Configuration

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
