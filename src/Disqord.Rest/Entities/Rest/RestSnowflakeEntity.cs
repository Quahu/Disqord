namespace Disqord.Rest
{
    public abstract class RestSnowflakeEntity : RestDiscordEntity, ISnowflakeEntity
    {
        public Snowflake Id { get; }

        internal RestSnowflakeEntity(RestDiscordClient client, Snowflake id) : base(client)
        {
            Id = id;
        }
    }
}
