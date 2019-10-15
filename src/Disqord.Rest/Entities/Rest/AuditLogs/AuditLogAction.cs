namespace Disqord.Rest.AuditLogs
{
    internal enum AuditLogAction
    {
        GuildUpdated = 1,

        ChannelCreated = 10,
        ChannelUpdated = 11,
        ChannelDeleted = 12,

        OverwriteCreated = 13,
        OverwriteUpdated = 14,
        OverwriteDeleted = 15,

        MemberKicked = 20,
        MembersPruned = 21,
        MemberBanned = 22,
        MemberUnbanned = 23,
        MemberUpdated = 24,
        MemberRolesUpdated = 25,

        RoleCreated = 30,
        RoleUpdated = 31,
        RoleDeleted = 32,

        InviteCreated = 40,
        InviteUpdated = 41,
        InviteDeleted = 42,

        WebhookCreated = 50,
        WebhookUpdated = 51,
        WebhookDeleted = 52,

        GuildEmojiCreated = 60,
        GuildEmojiUpdated = 61,
        GuildEmojiDeleted = 62,

        MessagesDeleted = 72
    }
}