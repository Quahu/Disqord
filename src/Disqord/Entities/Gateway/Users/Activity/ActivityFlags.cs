using System;

namespace Disqord
{
    /// <summary>
    ///     Describes what the activity includes.
    /// </summary>
    [Flags]
    public enum ActivityFlags
    {
        None        = 0,

        Instance    = 0b000001,

        Join        = 0b000010,

        Spectate    = 0b000100,

        JoinRequest = 0b001000,

        Sync        = 0b010000,

        Play        = 0b100000
    }
}