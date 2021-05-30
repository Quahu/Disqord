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

        public static LocalButtonComponent Button(string customId, string label, ButtonComponentStyle style = ButtonComponentStyle.Primary, LocalEmoji emoji = null, bool isDisabled = false)
            => new()
            {
                CustomId = customId,
                Label = label,
                Style = style,
                Emoji = emoji,
                IsDisabled = isDisabled
            };

        public static LocalButtonComponent LinkButton(string url, string label, LocalEmoji emoji = null, bool isDisabled = false)
            => new()
            {
                Label = label,
                Url = url,
                Style = ButtonComponentStyle.Link,
                Emoji = emoji,
                IsDisabled = isDisabled
            };

        public virtual LocalComponent Clone()
            => MemberwiseClone() as LocalComponent;

        object ICloneable.Clone()
            => Clone();

        public abstract void Validate();
    }
}
