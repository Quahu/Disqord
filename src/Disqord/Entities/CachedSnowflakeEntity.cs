namespace Disqord
{
    public abstract class CachedSnowflakeEntity : CachedDiscordEntity, ISnowflakeEntity
    {
        public Snowflake Id { get; }

        internal CachedSnowflakeEntity(DiscordClient client, ulong id) : base(client)
        {
            Id = id;
        }
    }
}
