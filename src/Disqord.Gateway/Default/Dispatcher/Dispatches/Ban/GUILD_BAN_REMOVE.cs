using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class GuildBanRemoveHandler : Handler<GuildBanRemoveJsonModel, BanDeletedEventArgs>
    {
        public override ValueTask<BanDeletedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, GuildBanRemoveJsonModel model)
        {
            var user = Dispatcher.GetSharedUserTransient(model.User);
            var e = new BanDeletedEventArgs(model.GuildId, user);
            return new(e);
        }
    }
}
