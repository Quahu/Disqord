using System;

namespace Disqord.Gateway
{
    public static partial class GatewayEntityExtensions
    {
        /// <summary>
        ///     Gets the cached parent channel of this thread.
        /// </summary>
        /// <param name="thread"> The thread to get the parent channel of. </param>
        /// <returns>
        ///     The parent channel or <see langword="null"/> if it was not cached.
        /// </returns>
        public static CachedTextChannel GetChannel(this IThreadChannel thread)
        {
            var client = thread.GetGatewayClient();
            return client.GetChannel(thread.GuildId, thread.ChannelId) as CachedTextChannel;
        }

        /// <summary>
        ///     Gets the cached stage of this stage channel.
        /// </summary>
        /// <param name="stageChannel"> The stage channel to get the stage of. </param>
        /// <returns>
        ///     The stage or <see langword="null"/> if it was not cached.
        /// </returns>
        public static CachedStage GetStage(this IStageChannel stageChannel)
        {
            var client = stageChannel.GetGatewayClient();
            if (client.CacheProvider.TryGetStages(stageChannel.GuildId, out var cache))
                return Array.Find(cache.Values, x => x.ChannelId == stageChannel.Id);

            return null;
        }
    }
}
