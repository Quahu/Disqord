namespace Disqord;

/// <summary>
///     Represents an entity with a <see cref="Snowflake"/> ID.
/// </summary>
public interface IIdentifiableEntity : IEntity
{
    /// <summary>
    ///     Gets the ID of this entity.
    /// </summary>
    Snowflake Id { get; }
}
