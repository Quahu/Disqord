using System.Collections.Generic;
using Disqord.Models;
using Qommon;

namespace Disqord
{
    /// <summary>
    ///     Represents a Discord application, e.g. a bot or a game.
    /// </summary>
    public interface IApplication : ISnowflakeEntity, INamableEntity, IJsonUpdatable<ApplicationJsonModel>
    {
        /// <summary>
        ///     Gets the icon image hash of this application.
        /// </summary>
        string IconHash { get; }

        /// <summary>
        ///     Gets the description of this application.
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     Gets whether the bot of this application is public.
        /// </summary>
        bool IsBotPublic { get; }

        /// <summary>
        ///     Gets whether the bot of this application requires
        ///     the full OAuth2 code grant flow.
        /// </summary>
        bool BotRequiresCodeGrant { get; }

        /// <summary>
        ///     Gets the URL of this application's terms of service.
        /// </summary>
        string TermsOfServiceUrl { get; }

        /// <summary>
        ///     Gets the URL of this application's privacy policy.
        /// </summary>
        string PrivacyPolicyUrl { get; }

        /// <summary>
        ///     Gets the owner of this application.
        /// </summary>
        /// <remarks>
        ///     You should not rely on this data for team applications.
        /// </remarks>
        IUser Owner { get; }

        /// <summary>
        ///     Gets the team of this application.
        ///     Returns <see langword="null"/> if this application has no team.
        /// </summary>
        IApplicationTeam Team { get; }

        /// <summary>
        ///     Gets the flags of this application.
        /// </summary>
        Optional<ApplicationFlag> Flags { get; }

        /// <summary>
        ///     Gets the tags of this application.
        /// </summary>
        IReadOnlyList<string> Tags { get; }

        /// <summary>
        ///     Gets the default authorization parameters of this application.
        /// </summary>
        IApplicationDefaultAuthorizationParameters DefaultAuthorizationParameters { get; }

        /// <summary>
        ///     Gets the custom authorization URL of this application.
        /// </summary>
        string CustomAuthorizationUrl { get; }
    }
}
