namespace Disqord;

/// <summary>
///     Represents the primary guild (server tag) of a user.
/// </summary>
public interface IUserPrimaryGuild
{
    /// <summary>
    ///     Gets the ID of the user's primary guild.
    /// </summary>
    /// <returns>
    ///     The ID of the guild or <see langword="null"/> if the identity guild is not set.
    /// </returns>
    Snowflake? GuildId { get; }

    /// <summary>
    ///     Gets whether the user is displaying the primary guild's server tag.
    /// </summary>
    /// <remarks>
    ///     This can be <see langword="null"/> if the system clears the identity,
    ///     e.g. the server no longer supports tags.
    ///     This will be <see langword="false"/> if the user manually removes their tag.
    /// </remarks>
    bool? IsIdentityEnabled { get; }

    /// <summary>
    ///     Gets the text of the user's server tag.
    /// </summary>
    /// <returns>
    ///     The server tag text (up to 4 characters) or <see langword="null"/> if not set.
    /// </returns>
    string? Tag { get; }

    /// <summary>
    ///     Gets the server tag badge image hash.
    /// </summary>
    /// <returns>
    ///     The image hash of the badge or <see langword="null"/> if not set.
    /// </returns>
    string? BadgeHash { get; }
}
