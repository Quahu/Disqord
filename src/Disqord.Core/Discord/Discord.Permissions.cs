using Qommon;

namespace Disqord;

public static partial class Discord
{
    public static class PermissionCalculation
    {
        /// <summary>
        ///     Calculates the channel permissions of the member.
        /// </summary>
        /// <param name="guild"> The guild of the member to use for the calculation. </param>
        /// <param name="channel"> The channel to use for the calculation. </param>
        /// <param name="member"> The member to use for the calculation. </param>
        /// <param name="roles"> The roles of the member to use for the calculation. This must include the <c>@everyone</c> role. </param>
        /// <returns>
        ///     The channel permissions of the member.
        /// </returns>
        public static Permissions CalculateChannelPermissions(IGuild guild, IGuildChannel channel, IMember member, IRole[] roles)
        {
            Guard.IsNotNull(guild);
            Guard.IsNotNull(channel);
            Guard.IsNotNull(member);
            Guard.IsNotNull(roles);

            if (member.GuildId != channel.GuildId)
                Throw.InvalidOperationException("The entities must be from the same guild.");

            Guard.HasSizeGreaterThan(roles, 0);

            var permissions = CalculateGuildPermissions(guild, member, roles);
            if (permissions.HasFlag(Permissions.Administrator))
                return Permissions.All;

            var overwrittenPermissionsAllowed = Permissions.None;
            var overwrittenPermissionsDenied = Permissions.None;
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

                foreach (var role in roles)
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
                        overwrittenPermissionsDenied |= overwritePermissions.Denied;
                        overwrittenPermissionsAllowed |= overwritePermissions.Allowed;
                    }
                }
            }

            // Apply the total overwrite permissions.
            permissions &= ~overwrittenPermissionsDenied;
            permissions |= overwrittenPermissionsAllowed;

            // Apply the member overwrite permissions.
            permissions &= ~overwrittenMemberPermissions.Denied;
            permissions |= overwrittenMemberPermissions.Allowed;

            if (!permissions.HasFlag(Permissions.ViewChannels))
                return Permissions.None;

            // In threads we set the SendMessages permission based on the SendMessagesInThreads permission.
            if (channel is IThreadChannel)
            {
                if (permissions.HasFlag(Permissions.SendMessagesInThreads))
                {
                    permissions |= Permissions.SendMessages;
                }
                else
                {
                    permissions &= ~Permissions.SendMessages;
                }
            }

            //  If the SendMessages permission is denied the permissions below are also denied implicitly.
            if (channel is IMessageGuildChannel && !permissions.HasFlag(Permissions.SendMessages))
            {
                const Permissions implicitlyDenied = Permissions.SendAttachments | Permissions.SendEmbeds
                    | Permissions.MentionEveryone | Permissions.UseTextToSpeech;

                permissions &= ~implicitlyDenied;
            }

            return permissions;
        }

        /// <summary>
        ///     Calculates the guild permissions of the member.
        /// </summary>
        /// <param name="guild"> The guild of the member to use for the calculation. </param>
        /// <param name="member"> The member to use for the calculation. </param>
        /// <param name="roles"> The roles of the member to use for the calculation. This must include the <c>@everyone</c> role. </param>
        /// <returns>
        ///     The guild permissions of the member.
        /// </returns>
        public static Permissions CalculateGuildPermissions(IGuild guild, IMember member, IRole[] roles)
        {
            Guard.IsNotNull(guild);
            Guard.IsNotNull(member);
            Guard.IsNotNull(roles);

            if (guild.Id != member.GuildId)
                Throw.InvalidOperationException("The entities must be from the same guild.");

            if (guild.OwnerId == member.Id)
                return Permissions.All;

            var permissions = Permissions.None;
            foreach (var role in roles)
            {
                permissions |= role.Permissions;
            }

            return permissions.HasFlag(Permissions.Administrator)
                ? Permissions.All
                : permissions;
        }
    }
}
