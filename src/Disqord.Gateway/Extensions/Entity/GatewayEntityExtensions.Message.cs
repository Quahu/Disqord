namespace Disqord.Gateway;

public static partial class GatewayEntityExtensions
{
    /// <summary>
    ///     Gets the cached channel of this message.
    /// </summary>
    /// <param name="message"> The message to get the channel of. </param>
    /// <returns>
    ///     The channel or <see langword="null"/> if it was not cached.
    /// </returns>
    public static CachedMessageGuildChannel? GetChannel(this IGatewayMessage message)
    {
        if (message.GuildId == null)
            return null;

        var client = message.GetGatewayClient();
        return client.GetChannel(message.GuildId.Value, message.ChannelId) as CachedMessageGuildChannel;
    }

    /// <summary>
    ///     Gets a URL for this message that can be used to jump to it in the Discord client.
    /// </summary>
    /// <param name="message"> The message to get the jump URL for. </param>
    /// <returns>
    ///     The jump URL for this message.
    /// </returns>
    public static string GetJumpUrl(this IGatewayMessage message)
    {
        return Discord.MessageJumpLink(message.GuildId, message.ChannelId, message.Id);
    }
}
