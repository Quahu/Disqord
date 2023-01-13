using Qommon;

namespace Disqord.AuditLogs;

public interface IOverwriteAuditLogData
{
    Optional<Permissions> Allowed { get; }

    Optional<Permissions> Denied { get; }
}
