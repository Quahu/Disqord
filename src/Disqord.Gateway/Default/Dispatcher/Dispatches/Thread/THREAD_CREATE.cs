using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon.Collections.Synchronized;

namespace Disqord.Gateway.Default.Dispatcher;

public class ThreadCreateDispatchHandler : DispatchHandler<ChannelJsonModel, ThreadCreatedEventArgs>
{
    public override ValueTask<ThreadCreatedEventArgs?> HandleDispatchAsync(IShard shard, ChannelJsonModel model)
    {
        IThreadChannel thread;
        if (CacheProvider.TryGetChannels(model.GuildId.Value, out var channelCache))
        {
            thread = (channelCache.AddOrUpdate(model.Id,
                (_, state) =>
                {
                    var (client, model) = state;
                    return new CachedThreadChannel(client, model);
                },
                (_, state, oldThread) =>
                {
                    var (_, model) = state;
                    oldThread.Update(model);
                    return oldThread;
                }, (Client, model)) as IThreadChannel)!;
        }
        else
        {
            thread = new TransientThreadChannel(Client, model);
        }

        JsonModel.TryGetExtensionDatum<bool>(model, "newly_created", out var isThreadCreation);
        var e = new ThreadCreatedEventArgs(thread, isThreadCreation);
        return new(e);
    }
}
