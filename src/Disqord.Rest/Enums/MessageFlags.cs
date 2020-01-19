using System;

namespace Disqord
{
    [Flags]
    public enum MessageFlags
    {
        None              = 0,

        Crossposted       = 0b0001,

        Crosspost         = 0b0010,

        SuppressedEmbeds  = 0b0100,

        Urgent            = 0b1000
    }
}
