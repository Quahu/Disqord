using System;

namespace Disqord.Extensions.Interactivity.Menus
{
    /// <summary>
    ///     Marks a method as a <see cref="Button"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ButtonAttribute : Attribute
    {
        /// <inheritdoc cref="Button.Emoji"/>
        public LocalEmoji Emoji { get; }

        /// <summary>
        ///     Instantiates a new <see cref="ButtonAttribute"/> with the specified emoji <see cref="string"/>.
        ///     This can be a custom emoji (e.g. <c>&lt;:professor:667582610431803437&gt;</c>) or a default Unicode emoji (e.g. <c>🍿</c>).
        ///     The type will be determined using <see cref="LocalCustomEmoji.TryParse(string, out LocalCustomEmoji)"/>.
        /// </summary>
        /// <param name="emoji"> The emoji <see cref="string"/> to parse. </param>
        public ButtonAttribute(string emoji)
        {
            if (emoji == null)
                throw new ArgumentNullException(nameof(emoji));

            Emoji = LocalCustomEmoji.TryParse(emoji, out var customEmoji)
                ? customEmoji
                : new LocalEmoji(emoji);
        }

        /// <summary>
        ///     Instantiates a new <see cref="ButtonAttribute"/> with the specified custom emoji ID
        ///     and optionally a name and whether the emoji is animated.
        /// </summary>
        /// <remarks>
        ///     <inheritdoc cref="LocalCustomEmoji(Snowflake, string, bool)"/>
        /// </remarks>
        /// <param name="emojiId"></param>
        /// <param name="name"></param>
        /// <param name="isAnimated"></param>
        public ButtonAttribute(ulong emojiId, string name = null, bool isAnimated = false)
        {
            Emoji = new LocalCustomEmoji(emojiId, name, isAnimated);
        }
    }
}
