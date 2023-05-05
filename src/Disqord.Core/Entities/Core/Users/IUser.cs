using System;
using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents a user.
/// </summary>
public interface IUser : ISnowflakeEntity, INamableEntity, IMentionableEntity, ITaggableEntity, IJsonUpdatable<UserJsonModel>
{
    /// <summary>
    ///     Gets the 4-digit discriminator of this user.
    /// </summary>
    [Obsolete(Pomelo.DiscriminatorObsoletion)]
    string Discriminator { get; }

    /// <summary>
    ///     Gets the global name of this user.
    /// </summary>
    /// <remarks>
    ///     This name is not unique and allows more characters.
    ///     Think of this as a global nick of the user.
    ///     <para/>
    ///     This is a part of the new name system.
    ///     Use the <see cref="Pomelo.HasMigratedName"/> extension method to check if the user is using the new name system.
    /// </remarks>
    /// <returns>
    ///     The global name or <see langword="null"/> if not set.
    /// </returns>
    string? GlobalName { get; }

    /// <summary>
    ///     Gets the avatar image hash of this user.
    /// </summary>
    /// <returns>
    ///     The image hash of the avatar or <see langword="null"/> if the user has no avatar set.
    /// </returns>
    string? AvatarHash { get; }

    /// <summary>
    ///     Gets whether this user is a bot.
    /// </summary>
    bool IsBot { get; }

    /// <summary>
    ///     Gets the public <see cref="UserFlags"/> of this user.
    /// </summary>
    UserFlags PublicFlags { get; }
}
