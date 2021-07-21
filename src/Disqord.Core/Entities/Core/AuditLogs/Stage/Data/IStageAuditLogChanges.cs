namespace Disqord.AuditLogs
{
    public interface IStageAuditLogChanges
    {
        AuditLogChange<string> Topic { get; }

        AuditLogChange<StagePrivacyLevel> PrivacyLevel { get; }
    }
}