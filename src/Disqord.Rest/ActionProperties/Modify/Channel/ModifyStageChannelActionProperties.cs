using Qommon;

namespace Disqord;

public class ModifyStageChannelActionProperties : ModifyNestableChannelActionProperties
{
    public Optional<int> Bitrate { internal get; set; }

    public Optional<string> Region { internal get; set; }

    internal ModifyStageChannelActionProperties()
    { }
}