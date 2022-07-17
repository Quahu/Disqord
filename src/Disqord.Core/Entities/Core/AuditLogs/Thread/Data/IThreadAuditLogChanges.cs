using System;

namespace Disqord.AuditLogs;

public interface IThreadAuditLogChanges
{
    AuditLogChange<string> Name { get; }

    AuditLogChange<bool> IsArchived { get; }

    AuditLogChange<bool> IsLocked { get; }

    AuditLogChange<TimeSpan> AutomaticArchiveDuration { get; }

    AuditLogChange<TimeSpan> Slowmode { get; }

    AuditLogChange<ChannelType> Type { get; }
}