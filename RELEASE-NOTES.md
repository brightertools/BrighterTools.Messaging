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
