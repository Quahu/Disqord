using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class GuildBanAddDispatchHandler : DispatchHandler<GuildBanAddJsonModel, BanCreatedEventArgs>
{
    public override ValueTask<BanCreatedEventArgs?> HandleDispatchAsync(IShard shard, GuildBanAddJsonModel model)
    {
        var user = Dispatcher.GetSharedUserTransient(model.User);
        var e = new BanCreatedEventArgs(model.GuildId, user);
        return new(e);
    }
}
