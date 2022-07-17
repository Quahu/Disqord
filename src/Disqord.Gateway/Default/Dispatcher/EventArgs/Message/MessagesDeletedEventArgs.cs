using System;
using System.Collections.Generic;

namespace Disqord.Gateway;

public class MessagesDeletedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild in which the delete occurred.
    /// </summary>
    public Snowflake GuildId { get; }

    /// <summary>
    ///     Gets the ID of the channel in which the delete occurred.
    /// </summary>
    public Snowflake ChannelId { get; }

    /// <summary>
    ///     Gets the IDs of the messages that were deleted.
    /// </summary>
    public IReadOnlyList<Snowflake> MessageIds { get; }

    /// <summary>
    ///     Gets the messages in the state before the delete occurred.
    ///     If any of the messages deleted were not cached, they will
    ///     not be present in the dictionary.
    /// </summary>
    public IReadOnlyDictionary<Snowflake, CachedUserMessage> Messages { get; }

    public MessagesDeletedEventArgs(
        Snowflake guildId,
        Snowflake channelId,
        IReadOnlyList<Snowflake> messageIds,
        IReadOnlyDictionary<Snowflake, CachedUserMessage> messages)
    {
        GuildId = guildId;
        ChannelId = channelId;
        MessageIds = messageIds;
        Messages = messages;
    }
}