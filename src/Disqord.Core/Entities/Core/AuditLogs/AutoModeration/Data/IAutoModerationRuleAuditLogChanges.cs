using System.Collections.Generic;

namespace Disqord.AuditLogs
{
    public interface IAutoModerationRuleAuditLogChanges
    {
        AuditLogChange<string> Name { get; }

        AuditLogChange<AutoModerationEventType> EventType { get; }

        AuditLogChange<AutoModerationRuleTriggerType> TriggerType { get; }

        AuditLogChange<IAutoModerationTriggerMetadata> TriggerMetadata { get; }

        AuditLogChange<IReadOnlyList<IAutoModerationAction>> Actions { get; }

        AuditLogChange<bool> IsEnabled { get; }

        AuditLogChange<IReadOnlyList<Snowflake>> ExemptRoles { get; }

        AuditLogChange<IReadOnlyList<Snowflake>> ExemptChannels { get; }
    }
}
