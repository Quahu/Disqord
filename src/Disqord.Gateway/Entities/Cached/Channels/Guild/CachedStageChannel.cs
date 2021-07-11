using Disqord.Models;

namespace Disqord.Gateway
{
    public class CachedStageChannel : CachedVocalGuildChannel, IStageChannel
    {
        public CachedStageChannel(IGatewayClient client, ChannelJsonModel model)
            : base(client, model)
        { }
    }
}
