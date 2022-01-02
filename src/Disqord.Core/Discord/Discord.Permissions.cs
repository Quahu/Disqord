using System.Collections.Generic;
using System.Linq;
using Qommon;

namespace Disqord
{
    public static partial class Discord
    {
        public static class Permissions
        {
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
                Guard.HasSizeGreaterThan(rolesArray, 0);

                var guildPermissions = CalculatePermissions(guild, member, rolesArray);
                if (guildPermissions.Administrator)
                    return ChannelPermissions.All;

                var permissions = ChannelPermissions.Mask(guildPermissions, channel);
                var overwrittenPermissions = OverwritePermissions.None;
                var overwrittenMemberPermissions = OverwritePermissions.None;
                var overwrites = channel.Overwrites;
                var overwriteCount = overwrites.Count;
                for (var i = 0; i < overwriteCount; i++)
                {
                    var overwrite = overwrites[i];
                    if (overwrite.TargetType == OverwriteTargetType.Member)
                    {
                        // Skips the overwrite if it's a member overwrite and it doesn't target the member.
                        if (overwrite.TargetId != member.Id)
                            continue;

                        // Stores the overwritten permissions for the member.
                        overwrittenMemberPermissions = overwrite.Permissions;
                        continue;
                    }

                    foreach (var role in rolesArray)
                    {
                        // Skips the role if it's not for the current overwrite.
                        if (overwrite.TargetId != role.Id)
                            continue;

                        var overwritePermissions = overwrite.Permissions;
                        if (role.Id == role.GuildId)
                        {
                            // If the role is the @everyone role, apply the permissions directly.
                            permissions &= ~overwritePermissions.Denied;
                            permissions |= overwritePermissions.Allowed;
                        }
                        else
                        {
                            // Sum the overwrite permissions.
                            overwrittenPermissions.Denied |= overwritePermissions.Denied;
                            overwrittenPermissions.Allowed |= overwritePermissions.Allowed;
                        }
                    }
                }

                // Apply the total overwrite permissions.
                permissions &= ~overwrittenPermissions.Denied;
                permissions |= overwrittenPermissions.Allowed;

                // Apply the member overwrite permissions.
                permissions &= ~overwrittenMemberPermissions.Denied;
                permissions |= overwrittenMemberPermissions.Allowed;

                if (!permissions.ViewChannels)
                    return ChannelPermissions.None;

                // In threads, the SendMessagesInThreads permission is used instead of the SendMessages permission
                if (channel is IThreadChannel)
                {
                    if (permissions.SendMessagesInThreads)
                        permissions |= Permission.SendMessages;
                    else
                        permissions &= ~Permission.SendMessages;
                }

                if (channel is IMessageGuildChannel && !permissions.SendMessages)
                {
                    permissions &= ~(Permission.SendAttachments |
                        Permission.SendEmbeds |
                        Permission.MentionEveryone |
                        Permission.UseTextToSpeech);
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
