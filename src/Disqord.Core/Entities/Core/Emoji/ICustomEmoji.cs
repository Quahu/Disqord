using System;

namespace Disqord
{
    /// <summary>
    ///     Represents a custom emoji (e.g. <c>&lt;:professor:667582610431803437&gt;</c>) tied to a possibly unknown guild.
    /// </summary>
    public interface ICustomEmoji : IEmoji, ISnowflakeEntity, ITaggable, IEquatable<ICustomEmoji>
    {
        /// <summary>
        ///     Gets whether this emoji is animated. This property is not reliable unless this instance is an <see cref="IGuildEmoji"/>.
        /// </summary>
        bool IsAnimated { get; }
    }
}
