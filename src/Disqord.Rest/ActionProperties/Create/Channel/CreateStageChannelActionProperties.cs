using Qommon;

namespace Disqord;

public sealed class CreateStageChannelActionProperties : CreateNestedChannelActionProperties
{
    public Optional<int> Bitrate { internal get; set; }

    public Optional<string> Region { internal get; set; }

    internal CreateStageChannelActionProperties()
    { }
}