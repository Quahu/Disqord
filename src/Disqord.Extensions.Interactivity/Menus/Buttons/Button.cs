using System;
using System.Threading.Tasks;

namespace Disqord.Extensions.Interactivity.Menus
{
    public delegate Task ButtonCallback(ButtonEventArgs e);

    public sealed class Button
    {
        public IEmoji Emoji { get; }

        public ButtonCallback Callback { get; }

        public int Position { get; }

        public Button(IEmoji emoji, ButtonCallback callback, int position = int.MaxValue)
        {
            if (emoji == null)
                throw new ArgumentNullException(nameof(emoji));

            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            Emoji = emoji;
            Callback = callback;
            Position = position;
        }

        public override string ToString()
            => Emoji.ToString();
    }
}
