namespace Disqord;

/// <summary>
///     Represents a Discord entity with a unique <see cref="Snowflake"/> ID that is also tied to a client.
/// </summary>
public interface ISnowflakeEntity : IIdentifiableEntity, IClientEntity
{ }