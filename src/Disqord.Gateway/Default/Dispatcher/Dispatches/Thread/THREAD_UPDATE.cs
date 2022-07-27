using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class ThreadUpdateDispatchHandler : DispatchHandler<ChannelJsonModel, ThreadUpdatedEventArgs>
{
    public override ValueTask<ThreadUpdatedEventArgs?> HandleDispatchAsync(IShard shard, ChannelJsonModel model)
    {
        CachedThreadChannel? oldThread;
        IThreadChannel newThread;
        if (CacheProvider.TryGetChannels(model.GuildId.Value, out var channelCache) && channelCache.TryGetValue(model.Id, out var cachedThread))
        {
            newThread = (cachedThread as CachedThreadChannel)!;
            oldThread = cachedThread.Clone() as CachedThreadChannel;
            newThread.Update(model);
        }
        else
        {
            oldThread = null;
            newThread = new TransientThreadChannel(Client, model);
        }

        var e = new ThreadUpdatedEventArgs(oldThread, newThread);
        return new(e);
    }
}
