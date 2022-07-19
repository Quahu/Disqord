using System;
using System.Collections.Generic;
using System.Linq;
using Qommon;
using Qommon.Collections;
using Qommon.Collections.ReadOnly;

namespace Disqord.Gateway;

public static partial class GatewayEntityExtensions
{
    /// <summary>
    ///     Gets the cached guild of this member.
    /// </summary>
    /// <param name="member"> The member to get the guild of. </param>
    /// <returns>
    ///     The guild or <see langword="null"/> if it was not cached.
    /// </returns>
    public static CachedGuild? GetGuild(this IMember member)
    {
        var client = member.GetGatewayClient();
        return client.GetGuild(member.GuildId);
    }

    /// <summary>
    ///     Gets a cached role with the given ID of this member.
    /// </summary>
    /// <param name="member"> The member to get the role of. </param>
    /// <param name="roleId"> The ID of the role to get. </param>
    /// <returns>
    ///     The role or <see langword="null"/>, if it was not cached or the member does not have the role.
    /// </returns>
    public static CachedRole? GetRole(this IMember member, Snowflake roleId)
    {
        var client = member.GetGatewayClient();
        if (roleId != member.GuildId && !member.RoleIds.Contains(roleId))
            return null;

        return client.GetRole(member.GuildId, roleId);
    }

    /// <summary>
    ///     Gets all cached roles of this member.
    /// </summary>
    /// <remarks>
    ///     This method, as opposed to <see cref="IMember.RoleIds"/>, returns the default guild role (<c>@everyone</c>).
    ///     <para/>
    ///     Discord sometimes sends members with non-existent role IDs, usually in very large guilds,
    ///     so there is a possibility that despite having all guild roles cached,
    ///     the returned roles will differ from <see cref="IMember.RoleIds"/>.
    ///     <br/>
    ///     If <paramref name="skipUncached"/> is set to <see langword="false"/> the
    ///     returned dictionary will contain null values for roles that were not found in the cache.
    /// </remarks>
    /// <param name="member"> The member to get the roles from. </param>
    /// <param name="skipUncached"> Whether to skip roles not found in the cache. </param>
    /// <returns>
    ///     A dictionary of roles of this member keyed by their IDs.
    /// </returns>
    public static IReadOnlyDictionary<Snowflake, CachedRole> GetRoles(this IMember member, bool skipUncached = true)
    {
        // TODO: maybe add an overload for skipUncached for NRT
        var client = member.GetGatewayClient();
        if (!client.CacheProvider.TryGetRoles(member.GuildId, out var cache, true))
            Throw.InvalidOperationException("The role cache must be enabled.");

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
                roles.Add(roleId, null!);
            }
        }

        if (cache.TryGetValue(member.GuildId, out var defaultRole))
        {
            roles.Add(defaultRole.Id, defaultRole);
        }
        else if (!skipUncached)
        {
            roles.Add(member.GuildId, null!);
        }

        return roles.ReadOnly();
    }

    /// <summary>
    ///     Gets the role hierarchy of this member,
    ///     i.e. the position of this member's highest role in the guild.
    /// </summary>
    /// <param name="member"> The member to get the hierarchy for. </param>
    /// <returns>
    ///     The highest role's position or <see cref="int.MaxValue"/> if this member is the guild's owner.
    /// </returns>
    public static int CalculateRoleHierarchy(this IMember member)
    {
        return member.CalculateRoleHierarchy(member.GetGuild()!);
    }

    /// <summary>
    ///     Gets the role hierarchy of this member,
    ///     i.e. the position of this member's highest role in the guild.
    /// </summary>
    /// <param name="member"> The member to get the hierarchy for. </param>
    /// <param name="guild"> The guild of the member. </param>
    /// <returns>
    ///     The highest role's position or <see cref="int.MaxValue"/> if this member is the guild's owner.
    /// </returns>
    public static int CalculateRoleHierarchy(this IMember member, IGuild guild)
    {
        if (member.GuildId != guild.Id)
            throw new ArgumentException("The member object must be from the specified guild.", nameof(member));

        if (guild.OwnerId == member.Id)
            return int.MaxValue;

        var roles = member.GetRoles();
        return roles.Count != 0
            ? roles.Values.Max(role => role.Position)
            : 0;
    }

    /// <summary>
    ///     Gets the guild permissions of this member.
    ///     This is calculated based on the roles of the member.
    /// </summary>
    /// <param name="member"> The member to get the permissions for. </param>
    /// <returns>
    ///     The guild permissions for this member.
    /// </returns>
    public static Permissions CalculateGuildPermissions(this IMember member)
    {
        return member.CalculateGuildPermissions(member.GetGuild()!);
    }

    /// <summary>
    ///     Gets the guild permissions of this member.
    ///     This is calculated based on the roles of the member.
    /// </summary>
    /// <param name="member"> The member to get the permissions for. </param>
    /// <param name="guild"> The guild of the member. </param>
    /// <returns>
    ///     The guild permissions for this member.
    /// </returns>
    public static Permissions CalculateGuildPermissions(this IMember member, IGuild guild)
    {
        if (member.GuildId != guild.Id)
            throw new ArgumentException("The member object must be from the specified guild.", nameof(member));

        return Discord.PermissionCalculation.CalculateGuildPermissions(guild, member, member.GetRoles().Values.GetArray());
    }

    /// <summary>
    ///     Gets the channel permissions of this member in the given channel.
    ///     This is calculated based on the roles of the member and overwrites in the channel.
    /// </summary>
    /// <param name="member"> The member to get the permissions for. </param>
    /// <param name="channel"> The channel to get the permissions for. </param>
    /// <returns>
    ///     The channel permissions for this member.
    /// </returns>
    public static Permissions CalculateChannelPermissions(this IMember member, IGuildChannel channel)
    {
        return member.CalculateChannelPermissions(member.GetGuild()!, channel);
    }

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
    public static Permissions CalculateChannelPermissions(this IMember member, IGuild guild, IGuildChannel channel)
    {
        if (member.GuildId != guild.Id)
            throw new ArgumentException("The member object must be from the specified guild.", nameof(member));

        return Discord.PermissionCalculation.CalculateChannelPermissions(guild, channel, member, member.GetRoles().Values.GetArray());
    }

    /// <summary>
    ///     Gets the cached voice state of the specified member.
    /// </summary>
    /// <param name="member"> The member to get the voice state of. </param>
    /// <returns>
    ///     The voice state or <see langword="null"/>, if not cached or the member is not in a voice channel.
    /// </returns>
    public static CachedVoiceState? GetVoiceState(this IMember member)
    {
        var client = member.GetGatewayClient();
        return client.GetVoiceState(member.GuildId, member.Id);
    }

    /// <summary>
    ///     Gets the cached presence of the specified member.
    /// </summary>
    /// <param name="member"> The member to get the presence of. </param>
    /// <returns>
    ///     The presence or <see langword="null"/> if not cached.
    /// </returns>
    public static CachedPresence? GetPresence(this IMember member)
    {
        var client = member.GetGatewayClient();
        return client.GetPresence(member.GuildId, member.Id);
    }
}
