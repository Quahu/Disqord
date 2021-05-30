using System;

namespace Disqord
{
    /// <summary>
    ///     Represents an emoji.
    ///     This can be a custom emoji (e.g. <c>&lt;:professor:667582610431803437&gt;</c>) or a default Unicode emoji (e.g. <c>🍿</c>).
    /// </summary>
    public interface IEmoji : IEquatable<IEmoji>
    {
        /// <summary>
        ///     Gets the name of this emoji.
        ///     For Unicode emojis this returns the Unicode string.
        /// </summary>
        string Name { get; }
    }
}
