using System.Threading.Tasks;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class GuildBanRemoveHandler : Handler<GuildBanRemoveJsonModel, BanDeletedEventArgs>
    {
        public override async Task<BanDeletedEventArgs> HandleDispatchAsync(GuildBanRemoveJsonModel model)
        {
            var user = await Dispatcher.GetSharedOrTransientUserAsync(model.User).ConfigureAwait(false);
            return new BanDeletedEventArgs(model.GuildId, user);
        }
    }
}
