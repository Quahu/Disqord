using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class ChannelDeleteHandler : Handler<ChannelJsonModel, ChannelDeletedEventArgs>
    {
        public override async Task<ChannelDeletedEventArgs> HandleDispatchAsync(ChannelJsonModel model)
        {
            if (!model.GuildId.HasValue)
                return null;

            IGuildChannel channel;
            if (CacheProvider.TryGetCache<CachedGuildChannel>(out var cache))
            {
                channel = await cache.RemoveAsync(model.Id).ConfigureAwait(false);
            }
            else
            {
                channel = TransientGuildChannel.Create(Client, model);
            }

            return new ChannelDeletedEventArgs(channel);
        }
    }
}
