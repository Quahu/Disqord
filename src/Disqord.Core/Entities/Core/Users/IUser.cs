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
    string Discriminator { get; }

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
