namespace Disqord.AuditLogs;

public interface IRoleAuditLogChanges
{
    AuditLogChange<string> Name { get; }

    AuditLogChange<Permissions> Permissions { get; }

    AuditLogChange<Color?> Color { get; }

    AuditLogChange<bool> IsHoisted { get; }

    AuditLogChange<string> IconHash { get; }

    AuditLogChange<bool> IsMentionable { get; }

    AuditLogChange<string> UnicodeEmoji { get; }
}
