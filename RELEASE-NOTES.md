# Postmark 1.2.0 publication — 12 November 2026

Published from `8dd302dc4d3b42c3274b6a47625d22c1613127ba` on
`codex/postmark-package-release-20261112`, using:
https://github.com/brightertools/BrighterTools.Messaging/actions/runs/43257138217

Packages the existing native retry opt-out and rejection handling, with unchanged
default retries for other consumers. Depends on Messaging core 1.1.0. Nine focused
captured-transport tests and the target dependency audit passed. The public archive
and NuGet signature were verified; SHA-256:
`64e7d92f4ecc8dcd9fd1090949c0a9e68e5504f36c83c1279f8c0bce15e632a0`.
Unrelated MailKit project audit failures were not suppressed or changed. This branch's
release workflow targets Postmark only; reconcile the main family workflow before
merging that workflow change. Core 1.1.0 was not republished.

# Release 1.1.0 — 2026-09-17

This release updates the existing library; SQL Server defaults and host-owned
persistence registration remain unchanged. The proposed production SQLite
expansion is cancelled. Existing SQLite samples/tests do not establish a new
production-support commitment.

## Changes

- Update compatible stable dependencies and .NET 10 servicing packages.
- Preserve existing BrighterTools integrations except where explicitly noted below.
- Validate Release builds and packages before publication.

The React companion release is `1.0.2`. React 18 remains supported.

## Dependency alignment

The NuGet family is 1.1.0; npm is 1.0.2. Public BrighterTools APIs are unchanged.
Applications pinning provider SDKs must align their references, including Twilio
8.0.1 and Polly 8.8.0. No real email/SMS is sent by release validation.

## Publication

Use `.github/workflows/publish-tool.yml` on the release commit. The workflow
validates before publishing and uses the production environment with registry
trusted publishing. Registry policies must authorize this repository and workflow;
a locally built package is not proof of successful publication.

## BrighterTools.Messaging.Postmark dedicated release

Only this provider is packed by the release branch workflow. Existing published
core and other provider packages are not republished. Host behaviour is retained.
