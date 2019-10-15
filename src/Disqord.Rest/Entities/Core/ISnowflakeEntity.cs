namespace Disqord
{
    public interface ISnowflakeEntity : IDiscordEntity
    {
        Snowflake Id { get; }
    }
}
