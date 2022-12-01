using System.Collections.Generic;
using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents an auto-moderation trigger's metadata.
/// </summary>
public interface IAutoModerationTriggerMetadata : IEntity, IJsonUpdatable<AutoModerationTriggerMetadataJsonModel>
{
    /// <summary>
    ///     Gets the list of keywords to match in message content.
    /// </summary>
    /// <remarks>
    ///     Used by the <see cref="AutoModerationRuleTrigger.Keyword"/> trigger type.
    /// </remarks>
    IReadOnlyList<string> Keywords { get; }

    /// <summary>
    ///     Gets the list of regex patterns to match in message content.
    /// </summary>
    /// <remarks>
    ///     Used by the <see cref="AutoModerationRuleTrigger.Keyword"/> trigger type.
    /// </remarks>
    IReadOnlyList<string> RegexPatterns { get; }

    /// <summary>
    ///     Gets which pre-defined lists of keywords should matched in message content.
    /// </summary>
    /// <remarks>
    ///     Used by the <see cref="AutoModerationRuleTrigger.KeywordPreset"/> trigger type.
    /// </remarks>
    IReadOnlyList<AutoModerationKeywordPresetType> Presets { get; }

    /// <summary>
    ///     Gets the list of substrings not to match in message content.
    /// </summary>
    /// <remarks>
    ///     Used by the <see cref="AutoModerationRuleTrigger.KeywordPreset"/> trigger type.
    /// </remarks>
    IReadOnlyList<string> AllowedSubstrings { get; }

    /// <summary>
    ///     Gets the total number of unique role and user mentions allowed in message content.
    /// </summary>
    /// <remarks>
    ///     Used by the <see cref="AutoModerationRuleTrigger.MentionSpam"/> trigger type.
    /// </remarks>
    int? MentionLimit { get; }
}
