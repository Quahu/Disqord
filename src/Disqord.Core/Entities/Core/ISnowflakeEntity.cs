using System;

namespace Disqord
{
    /// <summary>
    ///     Represents a Discord entity with a unique <see cref="Snowflake"/> ID.
    /// </summary>
    public interface ISnowflakeEntity : IEntity
    {
        /// <summary>
        ///     Gets the ID of this entity.
        /// </summary>
        Snowflake Id { get; }

        /// <summary>
        ///     Gets the creation date of this entity. Short for <see cref="Snowflake.CreatedAt"/> from <see cref="Id"/>.
        /// </summary>
        DateTimeOffset CreatedAt { get; }
    }
}
