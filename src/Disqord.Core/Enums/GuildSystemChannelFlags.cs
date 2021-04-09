using System;

namespace Disqord
{
    /// <summary>
    ///     Represents flags for guild system channels.
    /// </summary>
    [Flags]
    public enum GuildSystemChannelFlags
    {
        /// <summary>
        ///     Suppresses member join notifications.
        /// </summary>
        SuppressJoinNotifications = 1 << 0,

        /// <summary>
        ///     Suppresses guild boost notifications.
        /// </summary>
        SuppressPremiumSubscriptions = 1 << 1
    }
}