namespace Disqord.AuditLogs;

public interface IOverwriteAuditLogChanges
{
    AuditLogChange<Snowflake> TargetId { get; }

    AuditLogChange<OverwriteTargetType> TargetType { get; }

    AuditLogChange<ChannelPermissions> Allowed { get; }

    AuditLogChange<ChannelPermissions> Denied { get; }
}