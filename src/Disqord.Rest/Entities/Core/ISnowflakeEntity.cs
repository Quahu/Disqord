using System;

namespace Disqord
{
    /// <summary>
    ///     Represents a Discord entity with a unique <see cref="Snowflake"/> id.
    /// </summary>
    public interface ISnowflakeEntity : IDiscordEntity
    {
        /// <summary>
        ///     Gets the id of this entity.
        /// </summary>
        Snowflake Id { get; }

        /// <summary>
        ///     Gets the creation date of this entity.
        ///     Short for <see cref="Snowflake.CreatedAt"/> using <see cref="Id"/>.
        /// </summary>
        DateTimeOffset CreatedAt { get; }
    }
}
