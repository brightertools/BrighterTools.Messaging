import type { MessageTemplatePreviewResult } from "../types/templates";

export interface MessageTemplatePreviewProps {
  preview: MessageTemplatePreviewResult | null;
}

export function MessageTemplatePreview({ preview }: MessageTemplatePreviewProps) {
  if (!preview) {
    return <div className="alert alert-secondary mb-0">Preview will appear here.</div>;
  }

  return (
    <div className="card">
      <div className="card-header">
        <div className="small text-muted">Subject</div>
        <div className="fw-semibold">{preview.subject}</div>
      </div>
      <iframe
        className="border-0 w-100"
        sandbox=""
        srcDoc={preview.html}
        style={{ minHeight: 420 }}
        title="Email preview"
      />
    </div>
  );
}
