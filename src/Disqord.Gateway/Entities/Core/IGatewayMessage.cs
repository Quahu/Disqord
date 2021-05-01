namespace Disqord.Gateway
{
    public interface IGatewayMessage : IMessage
    {
        /// <summary>
        ///     Gets the guild ID of this message.
        /// </summary>
        Snowflake? GuildId { get; }
    }
}
