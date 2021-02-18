using System.Collections.Generic;

namespace Disqord
{
    /// <summary>
    ///     Represents a custom emoji retrieved from a known guild.
    /// </summary>
    public interface IGuildEmoji : ICustomEmoji, IGuildEntity
    {
        /// <summary>
        ///     Gets the role IDs that can use this emoji.
        /// </summary>
        IReadOnlyList<Snowflake> RoleIds { get; }

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
        ///     Returns <see langword="false"/> when, for example, the emoji limit has been raised and then lowered.
        /// </summary>
        bool IsAvailable { get; }
    }
}
