using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents a user.
    /// </summary>
    public interface IUser : ISnowflakeEntity, INamable, IMentionable, ITaggable, IJsonUpdatable<UserJsonModel>
    {
        /// <summary>
        ///     Gets the 4-digit discriminator of this user.
        /// </summary>
        string Discriminator { get; }

        /// <summary>
        ///     Gets the avatar image hash of this user.
        /// </summary>
        string AvatarHash { get; }

        /// <summary>
        ///     Gets whether this user is a bot.
        /// </summary>
        bool IsBot { get; }

        /// <summary>
        ///     Gets the public <see cref="UserFlag"/> of this user.
        /// </summary>
        UserFlag PublicFlags { get; }
    }
}
