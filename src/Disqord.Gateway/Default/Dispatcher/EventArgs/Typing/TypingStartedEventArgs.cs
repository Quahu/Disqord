using System;

namespace Disqord.Gateway;

public class TypingStartedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild in which the typing started.
    ///     Returns <see langword="null"/> if the typing started in a private channel.
    /// </summary>
    public Snowflake? GuildId { get; }

    /// <summary>
    ///     Gets the ID of the channel in which the typing started.
    /// </summary>
    public Snowflake ChannelId { get; }

    /// <summary>
    ///     Gets the ID of the user that started typing.
    /// </summary>
    public Snowflake UserId { get; }

    /// <summary>
    ///     Gets the date at which the user started typing.
    /// </summary>
    public DateTimeOffset StartedAt { get; }

    /// <summary>
    ///     Gets the cached member that started typing.
    ///     Returns <see langword="null"/> if the typing started in a private channel.
    /// </summary>
    public IMember? Member { get; }

    public TypingStartedEventArgs(
        Snowflake? guildId,
        Snowflake channelId,
        Snowflake userId,
        DateTimeOffset startedAt,
        IMember? member)
    {
        GuildId = guildId;
        ChannelId = channelId;
        UserId = userId;
        StartedAt = startedAt;
        Member = member;
    }
}
