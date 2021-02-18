using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class ChannelUpdateHandler : Handler<ChannelJsonModel, ChannelUpdatedEventArgs>
    {
        public override async Task<ChannelUpdatedEventArgs> HandleDispatchAsync(ChannelJsonModel model)
        {
            if (!model.GuildId.HasValue)
                return null;

            IGuildChannel oldChannel;
            IGuildChannel channel;
            if (Client.CacheProvider.TryGetCache<CachedGuildChannel>(out var cache))
            {
                channel = await cache.GetAsync(model.Id).ConfigureAwait(false);
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
