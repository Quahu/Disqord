using System;

namespace Disqord
{
    public abstract class LocalComponent : ILocalConstruct
    {
        public static LocalRowComponent Row(params LocalComponent[] components)
            => new()
            {
                Components = components
            };

        public static LocalButtonComponent Button(string label, string customId, ButtonComponentStyle style = ButtonComponentStyle.Primary, LocalEmoji emoji = null, bool isDisabled = false)
            => new()
            {
                Label = label,
                CustomId = customId,
                Style = style,
                Emoji = emoji,
                IsDisabled = isDisabled
            };

        public static LocalButtonComponent Button(LocalEmoji emoji, string customId, ButtonComponentStyle style = ButtonComponentStyle.Primary, bool isDisabled = false)
            => new()
            {
                CustomId = customId,
                Style = style,
                Emoji = emoji,
                IsDisabled = isDisabled
            };

        public static LocalButtonComponent LinkButton(string label, string url, LocalEmoji emoji = null, bool isDisabled = false)
            => new()
            {
                Label = label,
                Url = url,
                Style = ButtonComponentStyle.Link,
                Emoji = emoji,
                IsDisabled = isDisabled
            };

        public static LocalButtonComponent LinkButton(LocalEmoji emoji, string url, bool isDisabled = false)
            => new()
            {
                Url = url,
                Style = ButtonComponentStyle.Link,
                Emoji = emoji,
                IsDisabled = isDisabled
            };

        public abstract LocalComponent Clone();

        object ICloneable.Clone()
            => Clone();

        public abstract void Validate();
    }
}
