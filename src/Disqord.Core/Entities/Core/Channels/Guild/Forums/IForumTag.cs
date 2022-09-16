using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents a tag that can be applied to threads in a forum channel.
/// </summary>
public interface IForumTag : IIdentifiableEntity, INamableEntity, IJsonUpdatable<ForumTagJsonModel>
{
    /// <summary>
    ///     Gets whether this tag can only be applied to threads
    ///     by members with the <see cref="Permissions.ManageThreads"/> permission.
    /// </summary>
    bool IsModerated { get; }

    /// <summary>
    ///     Gets the emoji of this tag.
    /// </summary>
    IEmoji Emoji { get; }
}
