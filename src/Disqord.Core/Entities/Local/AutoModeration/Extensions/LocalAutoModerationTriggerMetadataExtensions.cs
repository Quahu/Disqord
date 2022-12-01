using System.Collections.Generic;
using System.ComponentModel;
using Qommon;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalAutoModerationTriggerMetadataExtensions
{
    public static LocalAutoModerationTriggerMetadata AddKeyword(this LocalAutoModerationTriggerMetadata metadata, string keyword)
    {
        Guard.IsNotNull(keyword);

        if (metadata.Keywords.Add(keyword, out var list))
            metadata.Keywords = new(list);

        return metadata;
    }

    public static LocalAutoModerationTriggerMetadata WithKeywords(this LocalAutoModerationTriggerMetadata metadata, IEnumerable<string> keywords)
    {
        Guard.IsNotNull(keywords);

        if (metadata.Keywords.With(keywords, out var list))
            metadata.Keywords = new(list);

        return metadata;
    }

    public static LocalAutoModerationTriggerMetadata WithKeywords(this LocalAutoModerationTriggerMetadata metadata, params string[] keywords)
    {
        return metadata.WithKeywords(keywords as IEnumerable<string>);
    }

    public static LocalAutoModerationTriggerMetadata AddRegexPattern(this LocalAutoModerationTriggerMetadata metadata, string regexPattern)
    {
        Guard.IsNotNull(regexPattern);

        if (metadata.RegexPatterns.Add(regexPattern, out var list))
            metadata.RegexPatterns = new(list);

        return metadata;
    }

    public static LocalAutoModerationTriggerMetadata WithRegexPatterns(this LocalAutoModerationTriggerMetadata metadata, IEnumerable<string> regexPatterns)
    {
        Guard.IsNotNull(regexPatterns);

        if (metadata.RegexPatterns.With(regexPatterns, out var list))
            metadata.RegexPatterns = new(list);

        return metadata;
    }

    public static LocalAutoModerationTriggerMetadata WithRegexPatterns(this LocalAutoModerationTriggerMetadata metadata, params string[] regexPatterns)
    {
        return metadata.WithRegexPatterns(regexPatterns as IEnumerable<string>);
    }

    public static LocalAutoModerationTriggerMetadata AddPreset(this LocalAutoModerationTriggerMetadata metadata, AutoModerationKeywordPresetType preset)
    {
        if (metadata.Presets.Add(preset, out var list))
            metadata.Presets = new(list);

        return metadata;
    }

    public static LocalAutoModerationTriggerMetadata WithPresets(this LocalAutoModerationTriggerMetadata metadata, IEnumerable<AutoModerationKeywordPresetType> presets)
    {
        Guard.IsNotNull(presets);

        if (metadata.Presets.With(presets, out var list))
            metadata.Presets = new(list);

        return metadata;
    }

    public static LocalAutoModerationTriggerMetadata WithPresets(this LocalAutoModerationTriggerMetadata metadata, params AutoModerationKeywordPresetType[] presets)
    {
        return metadata.WithPresets(presets as IEnumerable<AutoModerationKeywordPresetType>);
    }

    public static LocalAutoModerationTriggerMetadata AddAllowedSubstring(this LocalAutoModerationTriggerMetadata metadata, string allowedSubstring)
    {
        Guard.IsNotNull(allowedSubstring);

        if (metadata.AllowedSubstrings.Add(allowedSubstring, out var list))
            metadata.AllowedSubstrings = new(list);

        return metadata;
    }

    public static LocalAutoModerationTriggerMetadata WithAllowedSubstrings(this LocalAutoModerationTriggerMetadata metadata, IEnumerable<string> allowedSubstrings)
    {
        Guard.IsNotNull(allowedSubstrings);

        if (metadata.AllowedSubstrings.With(allowedSubstrings, out var list))
            metadata.AllowedSubstrings = new(list);

        return metadata;
    }

    public static LocalAutoModerationTriggerMetadata WithAllowedSubstrings(this LocalAutoModerationTriggerMetadata metadata, params string[] allowedSubstrings)
    {
        return metadata.WithAllowedSubstrings(allowedSubstrings as IEnumerable<string>);
    }

    public static LocalAutoModerationTriggerMetadata WithMentionLimit(this LocalAutoModerationTriggerMetadata metadata, int mentionLimit)
    {
        metadata.MentionLimit = mentionLimit;
        return metadata;
    }
}
