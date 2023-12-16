using System.Collections.Generic;
using Disqord.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord;

/// <inheritdoc cref="IAutoModerationTriggerMetadata"/>
public class TransientAutoModerationTriggerMetadata : TransientEntity<AutoModerationTriggerMetadataJsonModel>, IAutoModerationTriggerMetadata
{
    /// <inheritdoc/>
    public IReadOnlyList<string> Keywords
    {
        get
        {
            if (!Model.KeywordFilter.HasValue)
                return ReadOnlyList<string>.Empty;

            return _keywords ??= Model.KeywordFilter.Value.ToReadOnlyList();
        }
    }
    private IReadOnlyList<string>? _keywords;

    /// <inheritdoc/>
    public IReadOnlyList<string> RegexPatterns
    {
        get
        {
            if (!Model.RegexPatterns.HasValue)
                return ReadOnlyList<string>.Empty;

            return _regexPatterns ??= Model.RegexPatterns.Value.ToReadOnlyList();
        }
    }
    private IReadOnlyList<string>? _regexPatterns;

    /// <inheritdoc/>
    public IReadOnlyList<AutoModerationKeywordPresetType> Presets
    {
        get
        {
            if (!Model.Presets.HasValue)
                return ReadOnlyList<AutoModerationKeywordPresetType>.Empty;

            return _presets ??= Model.Presets.Value.ToReadOnlyList();
        }
    }
    private IReadOnlyList<AutoModerationKeywordPresetType>? _presets;

    /// <inheritdoc/>
    public IReadOnlyList<string> AllowedSubstrings
    {
        get
        {
            if (!Model.AllowList.HasValue)
                return ReadOnlyList<string>.Empty;

            return _allowedSubstrings ??= Model.AllowList.Value.ToReadOnlyList();
        }
    }
    private IReadOnlyList<string>? _allowedSubstrings;

    /// <inheritdoc/>
    public int? MentionLimit => Model.MentionTotalLimit.GetValueOrNullable();

    /// <inheritdoc/>
    public bool IsMentionRaidProtectionEnabled => Model.MentionRaidProtectionEnabled.GetValueOrDefault();

    public TransientAutoModerationTriggerMetadata(AutoModerationTriggerMetadataJsonModel model)
        : base(model)
    { }
}
