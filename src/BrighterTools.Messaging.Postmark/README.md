# BrighterTools.Messaging.Postmark

Registers `IEmailSender` for `BrighterTools.Messaging` using Postmark.

## Package

```powershell
dotnet add package BrighterTools.Messaging.Postmark
```

## Registration

```csharp
services.AddBrighterToolsMessaging(configuration);
services.AddBrighterToolsPostmarkEmailSender(configuration);
```

## Configuration

```json
{
  "Postmark": {
    "ServerToken": "pm-token",
    "TrackOpens": true
  }
}
```
