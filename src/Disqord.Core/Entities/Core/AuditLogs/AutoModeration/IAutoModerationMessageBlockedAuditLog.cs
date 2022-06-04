namespace Disqord.AuditLogs
{
    public interface IAutoModerationMessageBlockedAuditLog : IAuditLog
    {
        string RuleName { get; }

        AutoModerationTriggerType TriggerType { get; }

        Snowflake ChannelId { get; }
    }
}
