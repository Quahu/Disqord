namespace Disqord;

public readonly partial struct ChannelPermissions
{
    private const ulong AllPermissionsValue = (ulong) (
        Permissions.CreateInvites
        | Permissions.ManageChannels
        | Permissions.AddReactions
        | Permissions.UsePrioritySpeaker
        | Permissions.Stream
        | Permissions.ViewChannels
        | Permissions.SendMessages
        | Permissions.UseTextToSpeech
        | Permissions.ManageMessages
        | Permissions.SendEmbeds
        | Permissions.SendAttachments
        | Permissions.ReadMessageHistory
        | Permissions.MentionEveryone
        | Permissions.UseExternalEmojis
        | Permissions.Connect
        | Permissions.Speak
        | Permissions.MuteMembers
        | Permissions.DeafenMembers
        | Permissions.MoveMembers
        | Permissions.UseVoiceActivity
        | Permissions.ManageRoles
        | Permissions.ManageWebhooks
        | Permissions.UseApplicationCommands
        | Permissions.RequestToSpeak
        | Permissions.ManageThreads
        | Permissions.UseExternalStickers
        | Permissions.CreatePublicThreads
        | Permissions.CreatePrivateThreads
        | Permissions.SendMessagesInThreads
        | Permissions.StartActivities);

    private const ulong TextPermissionsValue = (ulong) (
        Permissions.CreateInvites
        | Permissions.ManageChannels
        | Permissions.AddReactions
        | Permissions.ViewChannels
        | Permissions.SendMessages
        | Permissions.UseTextToSpeech
        | Permissions.ManageMessages
        | Permissions.SendEmbeds
        | Permissions.SendAttachments
        | Permissions.ReadMessageHistory
        | Permissions.MentionEveryone
        | Permissions.UseExternalEmojis
        | Permissions.ManageRoles
        | Permissions.ManageWebhooks
        | Permissions.UseApplicationCommands
        | Permissions.ManageThreads
        | Permissions.CreatePublicThreads
        | Permissions.CreatePrivateThreads
        | Permissions.UseExternalStickers
        | Permissions.SendMessagesInThreads);

    private const ulong VoicePermissionsValue = (ulong) (
        Permissions.CreateInvites
        | Permissions.ManageChannels
        | Permissions.AddReactions
        | Permissions.UsePrioritySpeaker
        | Permissions.Stream
        | Permissions.ViewChannels
        | Permissions.SendMessages
        | Permissions.UseTextToSpeech
        | Permissions.ManageMessages
        | Permissions.SendEmbeds
        | Permissions.SendAttachments
        | Permissions.ReadMessageHistory
        | Permissions.MentionEveryone
        | Permissions.UseExternalEmojis
        | Permissions.Connect
        | Permissions.Speak
        | Permissions.MuteMembers
        | Permissions.DeafenMembers
        | Permissions.MoveMembers
        | Permissions.UseVoiceActivity
        | Permissions.ManageRoles
        | Permissions.ManageWebhooks
        | Permissions.UseApplicationCommands
        | Permissions.UseExternalStickers
        | Permissions.ManageEvents
        | Permissions.StartActivities
    );

    private const ulong StagePermissionsValue = (ulong) (
        Permissions.CreateInvites
        | Permissions.ManageChannels
        | Permissions.ViewChannels
        | Permissions.MentionEveryone
        | Permissions.Connect
        | Permissions.MuteMembers
        | Permissions.DeafenMembers
        | Permissions.MoveMembers
        | Permissions.ManageRoles
        | Permissions.RequestToSpeak // This is currently not used by the client or API but can be set and unset
        | Permissions.ManageEvents
    );

    private const ulong CategoryPermissionsValue = AllPermissionsValue;
}