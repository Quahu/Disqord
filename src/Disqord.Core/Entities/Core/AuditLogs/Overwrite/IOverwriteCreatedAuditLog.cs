namespace Disqord.AuditLogs;

public interface IOverwriteCreatedAuditLog : IOverwriteAuditLog, IDataAuditLog<IOverwriteAuditLogData>
{ }
