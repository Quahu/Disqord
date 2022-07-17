using System.Collections.Generic;

namespace Disqord.AuditLogs;

public interface IAutoModerationRuleAuditLogChanges
{
    AuditLogChange<string> Name { get; }

    AuditLogChange<AutoModerationEventType> EventType { get; }

    AuditLogChange<AutoModerationRuleTrigger> Trigger { get; }

    AuditLogChange<IAutoModerationTriggerMetadata> TriggerMetadata { get; }

    AuditLogChange<IReadOnlyList<IAutoModerationAction>> Actions { get; }

    AuditLogChange<bool> IsEnabled { get; }

    AuditLogChange<IReadOnlyList<Snowflake>> ExemptRoleIds { get; }

    AuditLogChange<IReadOnlyList<Snowflake>> ExemptChannelIds { get; }
}
