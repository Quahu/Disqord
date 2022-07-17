using System;

namespace Disqord.Gateway;

public class AutoModerationRuleCreatedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild in which the rule was created.
    /// </summary>
    public Snowflake GuildId => Rule.GuildId;

    /// <summary>
    ///     Gets the ID of the created rule.
    /// </summary>
    public Snowflake RuleId => Rule.Id;

    /// <summary>
    ///     Gets the created rule.
    /// </summary>
    public IAutoModerationRule Rule { get; }

    public AutoModerationRuleCreatedEventArgs(IAutoModerationRule rule)
    {
        Rule = rule;
    }
}