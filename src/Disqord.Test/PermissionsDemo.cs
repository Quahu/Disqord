namespace Disqord.Test
{
    class PermissionsDemo
    {
        void Code()
        {
            // these are all the same
            ChannelPermissions channelPermissions = new ChannelPermissions(Permission.ViewChannels | Permission.SendMessages);
            channelPermissions = Permission.ViewChannels | Permission.SendMessages;
            channelPermissions = 3072; // view channel and send messages (raw)

            // utility
            var raw = channelPermissions.RawValue; // ulong
            raw = channelPermissions;
            var canViewChannel = channelPermissions.ViewChannel;
            // or Has()
            canViewChannel = channelPermissions.Has(Permission.ViewChannels);
            // or stdlib HasFlag()
            channelPermissions.Permissions.HasFlag(Permission.ViewChannels);
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
            overwritePermissions = overwritePermissions.Allow(Permission.SendAttachments)
                .Deny(Permission.UseTextToSpeech)
                .Unset(Permission.ViewChannels);
            // or
            overwritePermissions += Permission.SendAttachments;
            overwritePermissions -= Permission.UseTextToSpeech;
            overwritePermissions /= Permission.ViewChannels;
        }
    }
}
