using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class GuildScheduledEventCreateDispatchHandler : DispatchHandler<GuildScheduledEventJsonModel, GuildEventCreatedEventArgs>
{
    public override ValueTask<GuildEventCreatedEventArgs?> HandleDispatchAsync(IShard shard, GuildScheduledEventJsonModel model)
    {
        IGuildEvent @event;
        if (CacheProvider.TryGetGuildEvents(model.GuildId, out var cache))
        {
            @event = new CachedGuildEvent(Client, model);
            cache.Add(@event.Id, (@event as CachedGuildEvent)!);
        }
        else
        {
            @event = new TransientGuildEvent(Client, model);
        }

        var e = new GuildEventCreatedEventArgs(@event);
        return new(e);
    }
}
