using System.Collections.Generic;
using Disqord.Models;
using Microsoft.Extensions.Logging;
using Qommon.Collections.ReadOnly;

namespace Disqord.AuditLogs
{
    public class TransientAutoModerationRuleAuditLogChanges : IAutoModerationRuleAuditLogChanges
    {
        public AuditLogChange<string> Name { get; }

        public AuditLogChange<AutoModerationEventType> EventType { get; }

        public AuditLogChange<AutoModerationRuleTriggerType> TriggerType { get; }

        public AuditLogChange<IAutoModerationTriggerMetadata> TriggerMetadata { get; }

        public AuditLogChange<IReadOnlyList<IAutoModerationAction>> Actions { get; }

        public AuditLogChange<bool> IsEnabled { get; }

        public AuditLogChange<IReadOnlyList<Snowflake>> ExemptRoleIds { get; }

        public AuditLogChange<IReadOnlyList<Snowflake>> ExemptChannelIds { get; }

        public TransientAutoModerationRuleAuditLogChanges(IClient client, AuditLogEntryJsonModel model)
        {
            for (var i = 0; i < model.Changes.Value.Length; i++)
            {
                var change = model.Changes.Value[i];
                switch (change.Key)
                {
                    case "name":
                    {
                        Name = AuditLogChange<string>.Convert(change);
                        break;
                    }
                    case "event_type":
                    {
                        EventType = AuditLogChange<AutoModerationEventType>.Convert(change);
                        break;
                    }
                    case "trigger_type":
                    {
                        TriggerType = AuditLogChange<AutoModerationRuleTriggerType>.Convert(change);
                        break;
                    }
                    case "trigger_metadata":
                    {
                        TriggerMetadata = AuditLogChange<IAutoModerationTriggerMetadata>.Convert<AutoModerationTriggerMetadataJsonModel>(change, x => new TransientAutoModerationTriggerMetadata(x));
                        break;
                    }
                    case "actions":
                    {
                        Actions = AuditLogChange<IReadOnlyList<IAutoModerationAction>>.Convert<AutoModerationActionJsonModel[]>(change, x => x.ToReadOnlyList(y => new TransientAutoModerationAction(y)));
                        break;
                    }
                    case "enabled":
                    {
                        IsEnabled = AuditLogChange<bool>.Convert(change);
                        break;
                    }
                    case "exempt_roles":
                    {
                        ExemptRoleIds = AuditLogChange<IReadOnlyList<Snowflake>>.Convert<Snowflake[]>(change, x => x.ToReadOnlyList());
                        break;
                    }
                    case "exempt_channels":
                    {
                        ExemptChannelIds = AuditLogChange<IReadOnlyList<Snowflake>>.Convert<Snowflake[]>(change, x => x.ToReadOnlyList());
                        break;
                    }
                    default:
                    {
                        client.Logger.LogDebug("Unknown key {0} for {1}", change.Key, this);
                        break;
                    }
                }
            }
        }
    }
}
