using System;

namespace Disqord.Gateway;

public class ReactionRemovedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild in which the reaction was removed.
    ///     Returns <see langword="null"/> if it was removed in a private channel.
    /// </summary>
    public Snowflake? GuildId { get; }

    /// <summary>
    ///     Gets the ID of the user who removed the reaction.
    /// </summary>
    public Snowflake UserId { get; }

    /// <summary>
    ///     Gets the ID of the channel in which the reaction was removed.
    /// </summary>
    public Snowflake ChannelId { get; }

    /// <summary>
    ///     Gets the ID of the message to which the reaction was removed.
    /// </summary>
    public Snowflake MessageId { get; }

    /// <summary>
    ///     Gets the message from which the reaction was removed.
    ///     Returns <see langword="null"/> if the message was not cached.
    /// </summary>
    public CachedUserMessage? Message { get; }

    /// <summary>
    ///     Gets the emoji that was removed.
    /// </summary>
    public IEmoji Emoji { get; }

    public ReactionRemovedEventArgs(
        Snowflake? guildId,
        Snowflake userId,
        Snowflake channelId,
        Snowflake messageId,
        CachedUserMessage? message,
        IEmoji emoji)
    {
        GuildId = guildId;
        UserId = userId;
        ChannelId = channelId;
        MessageId = messageId;
        Message = message;
        Emoji = emoji;
    }
}
