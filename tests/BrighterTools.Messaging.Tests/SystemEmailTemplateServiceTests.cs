using BrighterTools.Messaging.Enums;
using BrighterTools.Messaging.Seeds;
using BrighterTools.Messaging.Services;
using BrighterTools.Messaging.Tests.Support;
using FluentAssertions;

namespace BrighterTools.Messaging.Tests;

public class SystemEmailTemplateServiceTests
{
    [Fact]
    public async Task UpsertSeedData_CreatesTemplatesAndKeepsEmailTypesUnique()
    {
        var store = new InMemorySystemEmailTemplateStore();
        var service = new SystemEmailTemplateService(store);

        await service.UpsertSeedDataAsync(SystemEmailTemplateSeedCatalog.GetCoreTemplates());
        await service.UpsertSeedDataAsync(SystemEmailTemplateSeedCatalog.GetCoreTemplates());

        var templates = await service.GetSystemTemplatesAsync();

        templates.Should().Contain(x => x.EmailType == EmailType.BaseTemplate);
        templates.Should().Contain(x => x.EmailType == EmailType.EmailVerification);
        templates.Select(x => x.EmailType).Should().OnlyHaveUniqueItems();
    }
}
