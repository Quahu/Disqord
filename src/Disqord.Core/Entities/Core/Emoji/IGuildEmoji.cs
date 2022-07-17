using System.Collections.Generic;
using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents a custom emoji (e.g. <c>&lt;:professor:667582610431803437&gt;</c>) retrieved from a known guild.
/// </summary>
public interface IGuildEmoji : ICustomEmoji, IGuildEntity, IClientEntity, INamableEntity, IJsonUpdatable<EmojiJsonModel>
{
    /// <summary>
    ///     Gets the role IDs that can use this emoji.
    /// </summary>
    IReadOnlyList<Snowflake> RoleIds { get; }

    /// <summary>
    ///     Gets the user that created this emoji.
    /// </summary>
    IUser Creator { get; }

    /// <summary>
    ///     Gets whether this emoji requires colons in chat.
    /// </summary>
    bool RequiresColons { get; }

    /// <summary>
    ///     Gets whether this emoji is managed by an integration.
    /// </summary>
    bool IsManaged { get; }

    /// <summary>
    ///     Gets whether this emoji is available.
    /// </summary>
    /// <returns>
    ///     <see langword="false"/> when, for example, the emoji limit has been raised and then lowered.
    /// </returns>
    bool IsAvailable { get; }
}
