using System;

namespace Disqord
{
    [Flags]
    public enum MessageFlags
    {
        None                = 0,

        IsCrossposted       = 0b0001,

        IsCrosspost         = 0b0010,

        HasSuppressedEmbeds = 0b0100,

        IsUrgent            = 0b1000
    }
}
