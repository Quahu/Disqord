using System;

namespace Disqord.Gateway;

public class PollVoteRemovedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild in which the poll vote was removed.
    ///     Returns <see langword="null"/> if it was removed in a private channel.
    /// </summary>
    public Snowflake? GuildId { get; }

    /// <summary>
    ///     Gets the ID of the user who removed the poll vote.
    /// </summary>
    public Snowflake UserId { get; }

    /// <summary>
    ///     Gets the ID of the channel in which the poll vote was removed.
    /// </summary>
    public Snowflake ChannelId { get; }

    /// <summary>
    ///     Gets the ID of the message from which the poll vote was removed.
    /// </summary>
    public Snowflake MessageId { get; }

    /// <summary>
    ///     Gets the ID of the poll answer from which the vote was removed.
    /// </summary>
    public int AnswerId { get; }

    public PollVoteRemovedEventArgs(
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
