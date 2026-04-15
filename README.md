# BrighterTools.Messaging

`BrighterTools.Messaging` is a transport-agnostic messaging library for .NET applications.

The host application owns:
- notification persistence
- template persistence
- attachment persistence
- background dispatch
- provider registration and credential storage
- business-specific email and SMS composition

The packages in this repo provide:
- reusable messaging abstractions
- template rendering
- queued notification orchestration
- explicit transport packages for SMTP, Postmark, and Twilio

## Packages

```powershell
dotnet add package BrighterTools.Messaging
dotnet add package BrighterTools.Messaging.Postmark
dotnet add package BrighterTools.Messaging.Twilio
```

Optional SMTP transport:

```powershell
dotnet add package BrighterTools.Messaging.MailKit
```

## Repository Layout

- `src/BrighterTools.Messaging`
  - transport-agnostic messaging core
- `src/BrighterTools.Messaging.MailKit`
  - SMTP transport registration via MailKit
- `src/BrighterTools.Messaging.Postmark`
  - Postmark email transport registration
- `src/BrighterTools.Messaging.Twilio`
  - Twilio SMS transport registration
- `tests/BrighterTools.Messaging.Tests`
  - package-level behavior and registration tests
- `docs`
  - public integration documentation

## Validation

```powershell
dotnet test .\BrighterTools.Messaging.sln
```

## Documentation

- [`docs/README.md`](docs/README.md)
- [`docs/integration-guide.md`](docs/integration-guide.md)
