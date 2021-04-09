using System.Collections.Generic;

namespace Disqord
{
    /// <summary>
    ///     Represent a group channel.
    /// </summary>
    public interface IGroupChannel : IPrivateChannel
    {
        /// <summary>
        ///     Gets the icon image hash of this channel.
        /// </summary>
        string IconHash { get; }

        /// <summary>
        ///     Gets the owner ID of this channel.
        /// </summary>
        Snowflake OwnerId { get; }

        /// <summary>
        ///     Gets the recipients of this channel.
        /// </summary>
        IReadOnlyDictionary<Snowflake, IUser> Recipients { get; }
    }
}
