namespace Disqord.AuditLogs;

public interface IMessagePinnedAuditLog : IAuditLog
{
    /// <summary>
    ///     Gets the ID of the channel in which the message was pinned.
    /// </summary>
    Snowflake ChannelId { get; }

    /// <summary>
    ///     Gets the ID of the message of the message which was pinned.
    /// </summary>
    Snowflake MessageId { get; }
}