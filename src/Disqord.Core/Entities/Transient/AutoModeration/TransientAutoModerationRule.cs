using System.Collections.Generic;
using Disqord.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord;

/// <inheritdoc cref="IAutoModerationRule"/>
public class TransientAutoModerationRule : TransientClientEntity<AutoModerationRuleJsonModel>, IAutoModerationRule
{
    /// <inheritdoc/>
    public Snowflake Id => Model.Id;

    /// <inheritdoc/>
    public Snowflake GuildId => Model.GuildId;

    /// <inheritdoc cref="INamableEntity.Name"/>
    public string Name => Model.Name;

    /// <inheritdoc/>
    public Snowflake CreatorId => Model.CreatorId;

    /// <inheritdoc/>
    public AutoModerationEventType EventType => Model.EventType;

    /// <inheritdoc/>
    public AutoModerationRuleTrigger Trigger => Model.Trigger;

    /// <inheritdoc/>
    public IAutoModerationTriggerMetadata TriggerMetadata => _triggerMetadata ??= new TransientAutoModerationTriggerMetadata(Model.TriggerMetadata);

    private IAutoModerationTriggerMetadata? _triggerMetadata;

    /// <inheritdoc/>
    public IReadOnlyList<IAutoModerationAction> Actions => _actions ??= Model.Actions.ToReadOnlyList(x => new TransientAutoModerationAction(x));

    private IReadOnlyList<IAutoModerationAction>? _actions;

    /// <inheritdoc/>
    public bool IsEnabled => Model.Enabled;

    /// <inheritdoc/>
    public IReadOnlyList<Snowflake> ExemptRoleIds => _exemptRoleIds ??= Model.ExemptRoles.ToReadOnlyList();

    private IReadOnlyList<Snowflake>? _exemptRoleIds;

    /// <inheritdoc/>
    public IReadOnlyList<Snowflake> ExemptChannelIds => _exemptChannelIds ??= Model.ExemptChannels.ToReadOnlyList();

    private IReadOnlyList<Snowflake>? _exemptChannelIds;

    public TransientAutoModerationRule(IClient client, AutoModerationRuleJsonModel model)
        : base(client, model)
    { }
}
