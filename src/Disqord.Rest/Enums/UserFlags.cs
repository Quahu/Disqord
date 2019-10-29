using System;

namespace Disqord
{
    [Flags]
    public enum UserFlags : uint
    {
        None                = 0,

        Staff               = 0b0000000001,

        Partner             = 0b0000000010,

        HypeSquad           = 0b0000000100,

        BugHunter           = 0b0000001000,

        HypeSquadBravery    = 0b0001000000,

        HypeSquadBrilliance = 0b0010000000,

        HypeSquadBalance    = 0b0100000000,

        EarlySupporter      = 0b1000000000
    }
}
