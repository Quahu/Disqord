using System.ComponentModel;
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
        ///     Gets the time at which this interaction was received locally.
        /// </summary>
        /// <remarks>
        ///     This is an internal Disqord API and should not be used.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        long ReceivedAt { get; }

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
        ///     Gets the author's permissions in the channel of this interaction.
        /// </summary>
        Permission AuthorPermissions { get; }

        /// <summary>
        ///     Gets the locale of the user who triggered this interaction.
        ///     Returns <see langword="null"/> if <see cref="Type"/> is <see cref="InteractionType.Ping"/>.
        /// </summary>
        CultureInfo Locale { get; }

        /// <summary>
        ///     Gets the preferred locale of the guild this interaction was triggered in.
        ///     Returns <see langword="null"/> if this interaction was triggered in a private channel.
        /// </summary>
        CultureInfo GuildLocale { get; }
    }
}
