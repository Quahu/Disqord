using System;

namespace Disqord
{
    [Flags]
    public enum MessageFlags
    {
        None                = 0,

        IsCrossposted       = 0b001,

        IsCrosspost         = 0b010,

        HasSuppressedEmbeds = 0b100
    }
}
