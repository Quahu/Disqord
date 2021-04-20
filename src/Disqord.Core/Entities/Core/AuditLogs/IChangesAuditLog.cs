namespace Disqord.AuditLogs
{
    public interface IChangesAuditLog<T> : IAuditLog
    {
        T Changes { get; }
    }
}
