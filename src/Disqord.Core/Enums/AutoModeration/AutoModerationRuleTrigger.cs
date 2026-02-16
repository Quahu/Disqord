namespace Disqord;

/// <summary>
///     Represents the types of information to be checked to trigger an auto-moderation rule.
/// </summary>
public enum AutoModerationRuleTrigger
{
    /// <summary>
    ///     The trigger checks for user-defined keywords.
    /// </summary>
    Keyword = 1,

    /// <summary>
    ///     The trigger checks for generic spam.
    /// </summary>
    Spam = 3,

    /// <summary>
    ///     The trigger checks for keywords from pre-defined presets.
    /// </summary>
    KeywordPreset = 4,

    /// <summary>
    ///     The trigger checks for mention spam.
    /// </summary>
    MentionSpam = 5
}
