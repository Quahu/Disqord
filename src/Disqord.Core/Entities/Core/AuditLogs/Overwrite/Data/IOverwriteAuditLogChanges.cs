namespace Disqord.AuditLogs;

public interface IOverwriteAuditLogChanges
{
    AuditLogChange<Permissions> Allowed { get; }

    AuditLogChange<Permissions> Denied { get; }
}
