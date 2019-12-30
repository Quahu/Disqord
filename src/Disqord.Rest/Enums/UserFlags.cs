using System;

namespace Disqord
{
    [Flags]
    public enum UserFlags : uint
    {
        None                = 0,

        Staff               = 0b0000000000001,

        Partner             = 0b0000000000010,

        HypeSquad           = 0b0000000000100,

        BugHunter           = 0b0000000001000,

        HypeSquadBravery    = 0b0000001000000,

        HypeSquadBrilliance = 0b0000010000000,

        HypeSquadBalance    = 0b0000100000000,

        EarlySupporter      = 0b0001000000000,

        TeamUser            = 0b0010000000000,

        System              = 0b1000000000000
    }
}
