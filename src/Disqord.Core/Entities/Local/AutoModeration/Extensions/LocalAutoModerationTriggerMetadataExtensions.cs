using System.Collections.Generic;
using System.Linq;
using Disqord.Models;
using Qommon;

namespace Disqord;

public static class LocalAutoModerationTriggerMetadataExtensions
{
    public static LocalAutoModerationTriggerMetadata WithKeywords(this LocalAutoModerationTriggerMetadata metadata, IEnumerable<string> keywords)
    {
        Guard.IsNotNull(keywords);

        if (metadata.Keywords.With(keywords, out var list))
            metadata.Keywords = new Optional<IList<string>>(list);

        return metadata;
    }

    public static LocalAutoModerationTriggerMetadata WithKeywords(this LocalAutoModerationTriggerMetadata metadata, params string[] keywords)
    {
        return metadata.WithKeywords(keywords as IEnumerable<string>);
    }

    public static LocalAutoModerationTriggerMetadata WithPresets(this LocalAutoModerationTriggerMetadata metadata, IEnumerable<AutoModerationKeywordPresetType> presets)
    {
        Guard.IsNotNull(presets);

        if (metadata.Presets.With(presets, out var list))
            metadata.Presets = new Optional<IList<AutoModerationKeywordPresetType>>(list);

        return metadata;
    }

    public static LocalAutoModerationTriggerMetadata WithPresets(this LocalAutoModerationTriggerMetadata metadata, params AutoModerationKeywordPresetType[] presets)
    {
        return metadata.WithPresets(presets as IEnumerable<AutoModerationKeywordPresetType>);
    }

    public static AutoModerationTriggerMetadataJsonModel ToModel(this LocalAutoModerationTriggerMetadata metadata)
    {
        return new AutoModerationTriggerMetadataJsonModel
        {
            KeywordFilter = Optional.Convert(metadata.Keywords, x => x.ToArray()),
            Presents = Optional.Convert(metadata.Presets, x => x.ToArray())
        };
    }
}
