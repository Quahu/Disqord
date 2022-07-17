namespace Disqord.AuditLogs;

public interface IThreadUpdatedAuditLog : IChangesAuditLog<IThreadAuditLogChanges>, ITargetedAuditLog<IThreadChannel>
{ }