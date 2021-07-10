using System;
using System.Collections.Generic;
using System.Linq;
using Disqord.Collections;

namespace Disqord.Gateway
{
    // TODO: fix docs inconsistencies in gateway extensions
    public static partial class GatewayEntityExtensions
    {
        /// <summary>
        ///     Gets the cached guild for the specified member.
        ///     Returns <see langword="null"/> if the guild is not cached.
        /// </summary>
        /// <param name="member"> The member to get the guild for. </param>
        /// <returns>
        ///     The cached guild for this member.
        /// </returns>
        public static CachedGuild GetGuild(this IMember member)
        {
            var client = member.GetGatewayClient();
            return client.GetGuild(member.GuildId);
        }

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
            if (!member.RoleIds.Any(x => x == roleId) && roleId != member.GuildId)
                return null;

            return client.GetRole(member.GuildId, roleId);
        }

        /// <summary>
        ///     Gets all cached roles for the specified member.
        ///     This, as opposed to <see cref="IMember.RoleIds"/>, returns the default guild role (<c>@everyone</c>) as well.
        ///     If <paramref name="skipUncached"/> is set to <see langword="false"/> the
        ///     returned dictionary will contain null values for roles that were not found in the cache.
        /// </summary>
        /// <remarks>
        ///     Discord sometimes sends members with non-existent role IDs, usually in very large guilds.
        ///     This means that there is a possibility that despite having all guild roles cached,
        ///     the returned roles will differ from <see cref="IMember.RoleIds"/>.
        /// </remarks>
        /// <param name="member"> The member to get the roles for. </param>
        /// <param name="skipUncached"> Whether to skip roles not found in the cache. </param>
        /// <returns>
        ///     A dictionary of cached roles for this member.
        /// </returns>
        public static IReadOnlyDictionary<Snowflake, CachedRole> GetRoles(this IMember member, bool skipUncached = true)
        {
            var client = member.GetGatewayClient();
            if (!client.CacheProvider.TryGetRoles(member.GuildId, out var cache, true))
                throw new InvalidOperationException("The role cache must be enabled.");

            var roleIds = member.RoleIds;
            var roles = new Dictionary<Snowflake, CachedRole>(roleIds.Count);
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

            if (cache.TryGetValue(member.GuildId, out var defaultRole))
            {
                roles.Add(defaultRole.Id, defaultRole);
            }
            else if (!skipUncached)
            {
                roles.Add(member.GuildId, null);
            }

            return roles.ReadOnly();
        }

        /// <inheritdoc cref="GetHierarchy(Disqord.IMember,Disqord.IGuild)"/>
        public static int GetHierarchy(this IMember member)
            => member.GetHierarchy(member.GetGuild());

        /// <summary>
        ///     Gets the role hierarchy of this member, i.e. the position of this member's
        ///     highest role in the guild.
        ///     Returns <see cref="int.MaxValue"/> if this member is the guild's owner.
        /// </summary>
        /// <param name="member"> The member to get the hierarchy for. </param>
        /// <param name="guild"> The guild of the member. </param>
        /// <returns></returns>
        public static int GetHierarchy(this IMember member, IGuild guild)
        {
            if (member.GuildId != guild.Id)
                throw new ArgumentException("The member object must be from the specified guild.", nameof(member));

            if (guild.OwnerId == member.Id)
                return int.MaxValue;

            var roles = member.GetRoles();
            return roles.Count != 0
                ? roles.Values.Max(x => x.Position)
                : 0;
        }

        /// <inheritdoc cref="GetPermissions(Disqord.IMember,Disqord.IGuild)"/>
        public static GuildPermissions GetPermissions(this IMember member)
            => member.GetPermissions(member.GetGuild());

        /// <summary>
        ///     Gets the guild permissions of this member.
        ///     This is calculated based on the roles of the member.
        /// </summary>
        /// <param name="member"> The member to get the permissions for. </param>
        /// <param name="guild"> The guild of the member. </param>
        /// <returns>
        ///     The guild permissions for this member.
        /// </returns>
        public static GuildPermissions GetPermissions(this IMember member, IGuild guild)
        {
            if (member.GuildId != guild.Id)
                throw new ArgumentException("The member object must be from the specified guild.", nameof(member));

            return Discord.Permissions.CalculatePermissions(guild, member, member.GetRoles().Values);
        }

        /// <inheritdoc cref="GetPermissions(Disqord.IMember,Disqord.IGuild,Disqord.IGuildChannel)"/>
        public static ChannelPermissions GetPermissions(this IMember member, IGuildChannel channel)
            => member.GetPermissions(member.GetGuild(), channel);

        /// <summary>
        ///     Gets the channel permissions of this member in the given channel.
        ///     This is calculated based on the roles of the member and overwrites in the channel.
        /// </summary>
        /// <param name="member"> The member to get the permissions for. </param>
        /// <param name="guild"> The guild of the member. </param>
        /// <param name="channel"> The channel to get the permissions for. </param>
        /// <returns>
        ///     The channel permissions for this member.
        /// </returns>
        public static ChannelPermissions GetPermissions(this IMember member, IGuild guild, IGuildChannel channel)
        {
            if (member.GuildId != guild.Id)
                throw new ArgumentException("The member object must be from the specified guild.", nameof(member));

            return Discord.Permissions.CalculatePermissions(guild, channel, member, member.GetRoles().Values);
        }

        /// <summary>
        ///     Gets the cached voice state for the specified member.
        ///     Returns <see langword="null"/> if the voice state is not cached.
        /// </summary>
        /// <param name="member"> The member to get the voice state for. </param>
        /// <returns>
        ///     The cached voice state for this member.
        /// </returns>
        public static CachedVoiceState GetVoiceState(this IMember member)
        {
            var client = member.GetGatewayClient();
            return client.GetVoiceState(member.GuildId, member.Id);
        }

        /// <summary>
        ///     Gets the cached presence for the specified member.
        ///     Returns <see langword="null"/> if the presence is not cached.
        /// </summary>
        /// <param name="member"> The member to get the presence for. </param>
        /// <returns>
        ///     The cached presence for this member.
        /// </returns>
        public static CachedPresence GetPresence(this IMember member)
        {
            var client = member.GetGatewayClient();
            return client.GetPresence(member.GuildId, member.Id);
        }
    }
}
