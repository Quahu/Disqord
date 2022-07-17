namespace Disqord.AuditLogs;

public interface IMembersPrunedAuditLog : IAuditLog
{
    /// <summary>
    ///     Gets the number of days after which inactive members were pruned.
    /// </summary>
    int Days { get; }

    /// <summary>
    ///     Gets the amount of inactive members which were pruned.
    /// </summary>
    int Count { get; }
}