using System.Globalization;
using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents an interaction
    /// </summary>
    public interface IInteraction : ISnowflakeEntity, IPossibleGuildEntity, IChannelEntity, IJsonUpdatable<InteractionJsonModel>
    {
        /// <summary>
        ///     Gets the ID of the application of this interaction.
        /// </summary>
        Snowflake ApplicationId { get; }

        /// <summary>
        ///     Gets the version of this interaction.
        /// </summary>
        int Version { get; }

        /// <summary>
        ///     Gets the type of this interaction.
        /// </summary>
        InteractionType Type { get; }

        /// <summary>
        ///     Gets the token of this interaction.
        /// </summary>
        string Token { get; }

        /// <summary>
        ///     Gets the user/member who triggered this interaction.
        /// </summary>
        IUser Author { get; }

        /// <summary>
        ///     Gets the locale of the user who triggered this interaction.
        ///     Returns <see langword="null"/> if the <see cref="Type"/> of this interaction is <see cref="InteractionType.Ping"/>.
        /// </summary>
        CultureInfo Locale { get; }

        /// <summary>
        ///     Gets the preferred locale of the guild this interaction was triggered in.
        ///     Returns <see langword="null"/> if this interaction was not triggered in a guild.
        /// </summary>
        CultureInfo GuildPreferredLocale { get; }
    }
}
