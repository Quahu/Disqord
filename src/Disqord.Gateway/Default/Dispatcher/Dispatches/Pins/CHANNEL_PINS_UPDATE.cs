using System.Threading.Tasks;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class ChannelPinsUpdateHandler : Handler<ChannelPinsUpdatedJsonModel, ChannelPinsUpdatedEventArgs>
    {
        public override async Task<ChannelPinsUpdatedEventArgs> HandleDispatchAsync(ChannelPinsUpdatedJsonModel model)
        {
            IMessageChannel channel = null;
            if (model.GuildId.HasValue)
            {
                if (CacheProvider.TryGetChannels(model.GuildId.Value, out var cache))
                {
                    channel = cache.GetValueOrDefault(model.ChannelId) as IMessageChannel;
                    if (channel is CachedTextChannel textChannel)
                        textChannel.LastPinTimestamp = model.LastPinTimestamp.GetValueOrDefault();
                }
            }

            return new ChannelPinsUpdatedEventArgs(model.GuildId.GetValueOrNullable(), model.ChannelId, channel);
        }
    }
}
