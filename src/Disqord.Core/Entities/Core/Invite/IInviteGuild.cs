using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents an invite guild.
    /// </summary>
    public interface IInviteGuild : ISnowflakeEntity, INamable, IJsonUpdatable<GuildJsonModel>
    {
        /// <summary>
        ///     Gets the splash image hash of this guild.
        /// </summary>
        string SplashHash { get; }

        /// <summary>
        ///     Gets the banner image hash of this guild.
        /// </summary>
        string BannerHash { get; }

        /// <summary>
        ///     Gets the description of this guild.
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     Gets the icon image hash of this guild.
        /// </summary>
        string IconHash { get; }

        /// <summary>
        ///     Gets the features of this guild.
        /// </summary>
        GuildFeatures Features { get; }

        /// <summary>
        ///     Gets the <see cref="GuildVerificationLevel"/> of this guild.
        /// </summary>
        GuildVerificationLevel VerificationLevel { get; }

        /// <summary>
        ///     Gets the vanity URL code of this guild.
        /// </summary>
        string VanityUrlCode { get; }

        /// <summary>
        ///     Gets the welcome screen of this guild.
        /// </summary>
        IGuildWelcomeScreen WelcomeScreen { get; }

        /// <summary>
        ///     Gets the NSFW level of this guild.
        /// </summary>
        GuildNsfwLevel NsfwLevel { get; }
    }
}
