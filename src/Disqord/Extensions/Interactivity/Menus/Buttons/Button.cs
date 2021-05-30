using System;
using System.Threading.Tasks;

namespace Disqord.Extensions.Interactivity.Menus
{
    /// <summary>
    ///     Represents a method that executes when the given <see cref="Button"/> is triggered.
    /// </summary>
    /// <param name="e"> The reaction event data. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the callback work.
    /// </returns>
    public delegate ValueTask ButtonCallback(ButtonEventArgs e);

    /// <summary>
    ///     Represents a callback that gets executed whenever a user reacts with the appropriate emoji.
    /// </summary>
    public sealed class Button
    {
        /// <summary>
        ///     Gets the emoji of this button.
        /// </summary>
        public LocalEmoji Emoji { get; }

        /// <summary>
        ///     Gets the callback of this button.
        /// </summary>
        public ButtonCallback Callback { get; }

        /// <summary>
        ///     Gets the position of this button.
        /// </summary>
        public int Position { get; }

        /// <summary>
        ///     Instantiates a new <see cref="Button"/> with the specified emoji, callback, and optionally
        ///     the position in which the button should appear on the message.
        /// </summary>
        /// <param name="emoji"> The emoji this button will be triggered with. </param>
        /// <param name="callback"> The callback this button will execute. </param>
        /// <param name="position"> The position of this button. </param>
        public Button(LocalEmoji emoji, ButtonCallback callback, int position = 0)
        {
            if (emoji == null)
                throw new ArgumentNullException(nameof(emoji));

            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            Emoji = emoji;
            Callback = callback;
            Position = position;
        }

        /// <inheritdoc/>
        public override string ToString()
            => $"Button {Emoji}";
    }
}
