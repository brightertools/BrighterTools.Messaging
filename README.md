# BrighterTools.Messaging

`BrighterTools.Messaging` is a transport-agnostic messaging library for .NET applications with optional provider transports and a React companion package for template/message UI.

The host application owns notification persistence, template persistence, attachment persistence, background dispatch, provider credentials, and business-specific email/SMS composition.

## Packages

```powershell
dotnet add package BrighterTools.Messaging
dotnet add package BrighterTools.Messaging.MailKit
dotnet add package BrighterTools.Messaging.Postmark
dotnet add package BrighterTools.Messaging.SendGrid
dotnet add package BrighterTools.Messaging.Twilio
npm install brightertools-messaging-react
```

## Repository Layout

- `src/BrighterTools.Messaging` - transport-agnostic messaging core
- `src/BrighterTools.Messaging.MailKit` - SMTP transport registration via MailKit
- `src/BrighterTools.Messaging.Postmark` - Postmark email transport registration
- `src/BrighterTools.Messaging.SendGrid` - SendGrid email transport registration
- `src/BrighterTools.Messaging.Twilio` - Twilio SMS transport registration
- `react/brightertools-messaging-react` - React companion package
- `tests/BrighterTools.Messaging.Tests` - package behavior and registration tests

## Documentation

- [usage.md](./usage.md) for consuming application guidance
- [publishing.md](./publishing.md) for maintainer release steps
- [RELEASE_NOTES.md](./RELEASE_NOTES.md) for release history
- [docs/README.md](./docs/README.md) for additional notes

## Validation

```powershell
dotnet test .\BrighterTools.Messaging.sln
cd .\react\brightertools-messaging-react
npm install
npm test
npm run build
npm run pack:dry-run
```
