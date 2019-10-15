using System;
using System.Collections.Generic;
using System.Linq;

namespace Disqord
{
    public static partial class Discord
    {
        public static class Permissions
        {
            public static bool HasFlag(ulong rawValue, Permission flag)
                => HasFlag(rawValue, (ulong) flag);

            public static bool HasFlag(ulong rawValue, ulong flag)
                => (rawValue & flag) == flag;

            public static void SetFlag(ref ulong rawValue, Permission flag)
                => SetFlag(ref rawValue, (ulong) flag);

            public static void SetFlag(ref ulong rawValue, ulong flag)
                => rawValue |= flag;

            public static void UnsetFlag(ref ulong rawValue, Permission flag)
                => UnsetFlag(ref rawValue, (ulong) flag);

            public static void UnsetFlag(ref ulong rawValue, ulong flag)
                => rawValue &= ~flag;

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

                if (channel is ITextChannel)
                {
                    if (!permissions.ViewChannel)
                    {
                        return ChannelPermissions.None;
                    }

                    else if (!permissions.SendMessages)
                    {
                        permissions -= Permission.AttachFiles;
                        permissions -= Permission.EmbedLinks;
                        permissions -= Permission.MentionEveryone;
                        permissions -= Permission.SendTtsMessages;
                    }
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
                        yield return new KeyValuePair<Permission, bool>(flag, false);

                    yield return new KeyValuePair<Permission, bool>(flag, HasFlag(value, flag));
                }
            }

            internal static readonly Permission[] _perms = ((Permission[]) Enum.GetValues(typeof(Permission))).Skip(1).ToArray();
        }
    }
}