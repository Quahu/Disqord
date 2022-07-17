using System;

namespace Disqord.AuditLogs;

public interface IInviteAuditLogChanges
{
    AuditLogChange<string> Code { get; }

    AuditLogChange<Snowflake> ChannelId { get; }

    AuditLogChange<Snowflake> InviterId { get; }

    AuditLogChange<int> MaxUses { get; }

    AuditLogChange<int> Uses { get; }

    AuditLogChange<bool> IsTemporary { get; }

    AuditLogChange<TimeSpan> MaxAge { get; }
}
