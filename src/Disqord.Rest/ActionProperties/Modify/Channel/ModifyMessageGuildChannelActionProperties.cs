using System;

namespace Disqord
{
    public abstract class ModifyMessageGuildChannelActionProperties : ModifyNestableChannelActionProperties
    {
        public Optional<TimeSpan> Slowmode { internal get; set; }
        
        internal ModifyMessageGuildChannelActionProperties()
        { }
    }
}
