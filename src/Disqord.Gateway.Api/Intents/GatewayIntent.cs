using System;

namespace Disqord.Gateway
{
    /// <summary>
    ///     Represents one or more gateway intent flags.
    /// </summary>
    [Flags]
    public enum GatewayIntent : ulong
    {
        /// <summary>
        ///     No intent specified.
        /// </summary>
        None = 0,

        /// <summary>
        ///     The guilds intent. Required to receive guild-related events.
        /// </summary>
        Guilds = 1 << 0,

        /// <summary>
        ///     The members intent. Required to receive member-related events.
        /// </summary>
        Members = 1 << 1,

        /// <summary>
        ///     The bans intent. Required to receive ban-related events.
        /// </summary>
        Bans = 1 << 2,

        /// <summary>
        ///     The emojis and stickers intent. Required to receive emoji and sticker related events.
        /// </summary>
        EmojisAndStickers = 1 << 3,

        /// <summary>
        ///     The integrations intent. Required to receive integration-related events.
        /// </summary>
        Integrations = 1 << 4,

        /// <summary>
        ///     The webhooks intent. Required to receive webhook-related events.
        /// </summary>
        Webhooks = 1 << 5,

        /// <summary>
        ///     The invites intent. Required to receive invite-related events.
        /// </summary>
        Invites = 1 << 6,

        /// <summary>
        ///     The voice states intent. Required to receive voice-state-related events.
        /// </summary>
        VoiceStates = 1 << 7,

        /// <summary>
        ///     The presences intent. Required to receive presence-related events.
        /// </summary>
        Presences = 1 << 8,

        /// <summary>
        ///     The guild messages intent. Required to receive guild message events.
        /// </summary>
        GuildMessages = 1 << 9,

        /// <summary>
        ///     The guild reactions intent. Required to receive guild reaction events.
        /// </summary>
        GuildReactions = 1 << 10,

        /// <summary>
        ///     The guild typing intent. Required to receive guild typing events.
        /// </summary>
        GuildTyping = 1 << 11,

        /// <summary>
        ///     The direct messages intent. Required to receive message events for direct channels.
        /// </summary>
        DirectMessages = 1 << 12,

        /// <summary>
        ///     The direct reactions intent. Required to receive reaction events for direct channels.
        /// </summary>
        DirectReactions = 1 << 13,

        /// <summary>
        ///     The direct typing intent. Required to receive typing events for direct channels.
        /// </summary>
        DirectTyping = 1 << 14,

        /// <summary>
        ///     The guild events intent. Required to receive events related to guild events.
        /// </summary>
        GuildEvents = 1 << 16
    }
}
