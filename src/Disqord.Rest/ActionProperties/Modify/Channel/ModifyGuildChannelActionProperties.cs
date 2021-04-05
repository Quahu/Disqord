using System.Collections.Generic;

namespace Disqord
{
    public abstract class ModifyGuildChannelActionProperties
    {
        public Optional<string> Name { internal get; set; }

        public Optional<int> Position { internal get; set; }

        public Optional<IEnumerable<LocalOverwrite>> Overwrites { internal get; set; }

        internal ModifyGuildChannelActionProperties()
        { }
    }
}
