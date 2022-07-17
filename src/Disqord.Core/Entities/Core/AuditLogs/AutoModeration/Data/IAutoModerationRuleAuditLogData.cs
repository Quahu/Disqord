using System.Collections.Generic;
using Qommon;

namespace Disqord.AuditLogs;

public interface IAutoModerationRuleAuditLogData
{
    Optional<string> Name { get; }

    Optional<AutoModerationEventType> EventType { get; }

    Optional<AutoModerationRuleTrigger> Trigger { get; }

    Optional<IAutoModerationTriggerMetadata> TriggerMetadata { get; }

    Optional<IReadOnlyList<IAutoModerationAction>> Actions { get; }

    Optional<bool> IsEnabled { get; }

    Optional<IReadOnlyList<Snowflake>> ExemptRoleIds { get; }

    Optional<IReadOnlyList<Snowflake>> ExemptChannelIds { get; }
}
