namespace Disqord;

/// <summary>
///     Represents the context in Discord where an interaction can or was used.
/// </summary>
public enum InteractionContextType
{
    /// <summary>
    ///     Interaction used in a guild.
    /// </summary>
    Guild = 0,

    /// <summary>
    ///     Interaction used in a direct channel with the bot.
    /// </summary>
    BotDirect = 1,

    /// <summary>
    ///     Interaction used in a direct or group channel other than a direct channel with the bot.
    /// </summary>
    PrivateChannel = 2,
}
