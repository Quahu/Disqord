using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Default.Dispatcher;
using Disqord.Models;

namespace Disqord.Gateway.Default;

public class UserUpdateDispatchHandler : DispatchHandler<UserJsonModel, CurrentUserUpdatedEventArgs>
{
    public override ValueTask<CurrentUserUpdatedEventArgs?> HandleDispatchAsync(IShard shard, UserJsonModel model)
    {
        var newCurrentUser = Dispatcher.CurrentUser!;
        var oldCurrentUser = newCurrentUser.Clone() as CachedCurrentUser;
        newCurrentUser.Update(model);
        var e = new CurrentUserUpdatedEventArgs(oldCurrentUser, newCurrentUser);
        return new(e);
    }
}
