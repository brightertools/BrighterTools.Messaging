using BrighterTools.Messaging.Models;
using BrighterTools.Messaging.Services;
using FluentAssertions;

namespace BrighterTools.Messaging.Tests;

public class TemplateRendererTests
{
    [Fact]
    public void Render_EncodesHtmlButPreservesText()
    {
        var renderer = new TemplateRenderer();
        var template = new EmailContent
        {
            Subject = "Hello {{Name}}",
            Html = "<p>{{Name}}</p><div>{{MessageHtml}}</div>",
            Text = "Hello {{Name}}"
        };

        var result = renderer.Render(
            template,
            new Dictionary<string, string>
            {
                ["{{Name}}"] = "<Admin>",
                ["{{MessageHtml}}"] = "<strong>Allowed</strong>"
            },
            "[Prefix]");

        result.Subject.Should().Be("[Prefix] Hello &lt;Admin&gt;");
        result.Html.Should().Contain("&lt;Admin&gt;");
        result.Html.Should().Contain("<strong>Allowed</strong>");
        result.Text.Should().Be("Hello <Admin>");
    }

    [Fact]
    public void ApplyBaseTemplate_InjectsContentIntoWrapper()
    {
        var renderer = new TemplateRenderer();
        var result = renderer.ApplyBaseTemplate(
            new EmailContent { Subject = "Subject", Html = "<p>Body</p>", Text = "Body" },
            new EmailContent { Subject = "Base", Html = "<html>{{ContentTemplate}}</html>", Text = "Start {{ContentTemplate}} End" });

        result.Html.Should().Be("<html><p>Body</p></html>");
        result.Text.Should().Be("Start Body End");
    }
}
