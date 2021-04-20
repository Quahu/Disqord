namespace Disqord.AuditLogs
{
    public interface IMembersDisconnectedAuditLog : IAuditLog
    {
        int Count { get; }
    }
}
