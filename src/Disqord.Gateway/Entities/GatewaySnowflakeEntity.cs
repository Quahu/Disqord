using System;

namespace Disqord.Gateway
{
    /// <summary>
    ///     Represents a gateway Discord entity with a unique id.
    /// </summary>
    public abstract class GatewaySnowflakeEntity : GatewayEntity, ISnowflakeEntity
    {
        /// <inheritdoc/>
        public Snowflake Id { get; }

        /// <inheritdoc/>
        public DateTimeOffset CreatedAt => Id.CreatedAt;

        public GatewaySnowflakeEntity(IGatewayClient client, Snowflake id)
            : base(client)
        {
            Id = id;
        }
    }
}
