using System.Collections.Generic;
using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents an auto-moderation rule.
/// </summary>
public interface IAutoModerationRule : ISnowflakeEntity, IGuildEntity, INamableEntity, IJsonUpdatable<AutoModerationRuleJsonModel>
{
    /// <summary>
    ///     Gets the ID of the user who created this rule.
    /// </summary>
    Snowflake CreatorId { get; }

    /// <summary>
    ///     Gets the event type this rule applies to.
    /// </summary>
    AutoModerationEventType EventType { get; }

    /// <summary>
    ///     Gets the trigger type to be checked for this rule.
    /// </summary>
    AutoModerationRuleTrigger Trigger { get; }

    /// <summary>
    ///     Gets the trigger metadata of this rule.
    /// </summary>
    /// <returns>
    ///     The trigger metadata of this rule or <see langword="null"/>
    ///     if this rule's trigger does not have any metadata.
    /// </returns>
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
    IReadOnlyList<Snowflake> ExemptRoleIds { get; }

    /// <summary>
    ///     Gets the list of the IDs of channels which are exempt from this rule.
    /// </summary>
    IReadOnlyList<Snowflake> ExemptChannelIds { get; }
}