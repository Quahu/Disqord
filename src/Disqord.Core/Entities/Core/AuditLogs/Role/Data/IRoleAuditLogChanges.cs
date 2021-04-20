namespace Disqord.AuditLogs
{
    public interface IRoleAuditLogChanges
    {
        AuditLogChange<string> Name { get; }

        AuditLogChange<GuildPermissions> Permissions { get; }

        AuditLogChange<Color?> Color { get; }

        AuditLogChange<bool> IsHoisted { get; }

        AuditLogChange<bool> IsMentionable { get; }
    }
}
