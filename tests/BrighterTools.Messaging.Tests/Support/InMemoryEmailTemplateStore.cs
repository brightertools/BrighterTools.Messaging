using BrighterTools.Messaging.Abstractions;
using BrighterTools.Messaging.Models;

namespace BrighterTools.Messaging.Tests.Support;

internal sealed class InMemoryEmailTemplateStore : IEmailTemplateStore
{
    private readonly List<EmailTemplate> _templates = [];

    public void Add(EmailTemplate template)
    {
        if (template.Id == 0)
        {
            template.Id = _templates.Count == 0 ? 1 : _templates.Max(x => x.Id) + 1;
        }

        _templates.Add(template);
    }

    public Task<EmailTemplate?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_templates.SingleOrDefault(x => x.Id == id));
    }
}
