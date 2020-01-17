using System;

namespace Disqord.Extensions.Interactivity.Menus
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ButtonAttribute : Attribute
    {
        public IEmoji Emoji { get; }

        public ButtonAttribute(string emoji)
        {
            if (emoji == null)
                throw new ArgumentNullException(nameof(emoji));

            Emoji = LocalCustomEmoji.TryParse(emoji, out var customEmoji)
                ? customEmoji as IEmoji
                : new LocalEmoji(emoji);
        }

        public ButtonAttribute(ulong emojiId, string name = null)
        {
            Emoji = new LocalCustomEmoji(emojiId, name);
        }
    }
}
