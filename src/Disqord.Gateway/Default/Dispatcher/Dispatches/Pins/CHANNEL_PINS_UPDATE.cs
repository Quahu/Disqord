using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Qommon;
using Qommon.Collections.Synchronized;

namespace Disqord.Gateway.Default.Dispatcher;

public class ChannelPinsUpdateDispatchHandler : DispatchHandler<ChannelPinsUpdateJsonModel, ChannelPinsUpdatedEventArgs>
{
    public override ValueTask<ChannelPinsUpdatedEventArgs?> HandleDispatchAsync(IShard shard, ChannelPinsUpdateJsonModel model)
    {
        CachedMessageGuildChannel? channel = null;
        if (model.GuildId.HasValue)
        {
            if (CacheProvider.TryGetChannels(model.GuildId.Value, out var cache))
            {
                channel = cache.GetValueOrDefault(model.ChannelId) as CachedMessageGuildChannel;
                channel?.Update(model);
            }
        }

        var e = new ChannelPinsUpdatedEventArgs(model.GuildId.GetValueOrNullable(), model.ChannelId, channel);
        return new(e);
    }
}
