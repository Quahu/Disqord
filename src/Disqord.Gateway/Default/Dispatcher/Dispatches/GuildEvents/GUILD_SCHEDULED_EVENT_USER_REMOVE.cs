using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class GuildScheduledEventUserRemoveHandler : Handler<GuildScheduledEventUserRemoveJsonModel, GuildEventUserRemovedEventArgs>
    {
        public override ValueTask<GuildEventUserRemovedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, GuildScheduledEventUserRemoveJsonModel model)
        {
            var e = new GuildEventUserRemovedEventArgs(model.GuildId, model.GuildScheduledEventId, model.UserId);
            return new(e);
        }
    }
}
