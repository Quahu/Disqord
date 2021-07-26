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
        ///     The system channel does not send member join notifications.
        /// </summary>
        SuppressJoinNotifications = 1 << 0,

        /// <summary>
        ///     The system channel does not send guild boost notifications.
        /// </summary>
        SuppressBoostNotifications = 1 << 1,

        /// <summary>
        ///     The system channel does not send guild setup tips.
        /// </summary>
        SuppressSetupTips = 1 << 2
    }
}
