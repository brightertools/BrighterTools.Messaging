# BrighterTools.Messaging Publishing

This guide is for maintainers packaging and publishing the BrighterTools.Messaging NuGet packages and `@brightertools/messaging-react`.

## Package Pages

- NuGet: https://www.nuget.org/packages/BrighterTools.Messaging
- NuGet: https://www.nuget.org/packages/BrighterTools.Messaging.MailKit
- NuGet: https://www.nuget.org/packages/BrighterTools.Messaging.Postmark
- NuGet: https://www.nuget.org/packages/BrighterTools.Messaging.SendGrid
- NuGet: https://www.nuget.org/packages/BrighterTools.Messaging.Twilio
- npm: https://www.npmjs.com/package/@brightertools/messaging-react

## Local Packaging

```text
PackageToolForNuGet.bat
```

React package validation:

```text
cd ./react/brightertools-messaging-react
npm install
npm test
npm run build
npm run pack:dry-run
```

Equivalent NuGet flow:

```text
dotnet restore ./BrighterTools.Messaging.sln --configfile ./NuGet.config
dotnet build ./BrighterTools.Messaging.sln -c Release --no-restore
dotnet test ./BrighterTools.Messaging.sln -c Release --no-build
```

The packaging script and publish workflow pack core, MailKit, Postmark, SendGrid, and Twilio packages into `artifacts/nuget`.

## GitHub Actions Publishing

Publishing is handled by `.github/workflows/publish-tool.yml`.

Workflow inputs:

- `version` optionally overrides NuGet package versions.
- `publish_to_nuget` controls whether `.nupkg` files are pushed to nuget.org.
- `publish_to_npm` controls whether the React package is published to npm.

NuGet uses `NuGet/login@v1` and GitHub OIDC. npm uses trusted publishing from GitHub Actions. No long-lived NuGet or npm publish token is required after registry policies are configured.

## Registry Checklist

- NuGet package owner has Trusted Publishing policies for this repository, `publish-tool.yml`, and the `production` environment.
- npm package has a Trusted Publisher entry for this repository, `publish-tool.yml`, and the `production` environment.
- Package metadata uses the `MIT-0` license.
- Version is `1.0.1` for this patch publish.

## Related Docs

- [README.md](./README.md) for overview and package layout
- [usage.md](./usage.md) for consuming application guidance
