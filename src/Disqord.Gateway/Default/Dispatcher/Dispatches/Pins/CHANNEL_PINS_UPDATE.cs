using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class ChannelPinsUpdateHandler : Handler<ChannelPinsUpdatedJsonModel, ChannelPinsUpdatedEventArgs>
    {
        public override ValueTask<ChannelPinsUpdatedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, ChannelPinsUpdatedJsonModel model)
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

            var e = new ChannelPinsUpdatedEventArgs(model.GuildId.GetValueOrNullable(), model.ChannelId, channel);
            return new(e);
        }
    }
}
