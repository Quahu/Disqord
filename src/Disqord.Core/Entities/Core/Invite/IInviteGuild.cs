using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents a guild targeted by an invite.
/// </summary>
public interface IInviteGuild : ISnowflakeEntity, INamableEntity, IJsonUpdatable<GuildJsonModel>
{
    /// <summary>
    ///     Gets the splash image hash of the guild.
    /// </summary>
    string? SplashHash { get; }

    /// <summary>
    ///     Gets the banner image hash of the guild.
    /// </summary>
    string? BannerHash { get; }

    /// <summary>
    ///     Gets the description of the guild.
    /// </summary>
    string? Description { get; }

    /// <summary>
    ///     Gets the icon image hash of the guild.
    /// </summary>
    string? IconHash { get; }

    /// <summary>
    ///     Gets the features of the guild.
    /// </summary>
    GuildFeatures Features { get; }

    /// <summary>
    ///     Gets the <see cref="GuildVerificationLevel"/> of the guild.
    /// </summary>
    GuildVerificationLevel VerificationLevel { get; }

    /// <summary>
    ///     Gets the vanity URL code of the guild.
    /// </summary>
    string? VanityUrlCode { get; }

    /// <summary>
    ///     Gets the welcome screen of the guild.
    /// </summary>
    IGuildWelcomeScreen WelcomeScreen { get; }

    /// <summary>
    ///     Gets the NSFW level of the guild.
    /// </summary>
    GuildNsfwLevel NsfwLevel { get; }
}
