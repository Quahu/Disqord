namespace Disqord
{
    public readonly partial struct ChannelPermissions
    {
        private const ulong AllPermissionsValue = (ulong) (
            Permission.CreateInvites
            | Permission.ManageChannels
            | Permission.AddReactions
            | Permission.PrioritySpeaker
            | Permission.Stream
            | Permission.ViewChannels
            | Permission.SendMessages
            | Permission.UseTextToSpeech
            | Permission.ManageMessages
            | Permission.SendEmbeds
            | Permission.SendAttachments
            | Permission.ReadMessageHistory
            | Permission.MentionEveryone
            | Permission.UseExternalEmojis
            | Permission.Connect
            | Permission.Speak
            | Permission.MuteMembers
            | Permission.DeafenMembers
            | Permission.MoveMembers
            | Permission.UseVoiceActivity
            | Permission.ManageRoles
            | Permission.ManageWebhooks
            | Permission.UseApplicationCommands
            | Permission.RequestToSpeak
            | Permission.ManageThreads
            | Permission.UseExternalStickers
            | Permission.CreatePublicThreads
            | Permission.CreatePrivateThreads
            | Permission.SendMessagesInThreads
            | Permission.StartActivities);

        private const ulong TextPermissionsValue = (ulong) (
            Permission.CreateInvites
            | Permission.ManageChannels
            | Permission.AddReactions
            | Permission.ViewChannels
            | Permission.SendMessages
            | Permission.UseTextToSpeech
            | Permission.ManageMessages
            | Permission.SendEmbeds
            | Permission.SendAttachments
            | Permission.ReadMessageHistory
            | Permission.MentionEveryone
            | Permission.UseExternalEmojis
            | Permission.ManageRoles
            | Permission.ManageWebhooks
            | Permission.UseApplicationCommands
            | Permission.ManageThreads
            | Permission.CreatePublicThreads
            | Permission.CreatePrivateThreads
            | Permission.UseExternalStickers
            | Permission.SendMessagesInThreads);

        private const ulong VoicePermissionsValue = (ulong) (
            Permission.CreateInvites
            | Permission.ManageChannels
            | Permission.PrioritySpeaker
            | Permission.Stream
            | Permission.ViewChannels
            | Permission.Connect
            | Permission.Speak
            | Permission.MuteMembers
            | Permission.DeafenMembers
            | Permission.MoveMembers
            | Permission.UseVoiceActivity
            | Permission.ManageRoles
            | Permission.ManageEvents
            | Permission.StartActivities
        );

        private const ulong StagePermissionsValue = (ulong) (
            Permission.CreateInvites
            | Permission.ManageChannels
            | Permission.ViewChannels
            | Permission.MentionEveryone
            | Permission.Connect
            | Permission.MuteMembers
            | Permission.DeafenMembers
            | Permission.MoveMembers
            | Permission.ManageRoles
            | Permission.RequestToSpeak // This is currently not used by the client or API but can be set and unset
            | Permission.ManageEvents
        );

        private const ulong CategoryPermissionsValue = AllPermissionsValue;
    }
}
