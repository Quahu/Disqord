using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class InviteDeleteHandler : Handler<InviteDeleteJsonModel, InviteDeletedEventArgs>
    {
        public override ValueTask<InviteDeletedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, InviteDeleteJsonModel model)
        {
            var e = new InviteDeletedEventArgs(model.GuildId, model.ChannelId, model.Code);
            return new(e);
        }
    }
}
