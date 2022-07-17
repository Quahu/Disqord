using System;
using Qommon;

namespace Disqord.AuditLogs;

public interface IThreadAuditLogData
{
    Optional<string> Name { get; }

    Optional<bool> IsArchived { get; }

    Optional<bool> IsLocked { get; }

    Optional<TimeSpan> AutomaticArchiveDuration { get; }

    Optional<TimeSpan> Slowmode { get; }

    Optional<ChannelType> Type { get; }
}