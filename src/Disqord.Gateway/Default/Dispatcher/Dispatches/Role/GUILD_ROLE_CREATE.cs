using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class GuildRoleCreateHandler : Handler<GuildRoleCreateJsonModel, RoleCreatedEventArgs>
    {
        public override ValueTask<RoleCreatedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, GuildRoleCreateJsonModel model)
        {
            IRole role;
            if (CacheProvider.TryGetRoles(model.GuildId, out var cache))
            {
                role = new CachedRole(Client, model.GuildId, model.Role);
                cache.Add(role.Id, role as CachedRole);
            }
            else
            {
                role = new TransientRole(Client, model.GuildId, model.Role);
            }

            var e = new RoleCreatedEventArgs(role);
            return new(e);
        }
    }
}
