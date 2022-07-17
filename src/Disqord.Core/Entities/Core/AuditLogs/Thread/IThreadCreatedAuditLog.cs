namespace Disqord.AuditLogs;

public interface IThreadCreatedAuditLog : IDataAuditLog<IThreadAuditLogData>, ITargetedAuditLog<IThreadChannel>
{ }