using BrighterTools.Messaging.Models;
namespace BrighterTools.Messaging.Abstractions;
/// <summary>
/// Defines operations for Email Template Store.
/// </summary>
public interface IEmailTemplateStore
{
    /// <summary>
    /// Gets the get By Id Async.
    /// </summary>
    /// <param name="id">The id value.</param>
    /// <param name="cancellationToken">The cancellationToken value.</param>
    /// <returns>A task that represents the asynchronous operation and contains the operation result.</returns>
    Task<EmailTemplate?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}

