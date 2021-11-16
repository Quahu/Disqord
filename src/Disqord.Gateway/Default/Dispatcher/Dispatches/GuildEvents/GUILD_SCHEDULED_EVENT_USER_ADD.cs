using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class GuildScheduledEventUserAddHandler : Handler<GuildScheduledEventUserAddJsonModel, GuildEventUserAddedEventArgs>
    {
        public override ValueTask<GuildEventUserAddedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, GuildScheduledEventUserAddJsonModel model)
        {
            var e = new GuildEventUserAddedEventArgs(model.GuildId, model.GuildScheduledEventId, model.UserId);
            return new(e);
        }
    }
}
