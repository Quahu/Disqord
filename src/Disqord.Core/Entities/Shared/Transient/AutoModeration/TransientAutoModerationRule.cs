using System.Collections.Generic;
using Disqord.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord
{
    /// <inheritdoc cref="IAutoModerationRule"/>
    public class TransientAutoModerationRule : TransientClientEntity<AutoModerationRuleJsonModel>, IAutoModerationRule
    {
        /// <inheritdoc/>
        public Snowflake Id => Model.Id;

        /// <inheritdoc/>
        public Snowflake GuildId => Model.GuildId;

        /// <inheritdoc/>
        public string Name => Model.Name;

        /// <inheritdoc/>
        public Snowflake CreatorId => Model.CreatorId;

        /// <inheritdoc/>
        public AutoModerationEventType EventType => Model.EventType;

        /// <inheritdoc/>
        public AutoModerationRuleTriggerType TriggerType => Model.TriggerType;

        /// <inheritdoc/>
        public IAutoModerationTriggerMetadata TriggerMetadata => _triggerMetadata ??= new TransientAutoModerationTriggerMetadata(Model.TriggerMetadata);
        private IAutoModerationTriggerMetadata _triggerMetadata;

        /// <inheritdoc/>
        public IReadOnlyList<IAutoModerationAction> Actions => _actions ??= Model.Actions.ToReadOnlyList(x => new TransientAutoModerationAction(x));
        private IReadOnlyList<IAutoModerationAction> _actions;

        /// <inheritdoc/>
        public bool IsEnabled => Model.Enabled;

        /// <inheritdoc/>
        public IReadOnlyList<Snowflake> ExemptRoles => _exemptRoles ??= Model.ExemptRoles.ToReadOnlyList();
        private IReadOnlyList<Snowflake> _exemptRoles;

        /// <inheritdoc/>
        public IReadOnlyList<Snowflake> ExemptChannels => _exemptChannels ??= Model.ExemptChannels.ToReadOnlyList();
        private IReadOnlyList<Snowflake> _exemptChannels;

        public TransientAutoModerationRule(IClient client, AutoModerationRuleJsonModel model)
            : base(client, model)
        { }
    }
}
