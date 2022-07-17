using System.Collections.Generic;
using Disqord.Models;
using Microsoft.Extensions.Logging;
using Qommon.Collections.ReadOnly;

namespace Disqord.AuditLogs;

public class TransientAutoModerationRuleAuditLogChanges : IAutoModerationRuleAuditLogChanges
{
    /// <inheritdoc/>
    public AuditLogChange<string> Name { get; }

    /// <inheritdoc/>
    public AuditLogChange<AutoModerationEventType> EventType { get; }

    /// <inheritdoc/>
    public AuditLogChange<AutoModerationRuleTrigger> Trigger { get; }

    /// <inheritdoc/>
    public AuditLogChange<IAutoModerationTriggerMetadata> TriggerMetadata { get; }

    /// <inheritdoc/>
    public AuditLogChange<IReadOnlyList<IAutoModerationAction>> Actions { get; }

    /// <inheritdoc/>
    public AuditLogChange<bool> IsEnabled { get; }

    /// <inheritdoc/>
    public AuditLogChange<IReadOnlyList<Snowflake>> ExemptRoleIds { get; }

    /// <inheritdoc/>
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
                    Trigger = AuditLogChange<AutoModerationRuleTrigger>.Convert(change);
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
