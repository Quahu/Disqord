namespace Disqord.AuditLogs;

public interface IStageAuditLogChanges
{
    AuditLogChange<string> Topic { get; }

    AuditLogChange<PrivacyLevel> PrivacyLevel { get; }
}