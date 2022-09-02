using System.Collections.Generic;
using Qommon;

namespace Disqord;

public abstract class CreateGuildChannelActionProperties
{
    public Optional<int> Position { internal get; set; }

    public Optional<IReadOnlyList<LocalOverwrite>> Overwrites { internal get; set; }

    public Optional<GuildChannelFlags> Flags { internal get; set; }

    internal CreateGuildChannelActionProperties()
    { }
}
