namespace Disqord.AuditLogs;

public interface IAutoModerationMessageBlockedAuditLog : IAuditLog
{
    string RuleName { get; }

    AutoModerationRuleTrigger RuleTrigger { get; }

    Snowflake ChannelId { get; }
}
