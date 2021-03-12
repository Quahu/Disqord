using Disqord.Models;

namespace Disqord.Gateway
{
    public class CachedCategoryChannel : CachedGuildChannel, ICategoryChannel
    {
        public CachedCategoryChannel(IGatewayClient client, Snowflake guildId, ChannelJsonModel model)
            : base(client, guildId, model)
        { }
    }
}
