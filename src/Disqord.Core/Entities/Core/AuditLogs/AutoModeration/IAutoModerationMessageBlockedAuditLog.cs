namespace Disqord.AuditLogs
{
    public interface IAutoModerationMessageBlockedAuditLog : IAuditLog
    {
        string RuleName { get; }

        AutoModerationRuleTriggerType RuleTriggerType { get; }

        Snowflake ChannelId { get; }
    }
}
