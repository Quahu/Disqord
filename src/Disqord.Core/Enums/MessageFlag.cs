using System;

namespace Disqord
{
    [Flags]
    public enum MessageFlag
    {
        None                 = 0,

        Crossposted          = 1 << 0,

        Crosspost            = 1 << 1,

        SuppressedEmbeds     = 1 << 2,

        SourceMessageDeleted = 1 << 3,

        Urgent               = 1 << 4
    }
}
