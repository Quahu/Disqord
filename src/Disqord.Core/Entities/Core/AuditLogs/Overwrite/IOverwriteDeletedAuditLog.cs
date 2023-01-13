namespace Disqord.AuditLogs;

public interface IOverwriteDeletedAuditLog : IOverwriteAuditLog, IDataAuditLog<IOverwriteAuditLogData>
{ }
