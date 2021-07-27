using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class ThreadCreateHandler : Handler<ChannelJsonModel, ThreadCreatedEventArgs>
    {
        public override ValueTask<ThreadCreatedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, ChannelJsonModel model)
        {
            IThreadChannel thread;
            if (CacheProvider.TryGetChannels(model.GuildId.Value, out var channelCache))
            {
                thread = channelCache.AddOrUpdate(model.Id,
                    (_, tuple) =>
                    {
                        var (client, model) = tuple;
                        return new CachedThreadChannel(client, model);
                    },
                    (_, tuple, oldThread) =>
                    {
                        var (_, model) = tuple;
                        oldThread.Update(model);
                        return oldThread;
                    }, (Client, model)) as IThreadChannel;
            }
            else
            {
                thread = new TransientThreadChannel(Client, model);
            }

            var e = new ThreadCreatedEventArgs(thread);
            return new(e);
        }
    }
}
