export type MessageTemplateSourceFormat = "html" | "react-email-editor";
export type MessageTemplateScope = "library-default" | "host" | "tenant";

export interface MessageTemplateVariable {
  key: string;
  label?: string;
  required?: boolean;
  isHtml?: boolean;
  sampleValue?: string;
  description?: string;
}

export interface MessageTemplateSummary {
  key: string;
  name: string;
  description?: string;
  category?: string;
  culture?: string;
  scope: MessageTemplateScope;
  isSystem: boolean;
  isEditable: boolean;
  isCustomized: boolean;
  isTenantOverrideAllowed: boolean;
  sourceFormat: MessageTemplateSourceFormat;
  updatedAtUtc?: string;
}

export interface MessageTemplateDetail extends MessageTemplateSummary {
  subject: string;
  htmlContent: string;
  textContent: string;
  designContent?: string | null;
  requiredVariables: MessageTemplateVariable[];
  availableVariables: MessageTemplateVariable[];
  basedOnDefaultVersion?: number | null;
  currentDefaultVersion?: number | null;
}

export interface MessageTemplateSaveRequest {
  key: string;
  culture?: string;
  tenantId?: string | null;
  subject: string;
  htmlContent: string;
  textContent: string;
  designContent?: string | null;
  sourceFormat: MessageTemplateSourceFormat;
}

export interface MessageTemplatePreviewRequest {
  key: string;
  culture?: string;
  tenantId?: string | null;
  subject?: string;
  htmlContent?: string;
  textContent?: string;
  designContent?: string | null;
  mergeFields: Record<string, string>;
}

export interface MessageTemplatePreviewResult {
  subject: string;
  html: string;
  text: string;
}

export interface MessageTemplateQuery {
  tenantId?: string | null;
  culture?: string;
  category?: string;
  includeSystem?: boolean;
  includeFeature?: boolean;
  query?: string;
}
