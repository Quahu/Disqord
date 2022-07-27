using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class ChannelCreateDispatchHandler : DispatchHandler<ChannelJsonModel, ChannelCreatedEventArgs>
{
    public override ValueTask<ChannelCreatedEventArgs?> HandleDispatchAsync(IShard shard, ChannelJsonModel model)
    {
        if (!model.GuildId.HasValue)
            return new(result: null);

        IGuildChannel channel;
        if (CacheProvider.TryGetChannels(model.GuildId.Value, out var cache))
        {
            channel = CachedGuildChannel.Create(Client, model);
            cache.Add(channel.Id, (channel as CachedGuildChannel)!);
        }
        else
        {
            channel = TransientGuildChannel.Create(Client, model);
        }

        var e = new ChannelCreatedEventArgs(channel);
        return new(e);
    }
}
