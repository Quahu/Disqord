using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class ChannelDeleteDispatchHandler : DispatchHandler<ChannelJsonModel, ChannelDeletedEventArgs>
{
    public override ValueTask<ChannelDeletedEventArgs?> HandleDispatchAsync(IShard shard, ChannelJsonModel model)
    {
        if (!model.GuildId.HasValue)
            return new(result: null);

        IGuildChannel channel;
        if (CacheProvider.TryGetChannels(model.GuildId.Value, out var cache) && cache.TryRemove(model.Id, out var cachedChannel))
        {
            channel = cachedChannel;
        }
        else
        {
            channel = TransientGuildChannel.Create(Client, model);
        }

        //  TODO: Pass removed messages to e?
        CacheProvider.TryRemoveCache<CachedUserMessage>(model.Id, out _);

        var e = new ChannelDeletedEventArgs(channel);
        return new(e);
    }
}
