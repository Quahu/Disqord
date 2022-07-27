using System;

namespace Disqord.Gateway;

/// <summary>
///     Represents Discord gateway intents.
/// </summary>
/// <seealso href="https://discord.com/developers/docs/topics/gateway#gateway-intents"> Discord documentation </seealso>
[Flags]
public enum GatewayIntents : ulong
{
    /// <summary>
    ///     No intent specified.
    /// </summary>
    None = 0,

    /// <summary>
    ///     Allows receiving guild-related events.
    /// </summary>
    Guilds = 1 << 0,

    /// <summary>
    ///     Allows receiving member-related events.
    /// </summary>
    /// <remarks>
    ///     This is a privileged intent.
    /// </remarks>
    Members = 1 << 1,

    /// <summary>
    ///     Allows receiving ban-related events.
    /// </summary>
    Bans = 1 << 2,

    /// <summary>
    ///     Allows receiving emoji and sticker related events.
    /// </summary>
    EmojisAndStickers = 1 << 3,

    /// <summary>
    ///     Allows receiving integration-related events.
    /// </summary>
    Integrations = 1 << 4,

    /// <summary>
    ///     Allows receiving webhook-related events.
    /// </summary>
    Webhooks = 1 << 5,

    /// <summary>
    ///     Allows receiving invite-related events.
    /// </summary>
    Invites = 1 << 6,

    /// <summary>
    ///     Allows receiving voice-state-related events.
    /// </summary>
    VoiceStates = 1 << 7,

    /// <summary>
    ///     Allows receiving presence-related events.
    /// </summary>
    /// <remarks>
    ///     This is a privileged intent.
    /// </remarks>
    Presences = 1 << 8,

    /// <summary>
    ///     Allows receiving guild message events.
    /// </summary>
    GuildMessages = 1 << 9,

    /// <summary>
    ///     Allows receiving guild reaction events.
    /// </summary>
    GuildReactions = 1 << 10,

    /// <summary>
    ///     Allows receiving guild typing events.
    /// </summary>
    GuildTyping = 1 << 11,

    /// <summary>
    ///     Allows receiving message events for direct channels.
    /// </summary>
    DirectMessages = 1 << 12,

    /// <summary>
    ///     Allows receiving reaction events for direct channels.
    /// </summary>
    DirectReactions = 1 << 13,

    /// <summary>
    ///     Allows receiving typing events for direct channels.
    /// </summary>
    DirectTyping = 1 << 14,

    /// <summary>
    ///     Allows receiving non-empty message content data in messages,
    ///     such as <see cref="IMessage.Content"/>, <see cref="IUserMessage.Attachments"/>,
    ///     <see cref="IUserMessage.Embeds"/>, and <see cref="IUserMessage.Components"/>.
    /// </summary>
    /// <remarks>
    ///     This is a privileged intent.
    /// </remarks>
    MessageContent = 1 << 15,

    /// <summary>
    ///     Allows receiving events related to guild events.
    /// </summary>
    GuildEvents = 1 << 16,

    /// <summary>
    ///     Allows receiving events related to auto-moderation configuration.
    /// </summary>
    AutoModerationConfiguration = 1 << 20,

    /// <summary>
    ///     Allows receiving events related to auto-moderation execution.
    /// </summary>
    AutoModerationExecution = 1 << 21,

    /// <summary>
    ///     Represents all unprivileged intents,
    ///     i.e. intents that never require bot verification.
    /// </summary>
    Unprivileged = Guilds | Bans | EmojisAndStickers | Integrations
        | Webhooks | Invites | VoiceStates | GuildMessages
        | GuildReactions | GuildTyping | DirectMessages | DirectReactions
        | DirectTyping | GuildEvents | AutoModerationConfiguration | AutoModerationExecution,

    /// <summary>
    ///     Represents all intents recommended by the library.
    /// </summary>
    /// <remarks>
    ///     Includes <see cref="Members"/> and <see cref="MessageContent"/>
    ///     which are privileged intents.
    /// </remarks>
    LibraryRecommended = Guilds | Members | Bans | EmojisAndStickers
        | Integrations | Webhooks | Invites | VoiceStates
        | GuildMessages | GuildReactions | MessageContent | GuildEvents
        | AutoModerationConfiguration | AutoModerationExecution,

    /// <summary>
    ///     Represents all intents.
    /// </summary>
    All = Guilds | Members | Bans | EmojisAndStickers
        | Integrations | Webhooks | Invites | VoiceStates
        | Presences | GuildMessages | GuildReactions | GuildTyping
        | DirectMessages | DirectReactions | DirectTyping | MessageContent
        | GuildEvents | AutoModerationConfiguration | AutoModerationExecution,
}
