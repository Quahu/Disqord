using Qommon;

namespace Disqord;

public sealed class ModifyStageActionProperties
{
    public Optional<string> Topic { internal get; set; }

    public Optional<PrivacyLevel> PrivacyLevel { internal get; set; }

    internal ModifyStageActionProperties()
    { }
}