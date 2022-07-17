using Qommon;

namespace Disqord;

public sealed class ModifyGuildStickerActionProperties
{
    public Optional<string> Name { internal get; set; }

    public Optional<string> Description { internal get; set; }

    public Optional<string> Tags { internal get; set; }
}