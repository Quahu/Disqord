using System;
using System.Collections.Generic;

namespace Disqord
{
    public class ModifyThreadChannelActionProperties : ModifyGuildChannelActionProperties
    {
        public Optional<bool> IsArchived { internal get; set; }
        
        public Optional<int> AutoArchiveDuration { internal get; set; }
        
        public Optional<bool> IsLocked { internal get; set; }

        public Optional<int> Slowmode { internal get; set; }
        
        public override Optional<IEnumerable<LocalOverwrite>> Overwrites
        {
            internal get => Optional<IEnumerable<LocalOverwrite>>.Empty;
            set => throw new InvalidOperationException("Thread channels do not support overwrites");
        }

        public override Optional<int> Position
        {
            internal get => Optional<int>.Empty;
            set => throw new InvalidOperationException("Thread channels do not support position");
        }

        internal ModifyThreadChannelActionProperties()
        { }
    }
}