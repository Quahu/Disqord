using System;

namespace Disqord
{
    /// <summary>
    ///     Represents a cached Discord entity with a unique id.
    /// </summary>
    public abstract class CachedSnowflakeEntity : CachedDiscordEntity, ISnowflakeEntity
    {
        /// <inheritdoc/>
        public Snowflake Id { get; }

        /// <inheritdoc/>
        public DateTimeOffset CreatedAt => Id.CreatedAt;

        internal CachedSnowflakeEntity(DiscordClientBase client, Snowflake id) : base(client)
        {
            Id = id;
        }
    }
}
