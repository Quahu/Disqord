using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class GuildRoleUpdateDispatchHandler : DispatchHandler<GuildRoleUpdateJsonModel, RoleUpdatedEventArgs>
{
    public override ValueTask<RoleUpdatedEventArgs?> HandleDispatchAsync(IShard shard, GuildRoleUpdateJsonModel model)
    {
        CachedRole? oldRole;
        IRole newRole;
        if (CacheProvider.TryGetRoles(model.GuildId, out var cache) && cache.TryGetValue(model.Role.Id, out var role))
        {
            newRole = role;
            oldRole = (role.Clone() as CachedRole)!;
            newRole.Update(model.Role);
        }
        else
        {
            oldRole = null;
            newRole = new TransientRole(Client, model.GuildId, model.Role);
        }

        var e = new RoleUpdatedEventArgs(model.GuildId, oldRole, newRole);
        return new(e);
    }
}
