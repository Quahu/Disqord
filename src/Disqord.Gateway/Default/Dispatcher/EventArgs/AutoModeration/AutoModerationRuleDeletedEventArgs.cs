using System;

namespace Disqord.Gateway;

public class AutoModerationRuleDeletedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild in which the rule was deleted.
    /// </summary>
    public Snowflake GuildId => Rule.GuildId;

    /// <summary>
    ///     Gets the ID of the deleted rule.
    /// </summary>
    public Snowflake RuleId => Rule.Id;

    /// <summary>
    ///     Gets the deleted rule.
    /// </summary>
    public IAutoModerationRule Rule { get; }

    public AutoModerationRuleDeletedEventArgs(IAutoModerationRule rule)
    {
        Rule = rule;
    }
}