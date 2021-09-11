namespace Disqord
{
    public readonly partial struct ChannelPermissions
    {
        public const ulong ALL_PERMISSIONS_VALUE = (ulong) (
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
            | Permission.UseVad
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

        public const ulong TEXT_PERMISSIONS_VALUE = (ulong) (
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

        public const ulong VOICE_PERMISSIONS_VALUE = (ulong) (
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
            | Permission.UseVad
            | Permission.ManageRoles
            | Permission.StartActivities
            // | Permission.RequestToSpeak TODO: stage channel
        );

        public const ulong CATEGORY_PERMISSIONS_VALUE = ALL_PERMISSIONS_VALUE;
    }
}
