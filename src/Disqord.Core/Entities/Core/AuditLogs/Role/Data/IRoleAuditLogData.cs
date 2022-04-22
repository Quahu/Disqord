using Qommon;

namespace Disqord.AuditLogs
{
    public interface IRoleAuditLogData
    {
        Optional<string> Name { get; }

        Optional<GuildPermissions> Permissions { get; }

        Optional<Color?> Color { get; }

        Optional<bool> IsHoisted { get; }

        Optional<string> IconHash { get; }

        Optional<bool> IsMentionable { get; }

        Optional<string> UnicodeEmoji { get; }
    }
}
