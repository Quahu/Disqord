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

            IGuildChannel channel;
            if (CacheProvider.TryGetChannels(model.GuildId.Value, out var cache))
            {
                channel = CachedGuildChannel.Create(Client, model);
                cache.Add(channel.Id, channel as CachedGuildChannel);
            }
            else
            {
                channel = TransientGuildChannel.Create(Client, model);
            }

            return new ChannelCreatedEventArgs(channel);
        }
    }
}
