namespace Disqord.AuditLogs
{
    public sealed class IRoleAuditLogData
    {
        Optional<string> Name { get; }

        Optional<GuildPermissions> Permissions { get; }

        Optional<Color?> Color { get; }

        Optional<bool> IsHoisted { get; }

        Optional<bool> IsMentionable { get; }
    }
}
