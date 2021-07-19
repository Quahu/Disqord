namespace Disqord.AuditLogs
{
    public interface IStageAuditLogData
    {
        Optional<string> Topic { get; }

        Optional<StagePrivacyLevel> PrivacyLevel { get; }
    }
}