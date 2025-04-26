using System.Collections.Generic;
using System.Globalization;

namespace Disqord;

/// <summary>
///     Represents an interaction triggered by a user.
/// </summary>
public interface IUserInteraction : IInteraction, IPossiblyGuildEntity, IChannelEntity
{
    /// <summary>
    ///     Gets the channel of this interaction.
    /// </summary>
    IInteractionChannel? Channel { get; }

    /// <summary>
    ///     Gets the user who triggered this interaction.
    /// </summary>
    /// <returns>
    ///     <see cref="IUser"/> or <see cref="IMember"/> if this interaction was triggered in a guild.
    /// </returns>
    IUser Author { get; }

    /// <summary>
    ///     Gets the ID of the user who triggered this interaction.
    /// </summary>
    Snowflake AuthorId => Author.Id;

    /// <summary>
    ///     Gets the author's permissions in the channel of this interaction.
    /// </summary>
    Permissions AuthorPermissions { get; }

    /// <summary>
    ///     Gets the application's permissions in the channel of this interaction.
    /// </summary>
    /// <remarks>
    ///     This represents the bot's permissions for bot applications.
    /// </remarks>
    Permissions ApplicationPermissions { get; }

    /// <summary>
    ///     Gets the locale of the user who triggered this interaction.
    /// </summary>
    CultureInfo Locale { get; }

    /// <summary>
    ///     Gets the preferred locale of the guild this interaction was triggered in.
    /// </summary>
    /// <returns>
    ///     The locale of the guild or <see langword="null"/> if this interaction was triggered in a private channel.
    /// </returns>
    CultureInfo? GuildLocale { get; }

    /// <summary>
    ///     Gets the entitlements of the user who triggered this interaction.
    /// </summary>
    IReadOnlyList<IEntitlement> Entitlements { get; }

    /// <summary>
    ///     Gets the attachment size limit in bytes.
    /// </summary>
    /// <remarks>
    ///     This limit may be higher than the default attachment size limit, depending on the guild's boost status or the author's Nitro status.
    /// </remarks>
    int? AttachmentSizeLimit { get; }
}
