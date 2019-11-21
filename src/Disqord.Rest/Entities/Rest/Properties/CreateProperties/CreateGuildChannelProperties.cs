using System.Collections.Generic;

namespace Disqord
{
    public abstract class CreateGuildChannelProperties
    {
        public Optional<int> Position { internal get; set; }

        public Optional<IReadOnlyList<LocalOverwrite>> Overwrites { internal get; set; }

        internal CreateGuildChannelProperties()
        { }
    }
}
