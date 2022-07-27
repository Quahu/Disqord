using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class GuildScheduledEventDeleteDispatchHandler : DispatchHandler<GuildScheduledEventJsonModel, GuildEventDeletedEventArgs>
{
    public override ValueTask<GuildEventDeletedEventArgs?> HandleDispatchAsync(IShard shard, GuildScheduledEventJsonModel model)
    {
        IGuildEvent @event;
        if (CacheProvider.TryGetGuildEvents(model.GuildId, out var cache) && cache.TryRemove(model.Id, out var cachedEvent))
        {
            @event = cachedEvent;
        }
        else
        {
            @event = new TransientGuildEvent(Client, model);
        }

        var e = new GuildEventDeletedEventArgs(@event);
        return new(e);
    }
}
