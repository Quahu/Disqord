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
}
