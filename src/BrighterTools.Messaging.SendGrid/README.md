# BrighterTools.Messaging.SendGrid

Registers `IEmailSender` for `BrighterTools.Messaging` using SendGrid.

## Configuration

```json
{
  "Email": {
    "FromName": "System",
    "FromAddress": "noreply@example.com",
    "SubjectPrefix": "",
    "TestMode": true,
    "TestModeAddresses": ["test@example.com"]
  },
  "SendGrid": {
    "ApiKey": "SENDGRID_API_KEY"
  }
}
```

## Registration

```csharp
services.AddBrighterToolsMessaging(configuration);
services.AddBrighterToolsSendGridEmailSender(configuration);
```