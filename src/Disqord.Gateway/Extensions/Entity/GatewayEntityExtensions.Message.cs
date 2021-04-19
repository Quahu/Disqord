namespace Disqord.Gateway
{
    public static partial class GatewayEntityExtensions
    {
        /// <summary>
        ///     Gets the <see cref="CachedTextChannel"/> for the specified gateway message.
        ///     Returns <see langword="null"/> if the channel is not cached or the message was sent in a private channel.
        /// </summary>
        /// <param name="message"> The message to get the channel for. </param>
        /// <returns>
        ///     The cached channel of this message.
        /// </returns>
        public static CachedTextChannel GetChannel(this IGatewayMessage message)
        {
            if (message.GuildId == null)
                return null;

            var client = message.GetGatewayClient();
            return client.GetChannel(message.GuildId.Value, message.ChannelId) as CachedTextChannel;
        }

        /// <summary>
        ///     Gets a URL that can be used to jump the specified message in the Discord client.
        /// </summary>
        /// <param name="message"> The message to get the jump URL for. </param>
        /// <returns>
        ///     The jump URL for the message.
        /// </returns>
        public static string GetJumpUrl(this IGatewayMessage message)
            => Discord.MessageJumpLink(message.GuildId, message.ChannelId, message.Id);
    }
}
