using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Default.Dispatcher;
using Disqord.Models;

namespace Disqord.Gateway.Default;

public class UserUpdateHandler : Handler<UserJsonModel, CurrentUserUpdatedEventArgs>
{
    private ReadyHandler _readyHandler = null!;

    public override void Bind(DefaultGatewayDispatcher value)
    {
        _readyHandler = (value["READY"] as ReadyHandler)!;

        base.Bind(value);
    }

    public override ValueTask<CurrentUserUpdatedEventArgs?> HandleDispatchAsync(IGatewayApiClient shard, UserJsonModel model)
    {
        var newCurrentUser = _readyHandler.CurrentUser!;
        var oldCurrentUser = newCurrentUser.Clone() as CachedCurrentUser;
        newCurrentUser.Update(model);
        var e = new CurrentUserUpdatedEventArgs(oldCurrentUser, newCurrentUser);
        return new(e);
    }
}
