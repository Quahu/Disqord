using System.Collections.Generic;
using Qommon;

namespace Disqord
{
    public abstract class ModifyGuildChannelActionProperties
    {
        public Optional<string> Name { internal get; set; }

        public virtual Optional<int> Position { internal get; set; }

        public virtual Optional<IEnumerable<LocalOverwrite>> Overwrites { internal get; set; }

        internal ModifyGuildChannelActionProperties()
        { }
    }
}
