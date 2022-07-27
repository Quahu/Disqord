using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class ThreadDeleteDispatchHandler : DispatchHandler<ChannelJsonModel, ThreadDeletedEventArgs>
{
    public override ValueTask<ThreadDeletedEventArgs?> HandleDispatchAsync(IShard shard, ChannelJsonModel model)
    {
        IThreadChannel thread;
        if (CacheProvider.TryGetChannels(model.GuildId.Value, out var channelCache) && channelCache.TryRemove(model.Id, out var cachedThread))
        {
            thread = (cachedThread as CachedThreadChannel)!;
        }
        else
        {
            thread = new TransientThreadChannel(Client, model);
        }

        //  TODO: Pass removed messages to e?
        CacheProvider.TryRemoveCache<CachedUserMessage>(model.Id, out _);

        var e = new ThreadDeletedEventArgs(thread);
        return new(e);
    }
}
