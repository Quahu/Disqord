using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class ChannelUpdateHandler : Handler<ChannelJsonModel, ChannelUpdatedEventArgs>
    {
        public override async Task<ChannelUpdatedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, ChannelJsonModel model)
        {
            if (!model.GuildId.HasValue)
                return null;

            CachedGuildChannel oldChannel;
            IGuildChannel newChannel;
            if (CacheProvider.TryGetChannels(model.GuildId.Value, out var cache))
            {
                newChannel = cache.GetValueOrDefault(model.Id);
                oldChannel = (newChannel as CachedGuildChannel).Clone() as CachedGuildChannel;
                newChannel.Update(model);
            }
            else
            {
                oldChannel = null;
                newChannel = TransientGuildChannel.Create(Client, model);
            }

            return new ChannelUpdatedEventArgs(oldChannel, newChannel);
        }
    }
}
