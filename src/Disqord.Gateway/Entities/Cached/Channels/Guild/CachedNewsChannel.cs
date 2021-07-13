using Disqord.Models;

namespace Disqord.Gateway
{
    public class CachedNewsChannel : CachedTextChannel, INewsChannel
    {
        public CachedNewsChannel(IGatewayClient client, ChannelJsonModel model)
            : base(client, model)
        { }
    }
}
