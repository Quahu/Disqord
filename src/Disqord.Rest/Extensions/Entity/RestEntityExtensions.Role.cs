using System;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public static partial class RestEntityExtensions
    {
        public static Task<IGuild> FetchGuildAsync(this IRole role, IRestRequestOptions options = null)
        {
            var client = role.GetRestClient();
            return client.FetchGuildAsync(role.GuildId, options: options);
        }

        public static Task<IRole> ModifyAsync(this IRole role, Action<ModifyRoleActionProperties> action, IRestRequestOptions options = null)
        {
            var client = role.GetRestClient();
            return client.ModifyRoleAsync(role.GuildId, role.Id, action, options);
        }

        public static Task DeleteAsync(this IRole role, IRestRequestOptions options = null)
        {
            var client = role.GetRestClient();
            return client.DeleteRoleAsync(role.GuildId, role.Id, options);
        }
    }
}