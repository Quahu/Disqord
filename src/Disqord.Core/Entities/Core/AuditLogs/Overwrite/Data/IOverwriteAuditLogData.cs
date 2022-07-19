using Qommon;

namespace Disqord.AuditLogs;

public interface IOverwriteAuditLogData
{
    Optional<Snowflake> TargetId { get; }

    Optional<OverwriteTargetType> TargetType { get; }

    Optional<Permissions> Allowed { get; }

    Optional<Permissions> Denied { get; }
}
