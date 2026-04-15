using BrighterTools.Messaging.Enums;
using BrighterTools.Messaging.Models;
namespace BrighterTools.Messaging.Abstractions;
/// <summary>
/// Defines operations for System Email Template Store.
/// </summary>
public interface ISystemEmailTemplateStore
{
    /// <summary>
    /// Gets the get By Id Async.
    /// </summary>
    /// <param name="id">The id value.</param>
    /// <param name="cancellationToken">The cancellationToken value.</param>
    /// <returns>A task that represents the asynchronous operation and contains the operation result.</returns>
    Task<SystemEmailTemplate?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets the get By Email Type Async.
    /// </summary>
    /// <param name="emailType">The emailType value.</param>
    /// <param name="cancellationToken">The cancellationToken value.</param>
    /// <returns>A task that represents the asynchronous operation and contains the operation result.</returns>
    Task<SystemEmailTemplate?> GetByEmailTypeAsync(EmailType emailType, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets the get System Templates Async.
    /// </summary>
    /// <param name="cancellationToken">The cancellationToken value.</param>
    /// <returns>A task that represents the asynchronous operation and contains the operation result.</returns>
    Task<IReadOnlyList<SystemEmailTemplate>> GetSystemTemplatesAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// Executes the upsert Async operation.
    /// </summary>
    /// <param name="templates">The templates value.</param>
    /// <param name="cancellationToken">The cancellationToken value.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpsertAsync(IEnumerable<SystemEmailTemplate> templates, CancellationToken cancellationToken = default);
}

