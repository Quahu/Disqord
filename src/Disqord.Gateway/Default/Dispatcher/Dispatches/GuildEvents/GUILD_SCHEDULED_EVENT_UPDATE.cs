using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class GuildScheduledEventUpdateHandler : Handler<GuildScheduledEventJsonModel, GuildEventUpdatedEventArgs>
    {
        public override ValueTask<GuildEventUpdatedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, GuildScheduledEventJsonModel model)
        {
            CachedGuildEvent oldEvent;
            IGuildEvent newEvent;
            if (CacheProvider.TryGetGuildEvents(model.GuildId, out var cache) && cache.TryGetValue(model.Id, out var @event))
            {
                newEvent = @event;
                oldEvent = @event.Clone() as CachedGuildEvent;
                newEvent.Update(model);
            }
            else
            {
                oldEvent = null;
                newEvent = new TransientGuildEvent(Client, model);
            }

            var e = new GuildEventUpdatedEventArgs(oldEvent, newEvent);
            return new(e);
        }
    }
}
