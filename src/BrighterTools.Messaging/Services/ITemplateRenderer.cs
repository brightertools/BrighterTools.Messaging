using BrighterTools.Messaging.Models;
namespace BrighterTools.Messaging.Services;
/// <summary>
/// Defines operations for Template Renderer.
/// </summary>
public interface ITemplateRenderer
{
    /// <summary>
    /// Renders the render.
    /// </summary>
    /// <param name="template">The template value.</param>
    /// <param name="mergeFields">The mergeFields value.</param>
    /// <param name="subjectPrefix">The subjectPrefix value.</param>
    /// <returns>The operation result.</returns>
    EmailContent Render(EmailContent template, IReadOnlyDictionary<string, string>? mergeFields = null, string? subjectPrefix = null);
    /// <summary>
    /// Applies the apply Base Template.
    /// </summary>
    /// <param name="content">The content value.</param>
    /// <param name="baseTemplate">The baseTemplate value.</param>
    /// <returns>The operation result.</returns>
    EmailContent ApplyBaseTemplate(EmailContent content, EmailContent baseTemplate);
}

