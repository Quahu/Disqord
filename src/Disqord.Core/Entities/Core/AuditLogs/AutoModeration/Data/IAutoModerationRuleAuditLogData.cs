using System.Collections.Generic;
using Qommon;

namespace Disqord.AuditLogs
{
    public interface IAutoModerationRuleAuditLogData
    {
        Optional<string> Name { get; }

        Optional<AutoModerationEventType> EventType { get; }

        Optional<AutoModerationRuleTriggerType> TriggerType { get; }

        Optional<IAutoModerationTriggerMetadata> TriggerMetadata { get; }

        Optional<IReadOnlyList<IAutoModerationAction>> Actions { get; }

        Optional<bool> IsEnabled { get; }

        Optional<IReadOnlyList<Snowflake>> ExemptRoles { get; }

        Optional<IReadOnlyList<Snowflake>> ExemptChannels { get; }
    }
}
