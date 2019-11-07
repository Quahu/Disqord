namespace Disqord
{
    public readonly partial struct GuildPermissions
    {
        public const ulong ALL_PERMISSIONS_VALUE = (ulong) (
            Permission.CreateInstantInvite
            | Permission.KickMembers
            | Permission.BanMembers
            | Permission.Administrator
            | Permission.ManageChannels
            | Permission.ManageGuild
            | Permission.AddReactions
            | Permission.ViewAuditLog
            | Permission.PrioritySpeaker
            | Permission.Stream
            | Permission.ViewChannel
            | Permission.SendMessages
            | Permission.SendTtsMessages
            | Permission.ManageMessages
            | Permission.EmbedLinks
            | Permission.AttachFiles
            | Permission.ReadMessageHistory
            | Permission.MentionEveryone
            | Permission.UseExternalEmojis
            | Permission.ViewGuildInsights
            | Permission.Connect
            | Permission.Speak
            | Permission.MuteMembers
            | Permission.DeafenMembers
            | Permission.MoveMembers
            | Permission.UseVad
            | Permission.ChangeNickname
            | Permission.ManageNicknames
            | Permission.ManageRoles
            | Permission.ManageWebhooks
            | Permission.ManageEmojis);
    }
}
