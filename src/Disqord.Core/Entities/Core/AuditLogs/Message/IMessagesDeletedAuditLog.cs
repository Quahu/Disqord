namespace Disqord.AuditLogs;

public interface IMessagesDeletedAuditLog : IAuditLog
{
    /// <summary>
    ///     Gets the ID of the channel in which the messages were deleted.
    /// </summary>
    Snowflake ChannelId { get; }

    /// <summary>
    ///     Gets the amount of the messages which were deleted.
    /// </summary>
    int Count { get; }
}