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

        public static LocalButtonComponent Button(string customId, string label)
            => new()
            {
                CustomId = customId,
                Label = label
            };

        public static LocalButtonComponent Button(string customId, LocalEmoji emoji)
            => new()
            {
                CustomId = customId,
                Emoji = emoji
            };

        public static LocalLinkButtonComponent LinkButton(string url, string label)
            => new()
            {
                Url = url,
                Label = label
            };

        public static LocalLinkButtonComponent LinkButton(string url, LocalEmoji emoji)
            => new()
            {
                Url = url,
                Emoji = emoji
            };

        public static LocalSelectionComponent Selection(string customId, params LocalSelectionComponentOption[] options)
            => new()
            {
                CustomId = customId,
                Options = options
            };

        public static LocalTextInputComponent TextInput(string customId, string label)
            => new()
            {
                CustomId = customId,
                Label = label
            };

        public abstract LocalComponent Clone();

        object ICloneable.Clone()
            => Clone();
    }
}
