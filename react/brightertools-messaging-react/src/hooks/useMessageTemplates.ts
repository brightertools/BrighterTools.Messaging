import { useCallback, useEffect, useState } from "react";
import type { MessagingApiClient } from "../services/createMessagingApi";
import type { MessageTemplateQuery, MessageTemplateSummary } from "../types/templates";

export function useMessageTemplates(api: MessagingApiClient, query?: MessageTemplateQuery) {
  const [templates, setTemplates] = useState<MessageTemplateSummary[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<Error | null>(null);

  const reload = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      setTemplates(await api.listTemplates(query));
    } catch (exception) {
      setError(exception instanceof Error ? exception : new Error("Unable to load message templates."));
    } finally {
      setLoading(false);
    }
  }, [api, query?.category, query?.culture, query?.includeFeature, query?.includeSystem, query?.query, query?.tenantId]);

  useEffect(() => {
    void reload();
  }, [reload]);

  return { templates, loading, error, reload };
}
