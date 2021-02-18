using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class ChannelCreateHandler : Handler<ChannelJsonModel, ChannelCreatedEventArgs>
    {
        public override async Task<ChannelCreatedEventArgs> HandleDispatchAsync(ChannelJsonModel model)
        {
            if (!model.GuildId.HasValue)
                return null;

            var channel = CachedGuildChannel.Create(Client, model);
            if (CacheProvider.TryGetCache<CachedGuildChannel>(out var cache))
            {
                await cache.AddAsync(channel).ConfigureAwait(false);
            }

            return new ChannelCreatedEventArgs(channel);
        }
    }
}
