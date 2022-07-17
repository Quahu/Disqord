using Qommon;

namespace Disqord.AuditLogs;

public interface IStageAuditLogData
{
    Optional<string> Topic { get; }

    Optional<PrivacyLevel> PrivacyLevel { get; }
}