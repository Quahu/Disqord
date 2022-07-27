using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Default.Dispatcher;
using Disqord.Models;

namespace Disqord.Gateway.Default;

public class UserUpdateDispatchHandler : DispatchHandler<UserJsonModel, CurrentUserUpdatedEventArgs>
{
    private ReadyDispatchHandler _readyDispatchHandler = null!;

    public override void Bind(DefaultGatewayDispatcher value)
    {
        _readyDispatchHandler = (value["READY"] as ReadyDispatchHandler)!;

        base.Bind(value);
    }

    public override ValueTask<CurrentUserUpdatedEventArgs?> HandleDispatchAsync(IShard shard, UserJsonModel model)
    {
        var newCurrentUser = _readyDispatchHandler.CurrentUser!;
        var oldCurrentUser = newCurrentUser.Clone() as CachedCurrentUser;
        newCurrentUser.Update(model);
        var e = new CurrentUserUpdatedEventArgs(oldCurrentUser, newCurrentUser);
        return new(e);
    }
}
