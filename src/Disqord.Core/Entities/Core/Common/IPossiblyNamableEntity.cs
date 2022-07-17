namespace Disqord;

/// <summary>
///     Represents an entity with an optional name.
/// </summary>
public interface IPossiblyNamableEntity : IEntity
{
    /// <summary>
    ///     Gets the name of this entity.
    /// </summary>
    /// <returns>
    ///     The name of this entity or <see langword="null"/> if not present.
    /// </returns>
    string? Name { get; }
}
