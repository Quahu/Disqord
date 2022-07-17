namespace Disqord.AuditLogs;

public interface IMessageUnpinnedAuditLog : IAuditLog
{
    /// <summary>
    ///     Gets the ID of the channel in which the message was unpinned.
    /// </summary>
    Snowflake ChannelId { get; }

    /// <summary>
    ///     Gets the ID of the message of the message which was unpinned.
    /// </summary>
    Snowflake MessageId { get; }
}