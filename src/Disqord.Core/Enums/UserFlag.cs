using System;

namespace Disqord
{
    [Flags]
    public enum UserFlag : uint
    {
        None                        = 0,

        Staff                       = 1 << 0,

        Partner                     = 1 << 1,

        HypeSquad                   = 1 << 2,

        BugHunterLevel1             = 1 << 3,

        MfaSms                      = 1 << 4,

        NitroPromotionDismissed     = 1 << 5,

        HypeSquadBravery            = 1 << 6,

        HypeSquadBrilliance         = 1 << 7,

        HypeSquadBalance            = 1 << 8,

        EarlySupporter              = 1 << 9,

        TeamUser                    = 1 << 10,

        System                      = 1 << 12,

        UnreadUrgentMessages        = 1 << 13,

        BugHunterLevel2             = 1 << 14,

        VerifiedBot                 = 1 << 16,

        EarlyVerifiedBotDeveloper   = 1 << 17,

        CertifiedModerator          = 1 << 18
    }
}
