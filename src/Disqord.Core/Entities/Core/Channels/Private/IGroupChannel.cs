using System.Collections.Generic;

namespace Disqord;

/// <summary>
///     Represent a private group channel.
/// </summary>
public interface IGroupChannel : IPrivateChannel
{
    /// <summary>
    ///     Gets the icon image hash of this channel.
    /// </summary>
    string? IconHash { get; }

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
    /// </summary>
    /// <returns>
    ///     The ID of the application or <see langword="null"/> if the channel was not created by a bot.
    /// </returns>
    Snowflake? ApplicationId { get; }
}
