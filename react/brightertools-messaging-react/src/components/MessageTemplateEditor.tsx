import { useMemo, useState } from "react";
import type { MessagingApiClient } from "../services/createMessagingApi";
import type { MessageTemplateDetail, MessageTemplatePreviewResult, MessageTemplateSaveRequest } from "../types/templates";
import { MergeFieldsPanel } from "./MergeFieldsPanel";
import { MessageTemplatePreview } from "./MessageTemplatePreview";

export interface MessageTemplateEditorProps {
  api: MessagingApiClient;
  template: MessageTemplateDetail;
  tenantId?: string | null;
  onSaved?: (template: MessageTemplateDetail) => void;
}

export function MessageTemplateEditor({ api, template, tenantId, onSaved }: MessageTemplateEditorProps) {
  const [subject, setSubject] = useState(template.subject);
  const [htmlContent, setHtmlContent] = useState(template.htmlContent);
  const [textContent, setTextContent] = useState(template.textContent);
  const [designContent] = useState(template.designContent ?? null);
  const [preview, setPreview] = useState<MessageTemplatePreviewResult | null>(null);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const usedVariables = useMemo(
    () => template.availableVariables.filter(variable => htmlContent.includes(variable.key)).map(variable => variable.key),
    [htmlContent, template.availableVariables]
  );

  async function handlePreview() {
    setError(null);
    setPreview(await api.previewTemplate({
      key: template.key,
      culture: template.culture,
      tenantId,
      subject,
      htmlContent,
      textContent,
      designContent,
      mergeFields: buildSampleMergeFields(template)
    }));
  }

  async function handleSave() {
    setSaving(true);
    setError(null);
    try {
      const request: MessageTemplateSaveRequest = {
        key: template.key,
        culture: template.culture,
        tenantId,
        subject,
        htmlContent,
        textContent,
        designContent,
        sourceFormat: template.sourceFormat
      };
      onSaved?.(await api.saveTemplate(request));
    } catch (exception) {
      setError(exception instanceof Error ? exception.message : "Unable to save message template.");
    } finally {
      setSaving(false);
    }
  }

  return (
    <div className="row g-4">
      <div className="col-lg-7">
        {error && <div className="alert alert-danger">{error}</div>}
        <div className="card mb-3">
          <div className="card-body">
            <label className="form-label" htmlFor="message-template-subject">Subject</label>
            <input className="form-control" id="message-template-subject" value={subject} onChange={event => setSubject(event.target.value)} />
          </div>
        </div>
        <div className="card mb-3">
          <div className="card-header fw-semibold">HTML content</div>
          <div className="card-body">
            <textarea className="form-control font-monospace" rows={16} value={htmlContent} onChange={event => setHtmlContent(event.target.value)} />
          </div>
        </div>
        <div className="card mb-3">
          <div className="card-header fw-semibold">Text content</div>
          <div className="card-body">
            <textarea className="form-control font-monospace" rows={7} value={textContent} onChange={event => setTextContent(event.target.value)} />
          </div>
        </div>
        <div className="d-flex gap-2">
          <button className="btn btn-primary" disabled={saving || !template.isEditable} type="button" onClick={handleSave}>{saving ? "Saving..." : "Save"}</button>
          <button className="btn btn-outline-secondary" type="button" onClick={handlePreview}>Preview</button>
        </div>
      </div>
      <div className="col-lg-5">
        <MergeFieldsPanel variables={template.availableVariables} usedVariables={usedVariables} />
        <div className="mt-3">
          <MessageTemplatePreview preview={preview} />
        </div>
      </div>
    </div>
  );
}

function buildSampleMergeFields(template: MessageTemplateDetail): Record<string, string> {
  return Object.fromEntries(template.availableVariables.map(variable => [variable.key, variable.sampleValue ?? variable.label ?? variable.key]));
}
