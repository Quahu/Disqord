using System.Collections.Generic;

namespace Disqord
{
    /// <summary>
    ///     Represent a private group channel.
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

        /// <summary>
        ///     Gets the ID of the application that created this channel.
        ///     Returns <see langword="null"/> if the channel was not created by a bot.
        /// </summary>
        Snowflake? ApplicationId { get; }
    }
}
