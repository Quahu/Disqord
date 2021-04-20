namespace Disqord.AuditLogs
{
    public interface IMembersPrunedAuditLog : IAuditLog
    {
        int Days { get; }

        int Count { get; }
    }
}
