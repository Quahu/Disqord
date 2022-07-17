namespace Disqord.AuditLogs;

public interface IGuildEventCreatedAuditLog : IDataAuditLog<IGuildEventAuditLogData>, ITargetedAuditLog<IGuildEvent>
{ }