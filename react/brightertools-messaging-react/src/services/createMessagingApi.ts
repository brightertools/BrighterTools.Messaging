import type {
  MessageTemplateDetail,
  MessageTemplatePreviewRequest,
  MessageTemplatePreviewResult,
  MessageTemplateQuery,
  MessageTemplateSaveRequest,
  MessageTemplateSummary
} from "../types/templates";

export interface MessagingApiClient {
  listTemplates(query?: MessageTemplateQuery): Promise<MessageTemplateSummary[]>;
  getTemplate(key: string, query?: Pick<MessageTemplateQuery, "tenantId" | "culture">): Promise<MessageTemplateDetail>;
  saveTemplate(request: MessageTemplateSaveRequest): Promise<MessageTemplateDetail>;
  previewTemplate(request: MessageTemplatePreviewRequest): Promise<MessageTemplatePreviewResult>;
  revertTemplate(key: string, query?: Pick<MessageTemplateQuery, "tenantId" | "culture">): Promise<MessageTemplateDetail>;
  uploadAsset?(file: File, context?: { key?: string; tenantId?: string | null }): Promise<{ url: string }>;
}

export interface CreateMessagingApiOptions {
  baseUrl: string;
  getHeaders?: () => HeadersInit | Promise<HeadersInit>;
  fetchImpl?: typeof fetch;
}

export function createMessagingApi(options: CreateMessagingApiOptions): MessagingApiClient {
  const fetcher = options.fetchImpl ?? fetch;
  const baseUrl = options.baseUrl.replace(/\/$/, "");

  async function request<T>(path: string, init?: RequestInit): Promise<T> {
    const headers = new Headers(init?.headers);
    headers.set("Accept", "application/json");

    if (init?.body && !headers.has("Content-Type") && !(init.body instanceof FormData)) {
      headers.set("Content-Type", "application/json");
    }

    const configuredHeaders = await options.getHeaders?.();
    if (configuredHeaders) {
      new Headers(configuredHeaders).forEach((value, key) => headers.set(key, value));
    }

    const response = await fetcher(`${baseUrl}${path}`, { ...init, headers });
    if (!response.ok) {
      throw new Error(`Messaging API request failed with ${response.status}`);
    }

    return response.json() as Promise<T>;
  }

  return {
    listTemplates: query => request<MessageTemplateSummary[]>(`/message-templates${toQueryString(query)}`),
    getTemplate: (key, query) => request<MessageTemplateDetail>(`/message-templates/${encodeURIComponent(key)}${toQueryString(query)}`),
    saveTemplate: requestBody => request<MessageTemplateDetail>(`/message-templates/${encodeURIComponent(requestBody.key)}/save`, {
      method: "POST",
      body: JSON.stringify(requestBody)
    }),
    previewTemplate: requestBody => request<MessageTemplatePreviewResult>(`/message-templates/${encodeURIComponent(requestBody.key)}/preview`, {
      method: "POST",
      body: JSON.stringify(requestBody)
    }),
    revertTemplate: (key, query) => request<MessageTemplateDetail>(`/message-templates/${encodeURIComponent(key)}/revert${toQueryString(query)}`, {
      method: "POST"
    }),
    uploadAsset: async (file, context) => {
      const form = new FormData();
      form.append("file", file);
      if (context?.key) form.append("key", context.key);
      if (context?.tenantId) form.append("tenantId", context.tenantId);
      return request<{ url: string }>("/message-templates/editor-upload", { method: "POST", body: form });
    }
  };
}

function toQueryString(query?: object): string {
  if (!query) return "";
  const parameters = new URLSearchParams();
  Object.entries(query as Record<string, unknown>).forEach(([key, value]) => {
    if (value !== undefined && value !== null && value !== "") {
      parameters.set(key, String(value));
    }
  });
  const value = parameters.toString();
  return value ? `?${value}` : "";
}
