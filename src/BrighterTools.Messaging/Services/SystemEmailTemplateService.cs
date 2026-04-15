using BrighterTools.Messaging.Abstractions;
using BrighterTools.Messaging.Enums;
using BrighterTools.Messaging.Models;
using BrighterTools.Messaging.Seeds;
namespace BrighterTools.Messaging.Services;
/// <summary>
/// Provides System Email Template operations.
/// </summary>
public class SystemEmailTemplateService(ISystemEmailTemplateStore templateStore) : ISystemEmailTemplateService
{
    /// <summary>
    /// Gets BY ID.
    /// </summary>
    public Task<SystemEmailTemplate?> GetByIdAsync(int id, CancellationToken cancellationToken = default) => templateStore.GetByIdAsync(id, cancellationToken);
    /// <summary>
    /// Gets BY Email Type.
    /// </summary>
    public Task<SystemEmailTemplate?> GetByEmailTypeAsync(EmailType emailType, CancellationToken cancellationToken = default) => templateStore.GetByEmailTypeAsync(emailType, cancellationToken);
    /// <summary>
    /// Gets System Templates.
    /// </summary>
    public Task<IReadOnlyList<SystemEmailTemplate>> GetSystemTemplatesAsync(CancellationToken cancellationToken = default) => templateStore.GetSystemTemplatesAsync(cancellationToken);
    /// <summary>
    /// Executes Upsert Seed Data.
    /// </summary>
    public async Task UpsertSeedDataAsync(IEnumerable<SystemEmailTemplateSeedDefinition> templates, CancellationToken cancellationToken = default)
    {
        var seedList = templates.ToList();
        var entities = seedList.Select(seed => new SystemEmailTemplate
        {
            EmailType = seed.EmailType,
            Name = seed.Name,
            Subject = seed.Subject,
            Description = seed.Description,
            SingleTemplate = seed.SingleTemplate,
            RequiredFields = seed.RequiredFields,
            AvailableFields = seed.AvailableFields,
            ReadOnly = seed.ReadOnly,
            SendAsUser = seed.SendAsUser,
            IsSystemTemplate = seed.IsSystemTemplate,
            DesignContent = seed.DesignContent,
            HtmlContent = seed.HtmlContent,
            TextContent = seed.TextContent
        }).ToList();
        foreach (var entity in entities)
        {
            var existing = await templateStore.GetByEmailTypeAsync(entity.EmailType, cancellationToken);
            if (existing == null) { continue; }
            entity.Id = existing.Id;
            entity.Guid = existing.Guid;
            entity.CreatedDate = existing.CreatedDate;
        }
        await templateStore.UpsertAsync(entities, cancellationToken);
        foreach (var seed in seedList.Where(x => x.BaseTemplateEmailType.HasValue))
        {
            var template = await templateStore.GetByEmailTypeAsync(seed.EmailType, cancellationToken);
            var baseTemplate = await templateStore.GetByEmailTypeAsync(seed.BaseTemplateEmailType!.Value, cancellationToken);
            if (template == null || baseTemplate == null || template.BaseTemplateId == baseTemplate.Id) { continue; }
            template.BaseTemplateId = baseTemplate.Id;
            await templateStore.UpsertAsync([template], cancellationToken);
        }
    }
}

