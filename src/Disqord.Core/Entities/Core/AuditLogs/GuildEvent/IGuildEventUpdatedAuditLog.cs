namespace Disqord.AuditLogs
{
    public interface IGuildEventUpdatedAuditLog : IChangesAuditLog<IGuildEventAuditLogChanges>, ITargetedAuditLog<IGuildEvent>
    { }
}
