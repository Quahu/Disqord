namespace Disqord.AuditLogs
{
    public interface IMemberAuditLogChanges
    {
        AuditLogChange<string> Nick { get; }

        AuditLogChange<bool> IsMuted { get; }

        AuditLogChange<bool> IsDeafened { get; }
    }
}
