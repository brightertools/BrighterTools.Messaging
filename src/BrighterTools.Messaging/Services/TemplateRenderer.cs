using System.Net;
using BrighterTools.Messaging.Models;
namespace BrighterTools.Messaging.Services;
/// <summary>
/// Represents Template Renderer.
/// </summary>
public class TemplateRenderer : ITemplateRenderer
{
    private const string ContentTemplateReplaceToken = "{{ContentTemplate}}";
    private static readonly string[] HtmlPassthroughSuffixes = ["Message}}", "Html}}"]; 
    /// <summary>
    /// Renders the operation.
    /// </summary>
    public EmailContent Render(EmailContent template, IReadOnlyDictionary<string, string>? mergeFields = null, string? subjectPrefix = null)
    {
        var result = new EmailContent { Subject = template.Subject, Html = template.Html, Text = template.Text };
        if (mergeFields != null)
        {
            foreach (var mergeField in mergeFields)
            {
                var htmlValue = HtmlPassthroughSuffixes.Any(suffix => mergeField.Key.EndsWith(suffix, StringComparison.Ordinal)) ? mergeField.Value : WebUtility.HtmlEncode(mergeField.Value);
                result.Subject = result.Subject.Replace(mergeField.Key, htmlValue, StringComparison.Ordinal);
                result.Html = result.Html.Replace(mergeField.Key, htmlValue, StringComparison.Ordinal);
                result.Text = result.Text.Replace(mergeField.Key, mergeField.Value, StringComparison.Ordinal);
            }
        }
        if (!string.IsNullOrWhiteSpace(subjectPrefix))
        {
            result.Subject = $"{subjectPrefix} {result.Subject}".Trim();
        }
        return result;
    }
    /// <summary>
    /// Applies Base Template.
    /// </summary>
    public EmailContent ApplyBaseTemplate(EmailContent content, EmailContent baseTemplate)
    {
        return new EmailContent
        {
            Subject = content.Subject,
            Html = baseTemplate.Html.Contains(ContentTemplateReplaceToken, StringComparison.Ordinal) ? baseTemplate.Html.Replace(ContentTemplateReplaceToken, content.Html, StringComparison.Ordinal) : content.Html,
            Text = baseTemplate.Text.Contains(ContentTemplateReplaceToken, StringComparison.Ordinal) ? baseTemplate.Text.Replace(ContentTemplateReplaceToken, content.Text, StringComparison.Ordinal) : content.Text
        };
    }
}

