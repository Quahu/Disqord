namespace Disqord;

/// <summary>
///     Represents a Discord entity that exists within an application.
/// </summary>
public interface IApplicationEntity : IEntity
{
    /// <summary>
    ///     Gets the ID of the application this entity is tied to.
    /// </summary>
    Snowflake ApplicationId { get; }
}