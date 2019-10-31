namespace Disqord
{
    public abstract class CachedSnowflakeEntity : CachedDiscordEntity, ISnowflakeEntity
    {
        public Snowflake Id { get; }

        internal CachedSnowflakeEntity(DiscordClientBase client, ulong id) : base(client)
        {
            Id = id;
        }
    }
}
