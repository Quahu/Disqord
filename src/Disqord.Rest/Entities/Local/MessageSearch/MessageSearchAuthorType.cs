namespace Disqord;

/// <summary>
///     Represents an author type filter for message search.
/// </summary>
public enum MessageSearchAuthorType
{
    /// <summary>
    ///     The message author is a user.
    /// </summary>
    User,

    /// <summary>
    ///     The message author is a bot.
    /// </summary>
    Bot,

    /// <summary>
    ///     The message author is a webhook.
    /// </summary>
    Webhook,
}
