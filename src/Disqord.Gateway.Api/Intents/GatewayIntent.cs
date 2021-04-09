using System;

namespace Disqord.Gateway
{
    // TODO: xmldocs
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
        ///     The emojis intent. Required to receive receive emoji-related events.
        /// </summary>
        Emojis = 1 << 3,

        /// <summary>
        ///     The integrations intent. Required to receive integration-related events.
        /// </summary>
        Integrations = 1 << 4,

        /// <summary>
        ///     The integrations intent. Required to receive integration-related events.
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
        ///     The integrations intent. Required to receive integration-related events.
        /// </summary>
        GuildReactions = 1 << 10,

        /// <summary>
        ///     The integrations intent. Required to receive integration-related events.
        /// </summary>
        GuildTyping = 1 << 11,

        /// <summary>
        ///     The integrations intent. Required to receive integration-related events.
        /// </summary>
        DirectMessages = 1 << 12,

        /// <summary>
        ///     The integrations intent. Required to receive integration-related events.
        /// </summary>
        DirectReactions = 1 << 13,

        /// <summary>
        ///     The integrations intent. Required to receive integration-related events.
        /// </summary>
        DirectTyping = 1 << 14
    }
}
