using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class ChannelUpdateDispatchHandler : DispatchHandler<ChannelJsonModel, ChannelUpdatedEventArgs>
{
    public override ValueTask<ChannelUpdatedEventArgs?> HandleDispatchAsync(IShard shard, ChannelJsonModel model)
    {
        if (!model.GuildId.HasValue)
            return new(result: null);

        CachedGuildChannel? oldChannel;
        IGuildChannel newChannel;
        if (CacheProvider.TryGetChannels(model.GuildId.Value, out var cache) && cache.TryGetValue(model.Id, out var channel))
        {
            newChannel = channel;
            oldChannel = (channel.Clone() as CachedGuildChannel)!;
            newChannel.Update(model);
        }
        else
        {
            oldChannel = null;
            newChannel = TransientGuildChannel.Create(Client, model);
        }

        var e = new ChannelUpdatedEventArgs(oldChannel, newChannel);
        return new(e);
    }
}
