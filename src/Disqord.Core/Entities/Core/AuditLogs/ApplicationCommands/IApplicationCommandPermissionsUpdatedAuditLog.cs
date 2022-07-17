namespace Disqord.AuditLogs;

public interface IApplicationCommandPermissionsUpdatedAuditLog : IChangesAuditLog<IApplicationCommandPermissionAuditLogChanges>, ITargetedAuditLog<IApplicationCommand>
{
    Snowflake ApplicationId { get; }
}