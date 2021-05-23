using System;

namespace Disqord
{
    /// <summary>
    ///     Represents flags for guild system channels.
    /// </summary>
    [Flags]
    public enum SystemChannelFlag
    {
        /// <summary>
        ///     Suppresses member join notifications.
        /// </summary>
        SuppressJoinNotifications = 1 << 0,

        /// <summary>
        ///     Suppresses guild boost notifications.
        /// </summary>
        SuppressBoostNotifications = 1 << 1,

        /// <summary>
        ///     Suppresses guild setup tips.
        /// </summary>
        SuppressSetupTips = 1 << 2
    }
}
