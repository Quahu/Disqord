using System.Collections.Generic;

namespace Disqord
{
    public abstract class ModifyGuildChannelActionProperties : ModifyChannelActionProperties
    {
        public Optional<int> Position { internal get; set; }

        public Optional<IEnumerable<LocalOverwrite>> Overwrites { internal get; set; }

        internal ModifyGuildChannelActionProperties()
        { }
    }
}
