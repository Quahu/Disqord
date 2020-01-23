using System;

namespace Disqord.Rest
{
    /// <summary>
    ///     Represents a REST Discord entity with a unique id.
    /// </summary>
    public abstract class RestSnowflakeEntity : RestDiscordEntity, ISnowflakeEntity
    {
        /// <inheritdoc/>
        public Snowflake Id { get; }

        /// <inheritdoc/>
        public DateTimeOffset CreatedAt => Id.CreatedAt;

        internal RestSnowflakeEntity(RestDiscordClient client, Snowflake id) : base(client)
        {
            Id = id;
        }
    }
}
