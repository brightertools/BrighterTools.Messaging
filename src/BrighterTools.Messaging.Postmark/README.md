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
    "TrackOpens": true,
    "EnableAutomaticRetries": true
  }
}
```

Hosts with a durable delivery queue should set `Postmark:EnableAutomaticRetries=false`.
This makes one transport attempt and leaves retry decisions to the host. The default
remains four exponential-backoff retries for compatibility. A timeout or cancellation
after dispatch does not prove that the email was not sent; reconcile uncertain sends
before resending. This option does not provide exactly-once delivery.
