# Message Template Administration Architecture

## Goal

`BrighterTools.Messaging` should own the reusable orchestration for system and feature email templates, while host applications provide only thin persistence, authorization, routing, and storage adapters.

This keeps app integrations small and gives all BrighterTools apps the same production-ready notification/template workflow.

## Package Boundaries

### Core package: `BrighterTools.Messaging`

Owns:

- message template contracts and models
- system template seed catalog
- template versioning rules
- tenant/global/default template resolution
- culture fallback resolution
- base/nested template rendering
- merge-field validation and rendering
- notification queue orchestration
- preview orchestration
- template save/revert/publish orchestration
- abstractions for persistence, authorization context, and editor asset storage

Does not own:

- EF Core implementation details
- host application controllers
- host user/tenant models
- transport credentials
- storage provider SDKs
- React UI

### Transport packages

Existing transport packages remain focused on delivery only:

- `BrighterTools.Messaging.Postmark`
- `BrighterTools.Messaging.MailKit`
- `BrighterTools.Messaging.SendGrid`
- `BrighterTools.Messaging.Twilio`

### Storage packages

Optional packages should implement editor asset upload storage:

- `BrighterTools.Messaging.Storage.Mosaio`
- `BrighterTools.Messaging.Storage.FileStorage`
- `BrighterTools.Messaging.Storage.AzureBlob`

Disk storage can be added later if local-only development needs it.

### React package

`react/brightertools-messaging-react` owns reusable Bootstrap-friendly React UI:

- template list
- template editor shell
- HTML/source editor mode
- `react-email-editor` visual editor wrapper
- preview pane
- merge-field panel
- revert confirmation modal
- hooks and API client factory

The React package should not depend on any specific app, Redux store, auth context, or router.

## Template Concepts

### Definition

A definition describes a template the system knows about. Definitions are seeded by the library or by a host app.

Recommended fields:

- `Key` stable string key, for example `auth.password.reset`
- `Name`
- `Description`
- `Category`
- `Channel`, initially `Email`
- `IsSystem`
- `IsEditable`
- `IsTenantOverrideAllowed`
- `DefaultCulture`, default `en-GB`
- `RequiredVariablesJson`
- `AvailableVariablesJson`
- `DefaultVersion`

`EmailType` can remain for backward compatibility, but new orchestration should use string keys.

### Template Content

Template content stores renderable content for a definition.

Recommended fields:

- `DefinitionKey`
- `Culture`
- `Scope`, one of `LibraryDefault`, `Host`, `Tenant`
- `TenantId`, nullable
- `Subject`
- `HtmlContent`
- `TextContent`
- `DesignContent`, nullable
- `SourceFormat`, one of `Html`, `ReactEmailEditor`
- `BaseTemplateKey`, nullable
- `Version`
- `BasedOnDefaultVersion`, nullable
- `IsCustomized`
- `IsPublished`
- audit fields

### Revisions

Every save should create a revision.

Recommended fields:

- `TemplateId`
- `Version`
- `Subject`
- `HtmlContent`
- `TextContent`
- `DesignContent`
- `SourceFormat`
- `SavedByUserId`
- `SavedAtUtc`
- `ChangeReason`

This supports revert, audit, and safe upgrades.

## Base and Nested Templates

Nested template support must remain. The preferred reserved token is:

```text
{{ContentTemplate}}
```

A base layout can wrap a message body:

```html
<html>
  <body>
    <main>{{ContentTemplate}}</main>
  </body>
</html>
```

Rendering order:

1. Resolve message template.
2. Resolve base template if configured.
3. Inject message HTML into base template using `{{ContentTemplate}}`.
4. Apply merge fields to subject, HTML, and text.
5. Apply subject prefix if configured.

HTML-safe fields should not be inferred from names like `MessageHtml`. Instead, they should be declared in variable metadata:

```json
{
  "key": "{{OfferSummaryHtml}}",
  "required": false,
  "isHtml": true
}
```

For backward compatibility, the current suffix-based behavior can remain temporarily, but new templates should use explicit metadata.

## Template Resolution

Resolver order should be deterministic and safe.

For tenant-aware apps:

1. Tenant template for requested culture.
2. Tenant template for default culture.
3. Host template for requested culture.
4. Host template for default culture.
5. Library default for requested culture.
6. Library default for default culture.

For single-tenant apps, tenant steps are skipped.

System/auth templates should initially be host-admin editable only. Tenant overrides should be disabled by default for security-sensitive auth flows.

## Seed and Migration Rules

Default templates should be seeded through library seed definitions, not hand-coded app HTML migrations.

Seed behavior must be safe:

- seed by stable `Key`
- seed is idempotent
- new library defaults create/update library-default rows
- customised host/tenant templates are never overwritten
- `BasedOnDefaultVersion` tracks drift from the library default
- admin UI can show when a newer default exists
- revert resets the host/tenant override to the current resolved default

This avoids the current risk where upsert-by-enum can overwrite admin-edited content.

## Standard System Email Keys

Initial library-owned defaults should include:

```text
auth.signup.emailVerification
auth.account.emailVerification
auth.loginEmail.changeVerification
auth.notificationEmail.changeVerification
auth.password.reset
auth.password.updated
auth.passwordless.login
auth.provider.linked
auth.provider.removed
system.genericNotification
tenant.userInvitation
tenant.userInvitationResponse
```

Templates should be basic, non-branded, accessible HTML. Branding should primarily happen through base layouts.

## Merge Fields

A merge field definition should support:

- `Key`, for example `{{RecipientName}}`
- `Label`
- `Description`
- `Required`
- `IsHtml`
- `SampleValue`

Renderer rules:

- required variables must be present before send/publish
- unknown variables should be reported during preview/save
- normal variables are HTML encoded in HTML output
- `IsHtml` variables are passed through only after explicit declaration
- subject should never accept raw HTML

## Storage Abstraction

Editor image/file upload should use a storage abstraction.

```csharp
public interface IMessageTemplateAssetStore
{
    Task<MessageTemplateAssetUploadResult> UploadAsync(
        MessageTemplateAssetUploadRequest request,
        CancellationToken cancellationToken = default);
}
```

The core library owns the abstraction and request/response models. Provider packages own implementations.

Recommended mechanisms:

- `Mosaio`
- `BrighterToolsFileStorage`
- `AzureBlobStorage`

The React editor only calls the concise upload endpoint. The host app decides which storage adapter is registered.

## Backend Services To Add

Recommended library services:

- `IMessageTemplateDefinitionService`
- `IMessageTemplateSeedService`
- `IMessageTemplateResolver`
- `IMessageTemplateRenderer`
- `IMessageTemplateAdminService`
- `IMessageTemplatePreviewService`
- `IMessageTemplateAssetService`
- `ISystemMessageSender`

Recommended store abstractions:

- `IMessageTemplateDefinitionStore`
- `IMessageTemplateContentStore`
- `IMessageTemplateRevisionStore`
- `IMessageTemplateAssetStore`

The host app implements the stores against its database/storage. The library owns the workflows.

## Concise Host API

Host apps should expose thin admin endpoints that call `IMessageTemplateAdminService`.

Recommended route prefix:

```text
/api/v1/admin/message-templates
```

Recommended endpoints:

```text
GET    /api/v1/admin/message-templates
GET    /api/v1/admin/message-templates/{key}
POST   /api/v1/admin/message-templates/{key}/save
POST   /api/v1/admin/message-templates/{key}/preview
POST   /api/v1/admin/message-templates/{key}/revert
POST   /api/v1/admin/message-templates/{key}/publish
POST   /api/v1/admin/message-templates/editor-upload
```

Tenant-aware hosts can support optional query/body fields:

```text
tenantId
culture
```

The host controller should only handle:

- authorization policy
- current user id
- current tenant id, if applicable
- model binding
- calling the library service
- returning API results

## React Integration Contract

`brightertools-messaging-react` should consume a small API client interface:

```ts
interface MessagingApiClient {
  listTemplates(query?: MessageTemplateQuery): Promise<MessageTemplateSummary[]>;
  getTemplate(key: string, query?: TemplateScopeQuery): Promise<MessageTemplateDetail>;
  saveTemplate(request: MessageTemplateSaveRequest): Promise<MessageTemplateDetail>;
  previewTemplate(request: MessageTemplatePreviewRequest): Promise<MessageTemplatePreviewResult>;
  revertTemplate(key: string, query?: TemplateScopeQuery): Promise<MessageTemplateDetail>;
  uploadAsset?(file: File, context?: TemplateAssetUploadContext): Promise<{ url: string }>;
}
```

Apps can use the supplied `createMessagingApi` or provide their own client.

## Editor Modes

The editor should support two modes:

### HTML mode

Used when `SourceFormat = Html` or `DesignContent` is empty.

Capabilities:

- edit subject
- edit HTML
- edit text
- preview
- validate merge fields
- save
- revert

### Visual mode

Used when `SourceFormat = ReactEmailEditor` and `DesignContent` exists, or when admin chooses to replace HTML with a visual design.

Capabilities:

- load Unlayer design JSON
- set merge tags
- upload assets through `uploadAsset`
- export design JSON and HTML
- preview
- validate merge fields
- save

Plain HTML should not be force-converted into visual design JSON.

## Security Notes

- System/auth template editing is host-admin only initially.
- Tenant overrides for auth/security templates are disabled by default.
- Template edits should be audited.
- Preview iframe should be sandboxed.
- Stored editor/upload assets should validate file type and size.
- Subject rendering should not allow raw HTML.
- HTML pass-through merge fields must be explicit.
- Sending services should use rendered/snapshotted content, not mutable template references.

## Implementation Phases

### Phase 1: Library contracts and seeds

- add string-keyed template contracts
- add variable metadata contracts
- add seed definitions for auth/system messages
- add non-overwriting seed service behavior
- add resolver and renderer tests

### Phase 2: Admin orchestration

- add admin service for list/get/save/preview/revert/publish
- add revision support
- add asset upload abstraction
- add tests for permissions assumptions, fallback order, and revert

### Phase 3: React package

- complete HTML editor mode
- add visual editor wrapper around `react-email-editor`
- add merge-field validation UI
- add preview and revert modal

### Phase 4: Skilledly integration

- implement EF store adapters
- add migrations
- add thin admin controller
- register messaging services
- replace hard-coded auth emails with template sends
- mount admin UI

### Phase 5: MyRipple and tenant app parity

- implement tenant-aware stores
- enable tenant overrides for safe non-auth templates
- keep auth templates host-admin only unless explicitly enabled
