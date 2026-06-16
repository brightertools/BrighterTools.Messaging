import { useCallback, useEffect, useState } from "react";
import type { MessagingApiClient } from "../services/createMessagingApi";
import type { MessageTemplateDetail, MessageTemplateQuery, MessageTemplateSaveRequest } from "../types/templates";

export function useMessageTemplate(api: MessagingApiClient, key: string | null, query?: Pick<MessageTemplateQuery, "tenantId" | "culture">) {
  const [template, setTemplate] = useState<MessageTemplateDetail | null>(null);
  const [loading, setLoading] = useState(Boolean(key));
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<Error | null>(null);

  const reload = useCallback(async () => {
    if (!key) return;
    setLoading(true);
    setError(null);
    try {
      setTemplate(await api.getTemplate(key, query));
    } catch (exception) {
      setError(exception instanceof Error ? exception : new Error("Unable to load message template."));
    } finally {
      setLoading(false);
    }
  }, [api, key, query?.culture, query?.tenantId]);

  const save = useCallback(async (request: MessageTemplateSaveRequest) => {
    setSaving(true);
    setError(null);
    try {
      const saved = await api.saveTemplate(request);
      setTemplate(saved);
      return saved;
    } catch (exception) {
      const saveError = exception instanceof Error ? exception : new Error("Unable to save message template.");
      setError(saveError);
      throw saveError;
    } finally {
      setSaving(false);
    }
  }, [api]);

  const revert = useCallback(async () => {
    if (!key) return null;
    setSaving(true);
    setError(null);
    try {
      const reverted = await api.revertTemplate(key, query);
      setTemplate(reverted);
      return reverted;
    } catch (exception) {
      const revertError = exception instanceof Error ? exception : new Error("Unable to revert message template.");
      setError(revertError);
      throw revertError;
    } finally {
      setSaving(false);
    }
  }, [api, key, query?.culture, query?.tenantId]);

  useEffect(() => {
    void reload();
  }, [reload]);

  return { template, loading, saving, error, reload, save, revert };
}
