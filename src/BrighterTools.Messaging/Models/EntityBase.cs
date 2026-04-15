namespace BrighterTools.Messaging.Models;
/// <summary>
/// Represents Entity Base.
/// </summary>
public abstract class EntityBase
{
    /// <summary>
    /// Gets or sets the D.
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Gets or sets the guid.
    /// </summary>
    public Guid Guid { get; set; } = Guid.NewGuid();
    /// <summary>
    /// Gets or sets the Created Date.
    /// </summary>
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
    /// <summary>
    /// Gets or sets the Last Updated Date.
    /// </summary>
    public DateTimeOffset LastUpdatedDate { get; set; } = DateTimeOffset.UtcNow;
}

