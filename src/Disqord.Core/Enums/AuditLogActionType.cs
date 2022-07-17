namespace Disqord;

public enum AuditLogActionType
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
    MembersMoved = 26,
    MembersDisconnected = 27,
    BotAdded = 28,

    RoleCreated = 30,
    RoleUpdated = 31,
    RoleDeleted = 32,

    InviteCreated = 40,
    InviteUpdated = 41,
    InviteDeleted = 42,

    WebhookCreated = 50,
    WebhookUpdated = 51,
    WebhookDeleted = 52,

    EmojiCreated = 60,
    EmojiUpdated = 61,
    EmojiDeleted = 62,

    MessagesDeleted = 72,
    MessagesBulkDeleted = 73,
    MessagePinned = 74,
    MessageUnpinned = 75,

    IntegrationCreated = 80,
    IntegrationUpdated = 81,
    IntegrationDeleted = 82,

    StageCreated = 83,
    StageUpdated = 84,
    StageDeleted = 85,

    StickerCreated = 90,
    StickerUpdated = 91,
    StickerDeleted = 92,

    GuildEventCreated = 100,
    GuildEventUpdated = 101,
    GuildEventDeleted = 102,

    ThreadCreate = 110,
    ThreadUpdate = 111,
    ThreadDelete = 112,

    ApplicationCommandPermissionsUpdate = 121,

    AutoModerationRuleCreated = 140,
    AutoModerationRuleUpdated = 141,
    AutoModerationRuleDeleted = 142,
    AutoModerationMessageBlocked = 143
}