namespace Disqord.AuditLogs;

public interface IMembersMovedAuditLog : IAuditLog
{
    /// <summary>
    ///     Gets the ID of the channel to which the members were moved.
    /// </summary>
    Snowflake ChannelId { get; }

    /// <summary>
    ///     Gets the amount of members which were moved.
    /// </summary>
    int Count { get; }
}