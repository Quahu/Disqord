using System;

namespace Disqord.Gateway
{
    public static partial class GatewayEntityExtensions
    {
        public static CachedTextChannel GetChannel(this IThreadChannel thread)
        {
            var client = thread.GetGatewayClient();
            return client.GetChannel(thread.GuildId, thread.ChannelId) as CachedTextChannel;
        }

        public static CachedStageChannel GetChannel(this IStage stage)
        {
            var client = stage.GetGatewayClient();
            return client.GetChannel(stage.GuildId, stage.ChannelId) as CachedStageChannel;
        }

        public static CachedStage GetStage(this IStageChannel stageChannel)
        {
            var client = stageChannel.GetGatewayClient();
            if (client.CacheProvider.TryGetStages(stageChannel.GuildId, out var cache))
            {
                return Array.Find(cache.Values, x => x.ChannelId == stageChannel.Id);
            }
            return null;
        }
    }
}
