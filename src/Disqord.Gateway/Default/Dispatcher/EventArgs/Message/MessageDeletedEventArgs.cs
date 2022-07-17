using System;

namespace Disqord.Gateway;

public class MessageDeletedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild in which the delete occurred.
    ///     Returns <see langword="null"/> if the message was deleted in a private channel.
    /// </summary>
    public Snowflake? GuildId { get; }

    /// <summary>
    ///     Gets the ID of the channel in which the delete occurred.
    /// </summary>
    public Snowflake ChannelId { get; }

    /// <summary>
    ///     Gets the ID of the message that was deleted.
    /// </summary>
    public Snowflake MessageId { get; }

    /// <summary>
    ///     Gets the message in the state before the delete occurred.
    ///     Returns <see langword="null"/> if the message was not cached.
    /// </summary>
    public CachedUserMessage? Message { get; }

    public MessageDeletedEventArgs(
        Snowflake? guildId,
        Snowflake channelId,
        Snowflake messageId,
        CachedUserMessage? message)
    {
        GuildId = guildId;
        ChannelId = channelId;
        MessageId = messageId;
        Message = message;
    }
}
