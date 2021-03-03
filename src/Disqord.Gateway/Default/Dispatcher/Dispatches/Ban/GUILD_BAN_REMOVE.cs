using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class GuildBanRemoveHandler : Handler<GuildBanRemoveJsonModel, BanDeletedEventArgs>
    {
        public override async Task<BanDeletedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, GuildBanRemoveJsonModel model)
        {
            var user = Dispatcher.GetSharedOrTransientUser(model.User);
            return new BanDeletedEventArgs(model.GuildId, user);
        }
    }
}
