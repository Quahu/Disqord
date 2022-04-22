using Qommon;

namespace Disqord.AuditLogs
{
    public interface IInviteAuditLogData
    {
        Optional<string> Code { get; }

        Optional<Snowflake> ChannelId { get; }

        Optional<Snowflake> InviterId { get; }

        Optional<int> MaxUses { get; }

        Optional<int> Uses { get; }

        Optional<bool> IsTemporary { get; }
    }
}
