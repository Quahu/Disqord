using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class GuildRoleDeleteHandler : Handler<GuildRoleDeleteJsonModel, RoleDeletedEventArgs>
{
    public override ValueTask<RoleDeletedEventArgs?> HandleDispatchAsync(IGatewayApiClient shard, GuildRoleDeleteJsonModel model)
    {
        CachedRole? role = null;
        if (CacheProvider.TryGetRoles(model.GuildId, out var cache))
        {
            cache.TryRemove(model.RoleId, out role);
        }

        var e = new RoleDeletedEventArgs(model.GuildId, model.RoleId, role);
        return new(e);
    }
}
