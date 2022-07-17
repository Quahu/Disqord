namespace Disqord.AuditLogs;

public interface IMemberUpdatedAuditLog : IChangesAuditLog<IMemberAuditLogChanges>, ITargetedAuditLog<IUser>
{ }