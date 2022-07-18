using System.Collections.Generic;
using Disqord.Models;
using Qommon;

namespace Disqord;

public class LocalAutoModerationTriggerMetadata : ILocalConstruct<LocalAutoModerationTriggerMetadata>, IJsonConvertible<AutoModerationTriggerMetadataJsonModel>
{
    public Optional<IList<string>> Keywords { get; set; }

    public Optional<IList<AutoModerationKeywordPresetType>> Presets { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalAutoModerationTriggerMetadata"/>.
    /// </summary>
    public LocalAutoModerationTriggerMetadata()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalAutoModerationTriggerMetadata"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalAutoModerationTriggerMetadata(LocalAutoModerationTriggerMetadata other)
    {
        Keywords = other.Keywords.Clone();
        Presets = other.Presets.Clone();
    }

    /// <inheritdoc/>
    public virtual LocalAutoModerationTriggerMetadata Clone()
    {
        return new(this);
    }

    /// <inheritdoc />
    public AutoModerationTriggerMetadataJsonModel ToModel()
    {
        return new AutoModerationTriggerMetadataJsonModel
        {
            KeywordFilter = Keywords.ToArray(),
            Presets = Presets.ToArray()
        };
    }
}
