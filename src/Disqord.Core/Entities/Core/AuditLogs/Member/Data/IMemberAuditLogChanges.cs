using System;

namespace Disqord.AuditLogs;

public interface IMemberAuditLogChanges
{
    AuditLogChange<string?> Nick { get; }

    AuditLogChange<bool> IsMuted { get; }

    AuditLogChange<bool> IsDeafened { get; }

    AuditLogChange<DateTimeOffset> TimedOutUntil { get; }
}
