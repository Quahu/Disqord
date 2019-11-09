using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Disqord
{
    public static partial class Discord
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool HasFlag(ulong rawValue, ulong flag)
            => (rawValue & flag) == flag;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void SetFlag(ref ulong rawValue, ulong flag)
            => rawValue |= flag;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void UnsetFlag(ref ulong rawValue, ulong flag)
            => rawValue &= ~flag;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool HasFlag(uint rawValue, uint flag)
            => (rawValue & flag) == flag;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void SetFlag(ref uint rawValue, uint flag)
            => rawValue |= flag;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void UnsetFlag(ref uint rawValue, uint flag)
            => rawValue &= ~flag;

        public static class Permissions
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static void SetFlag(ref ulong rawValue, Permission flag)
                => Discord.SetFlag(ref rawValue, (ulong) flag);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool HasFlag(ulong rawValue, Permission flag)
                => Discord.HasFlag(rawValue, (ulong) flag);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static void UnsetFlag(ref ulong rawValue, Permission flag)
                => Discord.UnsetFlag(ref rawValue, (ulong) flag);

            public static ChannelPermissions CalculatePermissions(IGuild guild, IGuildChannel channel, IMember member, IEnumerable<IRole> roles)
            {
                if (channel == null)
                    throw new ArgumentNullException(nameof(channel));

                var guildPermissions = CalculatePermissions(guild, member, roles);
                if (guildPermissions.Administrator)
                    return ChannelPermissions.All;

                var permissions = ChannelPermissions.Mask(guildPermissions, channel);
                foreach (var role in roles.OrderBy(x => x.Position))
                {
                    for (var i = 0; i < channel.Overwrites.Count; i++)
                    {
                        var overwrite = channel.Overwrites[i];
                        if (overwrite.TargetId != role.Id)
                            continue;

                        permissions -= overwrite.Permissions.Denied;
                        permissions += overwrite.Permissions.Allowed;
                    }
                }

                for (var i = 0; i < channel.Overwrites.Count; i++)
                {
                    var overwrite = channel.Overwrites[i];
                    if (overwrite.TargetId != member.Id)
                        continue;

                    permissions -= overwrite.Permissions.Denied;
                    permissions += overwrite.Permissions.Allowed;
                }

                if (!permissions.ViewChannel)
                    return ChannelPermissions.None;

                if (channel is ITextChannel && !permissions.SendMessages)
                {
                    permissions -= Permission.AttachFiles |
                        Permission.EmbedLinks |
                        Permission.MentionEveryone |
                        Permission.SendTtsMessages;
                }

                return permissions;
            }

            public static GuildPermissions CalculatePermissions(IGuild guild, IMember member, IEnumerable<IRole> roles)
            {
                if (guild == null)
                    throw new ArgumentNullException(nameof(guild));

                if (member == null)
                    throw new ArgumentNullException(nameof(member));

                if (roles == null)
                    throw new ArgumentNullException(nameof(roles));

                if (guild.OwnerId == member.Id)
                    return GuildPermissions.All;

                var permissions = SumPermissions(roles);
                return permissions.Administrator
                    ? GuildPermissions.All
                    : permissions;
            }

            public static GuildPermissions SumPermissions(IEnumerable<IRole> roles)
            {
                if (roles == null)
                    throw new ArgumentNullException(nameof(roles));

                return roles.Aggregate(GuildPermissions.None, (permissions, role) => permissions + role.Permissions);
            }

            public static IEnumerable<Permission> GetFlags(Permission flags)
            {
                var value = (ulong) flags;
                for (var i = 0; i < _perms.Length; i++)
                {
                    var flag = _perms[i];
                    if (flag > flags)
                        yield break;

                    if (HasFlag(value, flag))
                        yield return flag;
                }
            }

            public static IEnumerable<KeyValuePair<Permission, bool>> GetBoolFlags(Permission flags)
            {
                var value = (ulong) flags;
                for (var i = 0; i < _perms.Length; i++)
                {
                    var flag = _perms[i];
                    if (flag > flags)
                        yield return KeyValuePair.Create(flag, false);

                    yield return KeyValuePair.Create(flag, HasFlag(value, flag));
                }
            }

            internal static readonly Permission[] _perms = ((Permission[]) Enum.GetValues(typeof(Permission))).Skip(1).ToArray();
        }
    }
}