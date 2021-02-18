using System.Threading.Tasks;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class GuildBanAddHandler : Handler<GuildBanAddJsonModel, BanCreatedEventArgs>
    {
        public override async Task<BanCreatedEventArgs> HandleDispatchAsync(GuildBanAddJsonModel model)
        {
            var user = await Dispatcher.GetSharedOrTransientUserAsync(model.User).ConfigureAwait(false);
            return new BanCreatedEventArgs(model.GuildId, user);
        }
    }
}
