namespace Disqord.Gateway
{
    public static partial class GatewayEntityExtensions
    {
        public static CachedStageChannel GetChannel(this IStage stage)
        {
            var client = stage.GetGatewayClient();
            return client.GetChannel(stage.GuildId, stage.ChannelId) as CachedStageChannel;
        }
    }
}