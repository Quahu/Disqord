using System;

namespace Disqord.Gateway;

public class AutoModerationActionExecutedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild in which the action was executed.
    /// </summary>
    public Snowflake GuildId { get; }

    /// <summary>
    ///     Gets the ID of the user who triggered the action.
    /// </summary>
    public Snowflake UserId { get; }

    /// <summary>
    ///     Gets the ID of the channel in which the content was sent.
    /// </summary>
    public Snowflake? ChannelId { get; }

    /// <summary>
    ///     Gets the ID of the message in which triggered the action.
    /// </summary>
    public Snowflake? MessageId { get; }

    /// <summary>
    ///     Gets the ID of the rule which the action belongs to.
    /// </summary>
    public Snowflake RuleId { get; }

    /// <summary>
    ///     Gets the trigger type of the rule which was triggered.
    /// </summary>
    public AutoModerationRuleTrigger RuleTrigger { get; }

    /// <summary>
    ///     Gets the ID of the alert system message posted as a result of this action.
    /// </summary>
    public Snowflake? AlertSystemMessageId { get; }

    /// <summary>
    ///     Gets the action which was executed.
    /// </summary>
    public IAutoModerationAction Action { get; }

    /// <summary>
    ///     Gets the text content which triggered the action.
    /// </summary>
    public string Content { get; }

    /// <summary>
    ///     Gets the matched keyword which triggered the action.
    /// </summary>
    public string? MatchedKeyword { get; }

    /// <summary>
    ///     Gets the matched content which triggered the action.
    /// </summary>
    public string? MatchedContent { get; }

    public AutoModerationActionExecutedEventArgs(
        Snowflake guildId, Snowflake userId, Snowflake? channelId, Snowflake? messageId,
        Snowflake ruleId, AutoModerationRuleTrigger ruleTrigger, Snowflake? alertSystemMessageId,
        IAutoModerationAction action, string content, string? matchedKeyword, string? matchedContent)
    {
        GuildId = guildId;
        UserId = userId;
        ChannelId = channelId;
        MessageId = messageId;
        RuleId = ruleId;
        RuleTrigger = ruleTrigger;
        AlertSystemMessageId = alertSystemMessageId;
        Action = action;
        Content = content;
        MatchedKeyword = matchedKeyword;
        MatchedContent = matchedContent;
    }
}
