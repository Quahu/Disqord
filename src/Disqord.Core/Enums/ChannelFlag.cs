using System;

namespace Disqord
{
    /// <summary>
    ///     Represents the flags of a channel.
    /// </summary>
    [Flags]
    public enum ChannelFlag
    {
        /// <summary>
        ///     The thread channel is pinned to the top of it's parent forum channel.
        /// </summary>
        Pinned = 1 << 1
    }
}
