using System.Threading.Tasks;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class ChannelPinsUpdateHandler : Handler<ChannelPinsUpdatedJsonModel, ChannelPinsUpdatedEventArgs>
    {
        public override async Task<ChannelPinsUpdatedEventArgs> HandleDispatchAsync(ChannelPinsUpdatedJsonModel model)
        {
            CachedTextChannel channel = null;
            if (model.GuildId.HasValue)
            {
                if (CacheProvider.TryGetChannels(model.GuildId.Value, out var cache))
                {
                    channel = cache.GetValueOrDefault(model.ChannelId) as CachedTextChannel;
                    if (channel != null)
                        channel.LastPinTimestamp = model.LastPinTimestamp.GetValueOrDefault();
                }
            }

            return new ChannelPinsUpdatedEventArgs(model.GuildId.GetValueOrNullable(), model.ChannelId, channel);
        }
    }
}
