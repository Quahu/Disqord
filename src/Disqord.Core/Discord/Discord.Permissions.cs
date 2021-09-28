using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Disqord.Utilities;
using Qommon;

namespace Disqord
{
    public static partial class Discord
    {
        public static class Permissions
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static void SetFlag(ref ulong rawValue, Permission flag)
                => FlagUtilities.SetFlag(ref rawValue, (ulong) flag);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool HasFlag(ulong rawValue, Permission flag)
                => FlagUtilities.HasFlag(rawValue, (ulong) flag);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static void UnsetFlag(ref ulong rawValue, Permission flag)
                => FlagUtilities.UnsetFlag(ref rawValue, (ulong) flag);

            public static ChannelPermissions CalculatePermissions(IGuild guild, IGuildChannel channel, IMember member, IEnumerable<IRole> roles)
            {
                Guard.IsNotNull(guild);
                Guard.IsNotNull(channel);
                Guard.IsNotNull(member);
                Guard.IsNotNull(roles);

                if (member.GuildId != channel.GuildId)
                    Throw.InvalidOperationException("The entities must be from the same guild.");

                var guildPermissions = CalculatePermissions(guild, member, roles);
                if (guildPermissions.Administrator)
                    return ChannelPermissions.All;

                var permissions = ChannelPermissions.Mask(guildPermissions, channel);
                foreach (var role in roles.OrderBy(x => x.Position).ThenBy(x => x.Id))
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

                if (!permissions.ViewChannels)
                    return ChannelPermissions.None;

                if (channel is ITextChannel && !permissions.SendMessages)
                {
                    permissions -= Permission.SendAttachments |
                        Permission.SendEmbeds |
                        Permission.MentionEveryone |
                        Permission.UseTextToSpeech;
                }

                return permissions;
            }

            public static GuildPermissions CalculatePermissions(IGuild guild, IMember member, IEnumerable<IRole> roles)
            {
                Guard.IsNotNull(guild);
                Guard.IsNotNull(member);
                Guard.IsNotNull(roles);

                if (guild.Id != member.GuildId)
                    Throw.InvalidOperationException("The entities must be from the same guild.");

                if (guild.OwnerId == member.Id)
                    return GuildPermissions.All;

                var permissions = SumPermissions(roles);
                return permissions.Administrator
                    ? GuildPermissions.All
                    : permissions;
            }

            public static GuildPermissions SumPermissions(IEnumerable<IRole> roles)
            {
                Guard.IsNotNull(roles);

                return roles.Aggregate(GuildPermissions.None, (permissions, role) => permissions + role.Permissions);
            }
        }
    }
}
