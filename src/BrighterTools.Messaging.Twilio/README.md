# BrighterTools.Messaging.Twilio

Registers `ISmsSender` for `BrighterTools.Messaging` using Twilio.

## Package

```powershell
dotnet add package BrighterTools.Messaging.Twilio
```

## Registration

```csharp
services.AddBrighterToolsMessaging(configuration);
services.AddBrighterToolsTwilioSmsSender(configuration);
```

## Configuration

```json
{
  "Twilio": {
    "AccountId": "twilio-account-id",
    "AuthToken": "twilio-auth-token"
  }
}
```
