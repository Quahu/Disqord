namespace Disqord;

/// <summary>
///     Represents presets of keywords to be checked.
/// </summary>
public enum AutoModerationKeywordPresetType
{
    /// <summary>
    ///     Words that may be considered swearing or cursing.
    /// </summary>
    Profanity = 1,

    /// <summary>
    ///     Words that refer to sexually explicit behavior or activity.
    /// </summary>
    SexualContent = 2,

    /// <summary>
    ///     Words that may be considered hate speech.
    /// </summary>
    Slurs = 3
}