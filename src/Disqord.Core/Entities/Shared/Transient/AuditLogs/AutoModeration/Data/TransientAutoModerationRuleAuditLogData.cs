using System.Collections.Generic;
using Disqord.Models;
using Qommon;

namespace Disqord.AuditLogs
{
    public class TransientAutoModerationRuleAuditLogData : IAutoModerationRuleAuditLogData
    {
        public Optional<string> Name { get; }

        public Optional<AutoModerationEventType> EventType { get; }

        public Optional<AutoModerationRuleTriggerType> TriggerType { get; }

        public Optional<IAutoModerationTriggerMetadata> TriggerMetadata { get; }

        public Optional<IReadOnlyList<IAutoModerationAction>> Actions { get; }

        public Optional<bool> IsEnabled { get; }

        public Optional<IReadOnlyList<Snowflake>> ExemptRoles { get; }

        public Optional<IReadOnlyList<Snowflake>> ExemptChannels { get; }

        public TransientAutoModerationRuleAuditLogData(IClient client, AuditLogEntryJsonModel model, bool isCreated)
        {
            var changes = new TransientAutoModerationRuleAuditLogChanges(client, model);
            if (isCreated)
            {
                Name = changes.Name.NewValue;
                EventType = changes.EventType.NewValue;
                TriggerType = changes.TriggerType.NewValue;
                TriggerMetadata = changes.TriggerMetadata.NewValue;
                Actions = changes.Actions.NewValue;
                IsEnabled = changes.IsEnabled.NewValue;
                ExemptRoles = changes.ExemptRoles.NewValue;
                ExemptChannels = changes.ExemptChannels.NewValue;
            }
            else
            {
                Name = changes.Name.OldValue;
                EventType = changes.EventType.OldValue;
                TriggerType = changes.TriggerType.OldValue;
                TriggerMetadata = changes.TriggerMetadata.OldValue;
                Actions = changes.Actions.OldValue;
                IsEnabled = changes.IsEnabled.OldValue;
                ExemptRoles = changes.ExemptRoles.OldValue;
                ExemptChannels = changes.ExemptChannels.OldValue;
            }
        }
    }
}
