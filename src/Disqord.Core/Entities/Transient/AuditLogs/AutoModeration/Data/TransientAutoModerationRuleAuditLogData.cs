using System.Collections.Generic;
using Disqord.Models;
using Qommon;

namespace Disqord.AuditLogs;

public class TransientAutoModerationRuleAuditLogData : IAutoModerationRuleAuditLogData
{
    /// <inheritdoc/>
    public Optional<string> Name { get; }

    /// <inheritdoc/>
    public Optional<AutoModerationEventType> EventType { get; }

    /// <inheritdoc/>
    public Optional<AutoModerationRuleTrigger> Trigger { get; }

    /// <inheritdoc/>
    public Optional<IAutoModerationTriggerMetadata> TriggerMetadata { get; }

    /// <inheritdoc/>
    public Optional<IReadOnlyList<IAutoModerationAction>> Actions { get; }

    /// <inheritdoc/>
    public Optional<bool> IsEnabled { get; }

    /// <inheritdoc/>
    public Optional<IReadOnlyList<Snowflake>> ExemptRoleIds { get; }

    /// <inheritdoc/>
    public Optional<IReadOnlyList<Snowflake>> ExemptChannelIds { get; }

    public TransientAutoModerationRuleAuditLogData(IClient client, AuditLogEntryJsonModel model, bool isCreated)
    {
        var changes = new TransientAutoModerationRuleAuditLogChanges(client, model);
        if (isCreated)
        {
            Name = changes.Name.NewValue;
            EventType = changes.EventType.NewValue;
            Trigger = changes.Trigger.NewValue;
            TriggerMetadata = changes.TriggerMetadata.NewValue;
            Actions = changes.Actions.NewValue;
            IsEnabled = changes.IsEnabled.NewValue;
            ExemptRoleIds = changes.ExemptRoleIds.NewValue;
            ExemptChannelIds = changes.ExemptChannelIds.NewValue;
        }
        else
        {
            Name = changes.Name.OldValue;
            EventType = changes.EventType.OldValue;
            Trigger = changes.Trigger.OldValue;
            TriggerMetadata = changes.TriggerMetadata.OldValue;
            Actions = changes.Actions.OldValue;
            IsEnabled = changes.IsEnabled.OldValue;
            ExemptRoleIds = changes.ExemptRoleIds.OldValue;
            ExemptChannelIds = changes.ExemptChannelIds.OldValue;
        }
    }
}
