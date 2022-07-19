namespace Disqord;

public readonly partial struct GuildPermissions
{
    private const ulong AllPermissionsValue = (ulong) (
        Permissions.CreateInvites
        | Permissions.KickMembers
        | Permissions.BanMembers
        | Permissions.Administrator
        | Permissions.ManageChannels
        | Permissions.ManageGuild
        | Permissions.AddReactions
        | Permissions.ViewAuditLog
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
        | Permissions.ViewGuildInsights
        | Permissions.Connect
        | Permissions.Speak
        | Permissions.MuteMembers
        | Permissions.DeafenMembers
        | Permissions.MoveMembers
        | Permissions.UseVoiceActivity
        | Permissions.SetNick
        | Permissions.ManageNicks
        | Permissions.ManageRoles
        | Permissions.ManageWebhooks
        | Permissions.ManageEmojisAndStickers
        | Permissions.UseApplicationCommands
        | Permissions.RequestToSpeak
        | Permissions.ManageEvents
        | Permissions.ManageThreads
        | Permissions.CreatePublicThreads
        | Permissions.CreatePrivateThreads
        | Permissions.UseExternalStickers
        | Permissions.SendMessagesInThreads
        | Permissions.StartActivities
        | Permissions.ModerateMembers);
}