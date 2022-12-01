using System.Collections.Generic;
using Disqord.Models;
using Qommon;

namespace Disqord;

public class LocalAutoModerationTriggerMetadata : ILocalConstruct<LocalAutoModerationTriggerMetadata>, IJsonConvertible<AutoModerationTriggerMetadataJsonModel>
{
    public Optional<IList<string>> Keywords { get; set; }

    public Optional<IList<string>> RegexPatterns { get; set; }

    public Optional<IList<AutoModerationKeywordPresetType>> Presets { get; set; }

    public Optional<IList<string>> AllowedSubstrings { get; set; }

    public Optional<int> MentionLimit { get; set; }

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
        RegexPatterns = other.RegexPatterns.Clone();
        Presets = other.Presets.Clone();
        AllowedSubstrings = other.AllowedSubstrings.Clone();
        MentionLimit = other.MentionLimit;
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
            RegexPatterns = RegexPatterns.ToArray(),
            Presets = Presets.ToArray(),
            AllowList = AllowedSubstrings.ToArray(),
            MentionTotalLimit = MentionLimit
        };
    }
}
