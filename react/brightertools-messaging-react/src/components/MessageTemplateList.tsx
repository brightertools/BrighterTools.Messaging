import type { MessageTemplateSummary } from "../types/templates";

export interface MessageTemplateListProps {
  templates: MessageTemplateSummary[];
  selectedKey?: string | null;
  onSelect?: (template: MessageTemplateSummary) => void;
}

export function MessageTemplateList({ templates, selectedKey, onSelect }: MessageTemplateListProps) {
  return (
    <div className="list-group">
      {templates.map(template => (
        <button
          className={`list-group-item list-group-item-action text-start ${selectedKey === template.key ? "active" : ""}`}
          key={template.key}
          type="button"
          onClick={() => onSelect?.(template)}
        >
          <div className="d-flex justify-content-between gap-3">
            <span className="fw-semibold">{template.name}</span>
            {template.isCustomized && <span className="badge text-bg-success">Customised</span>}
          </div>
          {template.description && <div className="small opacity-75">{template.description}</div>}
          <div className="small opacity-75">{template.key}</div>
        </button>
      ))}
    </div>
  );
}
