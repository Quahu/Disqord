namespace Disqord.Test
{
    class PermissionsDemo
    {
        void Code()
        {
            // these are all the same
            ChannelPermissions channelPermissions = new ChannelPermissions(Permission.ViewChannel | Permission.SendMessages);
            channelPermissions = Permission.ViewChannel | Permission.SendMessages;
            channelPermissions = 3072; // view channel and send messages (raw)

            // utility
            var raw = channelPermissions.RawValue; // ulong
            raw = channelPermissions;
            var canViewChannel = channelPermissions.ViewChannel;
            // or Has()
            canViewChannel = channelPermissions.Has(Permission.ViewChannel);
            // or stdlib HasFlag()
            channelPermissions.Permissions.HasFlag(Permission.ViewChannel);
            foreach (Permission permission in channelPermissions)
            {
                // enumerates all flags
            }

            // what about overwriting permissions in a channel?
            // these are also all the same
            OverwritePermissions overwritePermissions = new OverwritePermissions(allowed: ChannelPermissions.None, denied: channelPermissions);
            overwritePermissions = new OverwritePermissions(Permission.None, channelPermissions);
            overwritePermissions = (Permission.None, channelPermissions);
            overwritePermissions = (0, 3072);

            // utility
            overwritePermissions = overwritePermissions.Allow(Permission.AttachFiles)
                .Deny(Permission.UseTextToSpeech)
                .Unset(Permission.ViewChannel);
            // or
            overwritePermissions += Permission.AttachFiles;
            overwritePermissions -= Permission.UseTextToSpeech;
            overwritePermissions /= Permission.ViewChannel;
        }
    }
}
