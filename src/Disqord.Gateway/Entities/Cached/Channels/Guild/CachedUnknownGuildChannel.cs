using Disqord.Models;

namespace Disqord.Gateway
{
    public class CachedUnknownGuildChannel : CachedCategorizableGuildChannel, IUnknownGuildChannel
    {
        public CachedUnknownGuildChannel(IGatewayClient client, ChannelJsonModel model)
            : base(client, model)
        { }
    }
}
