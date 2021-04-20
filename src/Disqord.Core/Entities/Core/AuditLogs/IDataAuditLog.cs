namespace Disqord.AuditLogs
{
    public interface IDataAuditLog<T> : IAuditLog
    {
        T Data { get; }
    }
}
