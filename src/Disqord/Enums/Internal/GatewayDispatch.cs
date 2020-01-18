using System.Runtime.Serialization;
using Disqord.Serialization.Json;

namespace Disqord
{
    /// <summary>
    ///     Represents a Discord gateway dispatch.
    /// </summary>
    [StringEnum]
    internal enum GatewayDispatch : byte
    {
        [EnumMember(Value = "READY")]
        Ready,

        [EnumMember(Value = "RESUMED")]
        Resumed,

        [EnumMember(Value = "INVALID_SESSION")]
        InvalidSession,

        [EnumMember(Value = "CHANNEL_CREATE")]
        ChannelCreate,

        [EnumMember(Value = "CHANNEL_UPDATE")]
        ChannelUpdate,

        [EnumMember(Value = "CHANNEL_DELETE")]
        ChannelDelete,

        [EnumMember(Value = "CHANNEL_PINS_UPDATE")]
        ChannelPinsUpdate,

        [EnumMember(Value = "GUILD_CREATE")]
        GuildCreate,

        [EnumMember(Value = "GUILD_UPDATE")]
        GuildUpdate,

        [EnumMember(Value = "GUILD_DELETE")]
        GuildDelete,

        [EnumMember(Value = "GUILD_BAN_ADD")]
        GuildBanAdd,

        [EnumMember(Value = "GUILD_BAN_REMOVE")]
        GuildBanRemove,

        [EnumMember(Value = "GUILD_EMOJIS_UPDATE")]
        GuildEmojisUpdate,

        [EnumMember(Value = "GUILD_INTEGRATIONS_UPDATE")]
        GuildIntegrationsUpdate,

        [EnumMember(Value = "GUILD_MEMBER_ADD")]
        GuildMemberAdd,

        [EnumMember(Value = "GUILD_MEMBER_REMOVE")]
        GuildMemberRemove,

        [EnumMember(Value = "GUILD_MEMBER_UPDATE")]
        GuildMemberUpdate,

        [EnumMember(Value = "GUILD_MEMBERS_CHUNK")]
        GuildMembersChunk,

        [EnumMember(Value = "GUILD_ROLE_CREATE")]
        GuildRoleCreate,

        [EnumMember(Value = "GUILD_ROLE_UPDATE")]
        GuildRoleUpdate,

        [EnumMember(Value = "GUILD_ROLE_DELETE")]
        GuildRoleDelete,

        [EnumMember(Value = "INVITE_CREATE")]
        InviteCreate,

        [EnumMember(Value = "INVITE_DELETE")]
        InviteDelete,

        [EnumMember(Value = "GUILD_SYNC")]
        GuildSync,

        [EnumMember(Value = "MESSAGE_ACK")]
        MessageAck,

        [EnumMember(Value = "MESSAGE_CREATE")]
        MessageCreate,

        [EnumMember(Value = "MESSAGE_UPDATE")]
        MessageUpdate,

        [EnumMember(Value = "MESSAGE_DELETE")]
        MessageDelete,

        [EnumMember(Value = "MESSAGE_DELETE_BULK")]
        MessageDeleteBulk,

        [EnumMember(Value = "MESSAGE_REACTION_ADD")]
        MessageReactionAdd,

        [EnumMember(Value = "MESSAGE_REACTION_REMOVE")]
        MessageReactionRemove,

        [EnumMember(Value = "MESSAGE_REACTION_REMOVE_ALL")]
        MessageReactionRemoveAll,

        [EnumMember(Value = "MESSAGE_REACTION_REMOVE_EMOJI")]
        MessageReactionRemoveEmoji,

        [EnumMember(Value = "PRESENCE_UPDATE")]
        PresenceUpdate,

        [EnumMember(Value = "PRESENCES_REPLACE")]
        PresencesReplace,

        [EnumMember(Value = "RELATIONSHIP_ADD")]
        RelationshipAdd,

        [EnumMember(Value = "RELATIONSHIP_REMOVE")]
        RelationshipRemove,

        [EnumMember(Value = "TYPING_START")]
        TypingStart,

        [EnumMember(Value = "USER_NOTE_UPDATE")]
        UserNoteUpdate,

        [EnumMember(Value = "USER_UPDATE")]
        UserUpdate,

        [EnumMember(Value = "VOICE_STATE_UPDATE")]
        VoiceStateUpdate,

        [EnumMember(Value = "VOICE_SERVER_UPDATE")]
        VoiceServerUpdate,

        [EnumMember(Value = "WEBHOOKS_UPDATE")]
        WebhooksUpdate
    }
}
