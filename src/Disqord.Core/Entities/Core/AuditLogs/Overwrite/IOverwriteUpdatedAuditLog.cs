namespace Disqord.AuditLogs;

public interface IOverwriteUpdatedAuditLog : IOverwriteAuditLog, IChangesAuditLog<IOverwriteAuditLogChanges>
{ }
