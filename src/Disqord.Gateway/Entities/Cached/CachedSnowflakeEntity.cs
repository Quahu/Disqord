namespace Disqord.Gateway;

/// <inheritdoc cref="ISnowflakeEntity"/>
public abstract class CachedSnowflakeEntity : CachedEntity, ISnowflakeEntity
{
    /// <inheritdoc/>
    public Snowflake Id { get; }

    protected CachedSnowflakeEntity(IGatewayClient client, Snowflake id)
        : base(client)
    {
        Id = id;
    }
}