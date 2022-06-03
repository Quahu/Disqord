using System.Collections.Generic;
using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents an auto-moderation rule.
    /// </summary>
    public interface IAutoModerationRule : ISnowflakeEntity, IGuildEntity, INamableEntity, IJsonUpdatable<AutoModerationRuleJsonModel>
    {
        /// <summary>
        ///     Gets the ID of the user who created this rule.
        /// </summary>
        Snowflake CreatorId { get; }

        // TODO: Gets the event type for which this rule should be executed/checked?
        /// <summary>
        ///     Gets the event type for which this rule should be executed.
        /// </summary>
        AutoModerationEventType EventType { get; }

        // TODO: Better docs wording
        /// <summary>
        ///     Gets the trigger type to be checked for this rule to be triggered.
        /// </summary>
        AutoModerationTriggerType TriggerType { get; }

        // TODO: Might return null, would need to be documented
        /// <summary>
        ///     Gets the trigger metadata of this rule.
        /// </summary>
        IAutoModerationTriggerMetadata TriggerMetadata { get; }

        /// <summary>
        ///     Gets the actions which will be executed when this rule is triggered.
        /// </summary>
        IReadOnlyList<IAutoModerationAction> Actions { get; }

        /// <summary>
        ///     Gets whether this rule is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        ///     Gets the list of the IDs of roles which are exempt from this rule.
        /// </summary>
        IReadOnlyList<Snowflake> ExemptRoles { get; }

        /// <summary>
        ///     Gets the list of the IDs of channels which are exempt from this rule.
        /// </summary>
        IReadOnlyList<Snowflake> ExemptChannels { get; }
    }
}
