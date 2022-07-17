using System.Collections.Generic;
using Qommon;

namespace Disqord;

public class LocalAutoModerationTriggerMetadata : ILocalConstruct<LocalAutoModerationTriggerMetadata>
{
    public Optional<IList<string>> Keywords { get; set; }

    public Optional<IList<AutoModerationKeywordPresetType>> Presets { get; set; }

    public LocalAutoModerationTriggerMetadata()
    { }

    protected LocalAutoModerationTriggerMetadata(LocalAutoModerationTriggerMetadata other)
    {
        Keywords = other.Keywords.Clone();
        Presets = other.Presets.Clone();
    }

    public virtual LocalAutoModerationTriggerMetadata Clone()
    {
        return new(this);
    }
}
