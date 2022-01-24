namespace Disqord.Gateway
{
    /// <summary>
    ///     Defines the names of known gateway dispatches.
    /// </summary>
    public static class GatewayDispatchNames
    {
        /// <summary>
        /// 	The <c>READY</c> dispatch.
        /// </summary>
        public const string Ready = "READY";

        /// <summary>
        /// 	The <c>RESUMED</c> dispatch.
        /// </summary>
        public const string Resumed = "RESUMED";

        /// <summary>
        /// 	The <c>CHANNEL_CREATE</c> dispatch.
        /// </summary>
        public const string ChannelCreate = "CHANNEL_CREATE";

        /// <summary>
        /// 	The <c>CHANNEL_UPDATE</c> dispatch.
        /// </summary>
        public const string ChannelUpdate = "CHANNEL_UPDATE";

        /// <summary>
        /// 	The <c>CHANNEL_DELETE</c> dispatch.
        /// </summary>
        public const string ChannelDelete = "CHANNEL_DELETE";

        /// <summary>
        /// 	The <c>THREAD_CREATE</c> dispatch.
        /// </summary>
        public const string ThreadCreate = "THREAD_CREATE";

        /// <summary>
        /// 	The <c>THREAD_UPDATE</c> dispatch.
        /// </summary>
        public const string ThreadUpdate = "THREAD_UPDATE";

        /// <summary>
        /// 	The <c>THREAD_DELETE</c> dispatch.
        /// </summary>
        public const string ThreadDelete = "THREAD_DELETE";

        /// <summary>
        /// 	The <c>THREAD_LIST_SYNC</c> dispatch.
        /// </summary>
        public const string ThreadListSync = "THREAD_LIST_SYNC";

        /// <summary>
        /// 	The <c>THREAD_MEMBERS_UPDATE</c> dispatch.
        /// </summary>
        public const string ThreadMembersUpdate = "THREAD_MEMBERS_UPDATE";

        /// <summary>
        /// 	The <c>CHANNEL_PINS_UPDATE</c> dispatch.
        /// </summary>
        public const string ChannelPinsUpdate = "CHANNEL_PINS_UPDATE";

        /// <summary>
        /// 	The <c>GUILD_CREATE</c> dispatch.
        /// </summary>
        public const string GuildCreate = "GUILD_CREATE";

        /// <summary>
        /// 	The <c>GUILD_UPDATE</c> dispatch.
        /// </summary>
        public const string GuildUpdate = "GUILD_UPDATE";

        /// <summary>
        /// 	The <c>GUILD_DELETE</c> dispatch.
        /// </summary>
        public const string GuildDelete = "GUILD_DELETE";

        /// <summary>
        /// 	The <c>GUILD_BAN_ADD</c> dispatch.
        /// </summary>
        public const string GuildBanAdd = "GUILD_BAN_ADD";

        /// <summary>
        /// 	The <c>GUILD_BAN_REMOVE</c> dispatch.
        /// </summary>
        public const string GuildBanRemove = "GUILD_BAN_REMOVE";

        /// <summary>
        /// 	The <c>GUILD_EMOJIS_UPDATE</c> dispatch.
        /// </summary>
        public const string GuildEmojisUpdate = "GUILD_EMOJIS_UPDATE";

        /// <summary>
        /// 	The <c>GUILD_STICKERS_UPDATE</c> dispatch.
        /// </summary>
        public const string GuildStickersUpdate = "GUILD_STICKERS_UPDATE";

        /// <summary>
        /// 	The <c>GUILD_INTEGRATIONS_UPDATE</c> dispatch.
        /// </summary>
        public const string GuildIntegrationsUpdate = "GUILD_INTEGRATIONS_UPDATE";

        /// <summary>
        /// 	The <c>GUILD_MEMBER_ADD</c> dispatch.
        /// </summary>
        public const string GuildMemberAdd = "GUILD_MEMBER_ADD";

        /// <summary>
        /// 	The <c>GUILD_MEMBER_UPDATE</c> dispatch.
        /// </summary>
        public const string GuildMemberUpdate = "GUILD_MEMBER_UPDATE";

        /// <summary>
        /// 	The <c>GUILD_MEMBER_REMOVE</c> dispatch.
        /// </summary>
        public const string GuildMemberRemove = "GUILD_MEMBER_REMOVE";

        /// <summary>
        /// 	The <c>GUILD_MEMBERS_CHUNK</c> dispatch.
        /// </summary>
        public const string GuildMembersChunk = "GUILD_MEMBERS_CHUNK";

        /// <summary>
        /// 	The <c>GUILD_ROLE_CREATE</c> dispatch.
        /// </summary>
        public const string GuildRoleCreate = "GUILD_ROLE_CREATE";

        /// <summary>
        /// 	The <c>GUILD_ROLE_UPDATE</c> dispatch.
        /// </summary>
        public const string GuildRoleUpdate = "GUILD_ROLE_UPDATE";

        /// <summary>
        /// 	The <c>GUILD_ROLE_DELETE</c> dispatch.
        /// </summary>
        public const string GuildRoleDelete = "GUILD_ROLE_DELETE";

        /// <summary>
        /// 	The <c>GUILD_SCHEDULED_EVENT_CREATE</c> dispatch.
        /// </summary>
        public const string GuildScheduledEventCreate = "GUILD_SCHEDULED_EVENT_CREATE";
        
        /// <summary>
        /// 	The <c>GUILD_SCHEDULED_EVENT_UPDATE</c> dispatch.
        /// </summary>
        public const string GuildScheduledEventUpdate = "GUILD_SCHEDULED_EVENT_UPDATE";
        
        /// <summary>
        /// 	The <c>GUILD_SCHEDULED_EVENT_DELETE</c> dispatch.
        /// </summary>
        public const string GuildScheduledEventDelete = "GUILD_SCHEDULED_EVENT_DELETE";
        
        /// <summary>
        /// 	The <c>GUILD_SCHEDULED_EVENT_USER_ADD</c> dispatch.
        /// </summary>
        public const string GuildScheduledEventUserAdd = "GUILD_SCHEDULED_EVENT_USER_ADD";
        
        /// <summary>
        /// 	The <c>GUILD_SCHEDULED_EVENT_USER_REMOVE</c> dispatch.
        /// </summary>
        public const string GuildScheduledEventUserRemove = "GUILD_SCHEDULED_EVENT_USER_REMOVE";
        
        /// <summary>
        /// 	The <c>INTEGRATION_CREATE</c> dispatch.
        /// </summary>
        public const string IntegrationCreate = "INTEGRATION_CREATE";

        /// <summary>
        /// 	The <c>INTEGRATION_UPDATE</c> dispatch.
        /// </summary>
        public const string IntegrationUpdate = "INTEGRATION_UPDATE";

        /// <summary>
        /// 	The <c>INTEGRATION_DELETE</c> dispatch.
        /// </summary>
        public const string IntegrationDelete = "INTEGRATION_DELETE";

        /// <summary>
        /// 	The <c>INTERACTION_CREATE</c> dispatch.
        /// </summary>
        public const string InteractionCreate = "INTERACTION_CREATE";

        /// <summary>
        /// 	The <c>INVITE_CREATE</c> dispatch.
        /// </summary>
        public const string InviteCreate = "INVITE_CREATE";

        /// <summary>
        /// 	The <c>INVITE_DELETE</c> dispatch.
        /// </summary>
        public const string InviteDelete = "INVITE_DELETE";

        /// <summary>
        /// 	The <c>MESSAGE_CREATE</c> dispatch.
        /// </summary>
        public const string MessageCreate = "MESSAGE_CREATE";

        /// <summary>
        /// 	The <c>MESSAGE_UPDATE</c> dispatch.
        /// </summary>
        public const string MessageUpdate = "MESSAGE_UPDATE";

        /// <summary>
        /// 	The <c>MESSAGE_DELETE</c> dispatch.
        /// </summary>
        public const string MessageDelete = "MESSAGE_DELETE";

        /// <summary>
        /// 	The <c>MESSAGE_DELETE_BULK</c> dispatch.
        /// </summary>
        public const string MessageDeleteBulk = "MESSAGE_DELETE_BULK";

        /// <summary>
        /// 	The <c>MESSAGE_REACTION_ADD</c> dispatch.
        /// </summary>
        public const string MessageReactionAdd = "MESSAGE_REACTION_ADD";

        /// <summary>
        /// 	The <c>MESSAGE_REACTION_REMOVE</c> dispatch.
        /// </summary>
        public const string MessageReactionRemove = "MESSAGE_REACTION_REMOVE";

        /// <summary>
        /// 	The <c>MESSAGE_REACTION_REMOVE_ALL</c> dispatch.
        /// </summary>
        public const string MessageReactionRemoveAll = "MESSAGE_REACTION_REMOVE_ALL";

        /// <summary>
        /// 	The <c>MESSAGE_REACTION_REMOVE_EMOJI</c> dispatch.
        /// </summary>
        public const string MessageReactionRemoveEmoji = "MESSAGE_REACTION_REMOVE_EMOJI";

        /// <summary>
        /// 	The <c>PRESENCE_UPDATE</c> dispatch.
        /// </summary>
        public const string PresenceUpdate = "PRESENCE_UPDATE";

        /// <summary>
        /// 	The <c>STAGE_INSTANCE_CREATE</c> dispatch.
        /// </summary>
        public const string StageInstanceCreate = "STAGE_INSTANCE_CREATE";

        /// <summary>
        /// 	The <c>STAGE_INSTANCE_UPDATE</c> dispatch.
        /// </summary>
        public const string StageInstanceUpdate = "STAGE_INSTANCE_UPDATE";

        /// <summary>
        /// 	The <c>STAGE_INSTANCE_DELETE</c> dispatch.
        /// </summary>
        public const string StageInstanceDelete = "STAGE_INSTANCE_DELETE";

        /// <summary>
        /// 	The <c>TYPING_START</c> dispatch.
        /// </summary>
        public const string TypingStart = "TYPING_START";

        /// <summary>
        /// 	The <c>USER_UPDATE</c> dispatch.
        /// </summary>
        public const string UserUpdate = "USER_UPDATE";

        /// <summary>
        /// 	The <c>VOICE_STATE_UPDATE</c> dispatch.
        /// </summary>
        public const string VoiceStateUpdate = "VOICE_STATE_UPDATE";

        /// <summary>
        /// 	The <c>VOICE_SERVER_UPDATE</c> dispatch.
        /// </summary>
        public const string VoiceServerUpdate = "VOICE_SERVER_UPDATE";

        /// <summary>
        /// 	The <c>WEBHOOKS_UPDATE</c> dispatch.
        /// </summary>
        public const string WebhooksUpdate = "WEBHOOKS_UPDATE";
    }
}
