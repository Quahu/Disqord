using System;

namespace Disqord.Gateway;

public class AutoModerationRuleUpdatedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild in which the rule was updated.
    /// </summary>
    public Snowflake GuildId => NewRule.GuildId;

    /// <summary>
    ///     Gets the ID of the updated rule.
    /// </summary>
    public Snowflake RuleId => NewRule.Id;

    /// <summary>
    ///     Gets the updated rule.
    /// </summary>
    public IAutoModerationRule NewRule { get; }

    public AutoModerationRuleUpdatedEventArgs(IAutoModerationRule newRule)
    {
        NewRule = newRule;
    }
}