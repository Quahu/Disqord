using System;

namespace Disqord.Gateway;

public class ReactionAddedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild in which the reaction was added.
    ///     Returns <see langword="null"/> if it was added in a private channel.
    /// </summary>
    public Snowflake? GuildId { get; }

    /// <summary>
    ///     Gets the ID of the user who added the reaction.
    /// </summary>
    public Snowflake UserId { get; }

    /// <summary>
    ///     Gets the ID of the channel in which the reaction was added.
    /// </summary>
    public Snowflake ChannelId { get; }

    /// <summary>
    ///     Gets the ID of the message to which the reaction was added.
    /// </summary>
    public Snowflake MessageId { get; }

    /// <summary>
    ///     Gets the message to which the reaction was added.
    ///     Returns <see langword="null"/> if the message was not cached.
    /// </summary>
    public CachedUserMessage? Message { get; }

    /// <summary>
    ///     Gets the member who added the reaction.
    ///     Returns <see langword="null"/> if it was added in a private channel.
    /// </summary>
    public IMember? Member { get; }

    /// <summary>
    ///     Gets the emoji that was added.
    /// </summary>
    public IEmoji Emoji { get; }

    public ReactionAddedEventArgs(
        Snowflake? guildId,
        Snowflake userId,
        Snowflake channelId,
        Snowflake messageId,
        CachedUserMessage? message,
        IMember? member,
        IEmoji emoji)
    {
        GuildId = guildId;
        UserId = userId;
        ChannelId = channelId;
        MessageId = messageId;
        Message = message;
        Member = member;
        Emoji = emoji;
    }
}
