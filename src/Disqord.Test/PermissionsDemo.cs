namespace Disqord.Test
{
    class PermissionsDemo
    {
        void Code()
        {
            // these are all the same
            var channelPermissions = new ChannelPermissions(Permission.ViewChannels | Permission.SendMessages);
            channelPermissions = Permission.ViewChannels | Permission.SendMessages;
            channelPermissions = 3072; // view channel and send messages (raw value)

            // utility
            ulong rawValue = channelPermissions.RawValue; // raw value
            Permission permission = channelPermissions; // implicit permission

            if (channelPermissions.ViewChannels) ; // bool properties

            // or Has()
            if (channelPermissions.Has(Permission.ViewChannels)) ;

            // or stdlib HasFlag()
            if (channelPermissions.Flags.HasFlag(Permission.ViewChannels)) ;

            // Enumerates all flags
            foreach (Permission perm in channelPermissions)
            { }

            // what about overwriting permissions in a channel?
            var overwritePermissions = new OverwritePermissions(allowed: ChannelPermissions.None, denied: channelPermissions);
            overwritePermissions = (Permission.None, channelPermissions);

            // utility
            overwritePermissions = overwritePermissions
                .Allow(Permission.SendAttachments)
                .Deny(Permission.UseTextToSpeech)
                .Unset(Permission.ViewChannels);

            // or
            overwritePermissions.Allowed |= Permission.SendAttachments;
            overwritePermissions.Denied |= Permission.UseTextToSpeech;
            overwritePermissions.Allowed &= ~Permission.ViewChannels;
            overwritePermissions.Denied &= ~Permission.ViewChannels;
        }
    }
}