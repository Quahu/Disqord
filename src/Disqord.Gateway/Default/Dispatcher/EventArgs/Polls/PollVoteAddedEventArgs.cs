using System;

namespace Disqord.Gateway;

public class PollVoteAddedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild in which the poll vote was added.
    ///     Returns <see langword="null"/> if it was added in a private channel.
    /// </summary>
    public Snowflake? GuildId { get; }

    /// <summary>
    ///     Gets the ID of the user who added the poll vote.
    /// </summary>
    public Snowflake UserId { get; }

    /// <summary>
    ///     Gets the ID of the channel in which the poll vote was added.
    /// </summary>
    public Snowflake ChannelId { get; }

    /// <summary>
    ///     Gets the ID of the message from which the poll vote was added.
    /// </summary>
    public Snowflake MessageId { get; }

    /// <summary>
    ///     Gets the ID of the poll answer from which the vote was added.
    /// </summary>
    public int AnswerId { get; }

    public PollVoteAddedEventArgs(
        Snowflake? guildId,
        Snowflake userId,
        Snowflake channelId,
        Snowflake messageId,
        int answerId)
    {
        GuildId = guildId;
        UserId = userId;
        ChannelId = channelId;
        MessageId = messageId;
        AnswerId = answerId;
    }
}
