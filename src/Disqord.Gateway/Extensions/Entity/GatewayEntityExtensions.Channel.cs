namespace Disqord.Gateway
{
    public static partial class GatewayEntityExtensions
    {
        public static CachedTextChannel GetChannel(this IThreadChannel thread)
        {
            var client = thread.GetGatewayClient();
            return client.GetChannel(thread.GuildId, thread.ChannelId) as CachedTextChannel;
        }
    }
}
