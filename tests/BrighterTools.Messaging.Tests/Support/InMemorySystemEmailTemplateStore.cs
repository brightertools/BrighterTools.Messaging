using BrighterTools.Messaging.Abstractions;
using BrighterTools.Messaging.Enums;
using BrighterTools.Messaging.Models;

namespace BrighterTools.Messaging.Tests.Support;

internal sealed class InMemorySystemEmailTemplateStore : ISystemEmailTemplateStore
{
    private readonly List<SystemEmailTemplate> _templates = [];

    public Task<SystemEmailTemplate?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_templates.SingleOrDefault(x => x.Id == id));
    }

    public Task<SystemEmailTemplate?> GetByEmailTypeAsync(EmailType emailType, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_templates.SingleOrDefault(x => x.EmailType == emailType));
    }

    public Task<IReadOnlyList<SystemEmailTemplate>> GetSystemTemplatesAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<SystemEmailTemplate>>(_templates.OrderBy(x => x.Id).ToList());
    }

    public Task UpsertAsync(IEnumerable<SystemEmailTemplate> templates, CancellationToken cancellationToken = default)
    {
        foreach (var template in templates)
        {
            var existingIndex = _templates.FindIndex(x => x.EmailType == template.EmailType);
            if (existingIndex >= 0)
            {
                template.Id = _templates[existingIndex].Id;
                _templates[existingIndex] = template;
                continue;
            }

            template.Id = _templates.Count == 0 ? 1 : _templates.Max(x => x.Id) + 1;
            _templates.Add(template);
        }

        return Task.CompletedTask;
    }
}
