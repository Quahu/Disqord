using Disqord.Models;

namespace Disqord.Gateway
{
    public class CachedCategoryChannel : CachedGuildChannel, ICategoryChannel
    {
        public CachedCategoryChannel(IGatewayClient client, ChannelJsonModel model)
            : base(client, model)
        { }
    }
}
