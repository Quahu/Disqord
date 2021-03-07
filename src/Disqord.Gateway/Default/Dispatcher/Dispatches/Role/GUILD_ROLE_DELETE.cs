using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class GuildRoleDeleteHandler : Handler<GuildRoleDeleteJsonModel, RoleDeletedEventArgs>
    {
        public override async Task<RoleDeletedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, GuildRoleDeleteJsonModel model)
        {
            CachedRole role = null;
            if (CacheProvider.TryGetRoles(model.GuildId, out var cache))
            {
                role = cache.GetValueOrDefault(model.RoleId);
            }

            return new RoleDeletedEventArgs(model.GuildId, model.RoleId, role);
        }
    }
}
