using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class GuildScheduledEventUserRemoveHandler : Handler<GuildScheduledEventUserRemoveJsonModel, GuildEventMemberRemovedEventArgs>
    {
        public override ValueTask<GuildEventMemberRemovedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, GuildScheduledEventUserRemoveJsonModel model)
        {
            var e = new GuildEventMemberRemovedEventArgs(model.GuildId, model.GuildScheduledEventId, model.UserId);
            return new(e);
        }
    }
}
