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

            IGuildChannel oldChannel;
            IGuildChannel channel;
            if (CacheProvider.TryGetChannels(model.GuildId.Value, out var cache))
            {
                channel = cache.GetValueOrDefault(model.Id);
                oldChannel = (IGuildChannel) (channel as CachedGuildChannel).Clone();
                channel.Update(model);
            }
            else
            {
                oldChannel = null;
                channel = TransientGuildChannel.Create(Client, model);
            }

            return new ChannelUpdatedEventArgs(oldChannel, channel);
        }
    }
}
