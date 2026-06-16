import type { MessageTemplateVariable } from "../types/templates";

export interface MergeFieldsPanelProps {
  variables: MessageTemplateVariable[];
  usedVariables?: string[];
}

export function MergeFieldsPanel({ variables, usedVariables = [] }: MergeFieldsPanelProps) {
  return (
    <div className="card">
      <div className="card-header fw-semibold">Merge fields</div>
      <div className="list-group list-group-flush">
        {variables.length === 0 && <div className="list-group-item text-muted">No merge fields configured.</div>}
        {variables.map(variable => {
          const used = usedVariables.includes(variable.key);
          return (
            <div className="list-group-item d-flex justify-content-between gap-3" key={variable.key}>
              <div>
                <code>{variable.key}</code>
                {variable.label && <div className="small text-muted">{variable.label}</div>}
              </div>
              <div className="text-end small">
                {variable.required && <span className="badge text-bg-warning me-1">Required</span>}
                {variable.isHtml && <span className="badge text-bg-info me-1">HTML</span>}
                {used && <span className="badge text-bg-success">Used</span>}
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
}
