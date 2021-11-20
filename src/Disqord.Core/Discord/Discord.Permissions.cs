using System;
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

            /// <summary>
            ///     Calculates a member's channel permissions.
            /// </summary>
            /// <param name="guild"> The guild the member to use for the calculation. </param>
            /// <param name="channel"> The channel to use for the calculation. </param>
            /// <param name="member"> The member to use for the calculation. </param>
            /// <param name="roles"> The roles of the member to use for the calculation. This must include the <c>@everyone</c> role. </param>
            /// <returns>
            ///     The channel permissions of the member.
            /// </returns>
            public static ChannelPermissions CalculatePermissions(IGuild guild, IGuildChannel channel, IMember member, IEnumerable<IRole> roles)
            {
                Guard.IsNotNull(guild);
                Guard.IsNotNull(channel);
                Guard.IsNotNull(member);
                Guard.IsNotNull(roles);

                if (member.GuildId != channel.GuildId)
                    Throw.InvalidOperationException("The entities must be from the same guild.");

                var rolesArray = roles.ToArray();
                var guildPermissions = CalculatePermissions(guild, member, rolesArray);
                if (guildPermissions.Administrator)
                    return ChannelPermissions.All;

                var permissions = ChannelPermissions.Mask(guildPermissions, channel);
                Array.Sort(rolesArray, (a, b) =>
                {
                    var difference = a.Position.CompareTo(b.Position);
                    if (difference != 0)
                        return difference;

                    return a.Id.CompareTo(b.Id);
                });

                var overwrites = channel.Overwrites;
                foreach (var role in rolesArray)
                {
                    for (var i = 0; i < overwrites.Count; i++)
                    {
                        var overwrite = overwrites[i];
                        if (overwrite.TargetType != OverwriteTargetType.Role || overwrite.TargetId != role.Id)
                            continue;

                        var overwritePermissions = overwrite.Permissions;
                        permissions -= overwritePermissions.Denied;
                        permissions += overwritePermissions.Allowed;
                        break;
                    }
                }

                for (var i = 0; i < overwrites.Count; i++)
                {
                    var overwrite = overwrites[i];
                    if (overwrite.TargetType != OverwriteTargetType.Member || overwrite.TargetId != member.Id)
                        continue;

                    var overwritePermissions = overwrite.Permissions;
                    permissions -= overwritePermissions.Denied;
                    permissions += overwritePermissions.Allowed;
                    break;
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

            /// <summary>
            ///     Calculates a member's guild permissions.
            /// </summary>
            /// <param name="guild"> The guild the member to use for the calculation. </param>
            /// <param name="member"> The member to use for the calculation. </param>
            /// <param name="roles"> The roles of the member to use for the calculation. This must include the <c>@everyone</c> role. </param>
            /// <returns>
            ///     The channel permissions of the member.
            /// </returns>
            public static GuildPermissions CalculatePermissions(IGuild guild, IMember member, IEnumerable<IRole> roles)
            {
                Guard.IsNotNull(guild);
                Guard.IsNotNull(member);
                Guard.IsNotNull(roles);

                if (guild.Id != member.GuildId)
                    Throw.InvalidOperationException("The entities must be from the same guild.");

                if (guild.OwnerId == member.Id)
                    return GuildPermissions.All;

                var permissions = roles.Aggregate(Permission.None, (permissions, role) => permissions | role.Permissions);
                return permissions.HasFlag(Permission.Administrator)
                    ? GuildPermissions.All
                    : permissions;
            }
        }
    }
}
