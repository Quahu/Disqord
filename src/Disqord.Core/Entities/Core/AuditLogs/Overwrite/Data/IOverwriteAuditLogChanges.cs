namespace Disqord.AuditLogs;

public interface IOverwriteAuditLogChanges
{
    AuditLogChange<Snowflake> TargetId { get; }

    AuditLogChange<OverwriteTargetType> TargetType { get; }

    AuditLogChange<Permissions> Allowed { get; }

    AuditLogChange<Permissions> Denied { get; }
}
