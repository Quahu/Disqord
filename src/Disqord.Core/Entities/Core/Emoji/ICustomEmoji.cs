using System;

namespace Disqord
{
    /// <summary>
    ///     Represents a custom emoji tied to a possibly unknown guild.
    /// </summary>
    public interface ICustomEmoji : IEmoji, ISnowflakeEntity, ITaggable, IEquatable<ICustomEmoji>
    {
        /// <summary>
        ///     Gets whether this emoji is animated. This property is not reliable unless this instance is an <see cref="IGuildEmoji"/>.
        /// </summary>
        bool IsAnimated { get; }
    }
}
