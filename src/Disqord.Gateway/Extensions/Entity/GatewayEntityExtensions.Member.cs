using System.Collections.Generic;
using System.Linq;
using Disqord.Collections;

namespace Disqord.Gateway
{
    public static partial class GatewayEntityExtensions
    {
        /// <summary>
        ///     Gets a cached role for the specified member.
        ///     Returns <see langword="null"/> if the role is not cached or the member does not have the given role.
        /// </summary>
        /// <param name="member"> The member to get the role for. </param>
        /// <param name="roleId"> The ID of the role to get. </param>
        /// <returns>
        ///     A cached role for this member.
        /// </returns>
        public static CachedRole GetRole(this IMember member, Snowflake roleId)
        {
            var client = member.GetGatewayClient();
            if (!member.RoleIds.Any(x => x == roleId))
                return null;

            return client.GetRole(member.GuildId, roleId);
        }

        /// <summary>
        ///     Gets all cached roles for the specified member.
        ///     If <paramref name="skipUncached"/> is set to <see langword="false"/> the
        ///     returned dictionary will contain null values for roles that were not found in the cache.
        /// </summary>
        /// <remarks>
        ///     Discord sometimes sends members with non-existent role IDs, usually in very large guilds.
        ///     This means that there is a possibility that despite having all guild roles cached,
        ///     the returned roles will differ from <see cref="IMember.RoleIds"/>.
        /// </remarks>
        /// <param name="member"> The member to get the role for. </param>
        /// <param name="skipUncached"> Whether to skip roles not found in the cache. </param>
        /// <returns>
        ///     A dictionary of cached roles for this member.
        /// </returns>
        public static IReadOnlyDictionary<Snowflake, CachedRole> GetRoles(this IMember member, bool skipUncached = true)
        {
            var client = member.GetGatewayClient();
            var roleIds = member.RoleIds;
            var roles = new Dictionary<Snowflake, CachedRole>(member.RoleIds.Count);
            if (client.CacheProvider.TryGetRoles(member.GuildId, out var cache))
            {
                for (var i = 0; i < roleIds.Count; i++)
                {
                    var roleId = roleIds[i];
                    if (cache.TryGetValue(roleId, out var role))
                    {
                        roles.Add(roleId, role);
                    }
                    else if (!skipUncached)
                    {
                        roles.Add(roleId, null);
                    }
                }
            }

            return roles.ReadOnly();
        }
    }
}
