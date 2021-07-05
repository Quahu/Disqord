namespace Disqord
{
    public static class LocalSelectionComponentOptionOptionExtensions
    {
        public static TOption WithLabel<TOption>(this TOption component, string label)
            where TOption : LocalSelectionComponentOption
        {
            component.Label = label;
            return component;
        }

        public static TOption WithValue<TOption>(this TOption component, string value)
            where TOption : LocalSelectionComponentOption
        {
            component.Value = value;
            return component;
        }

        public static TOption WithDescription<TOption>(this TOption component, string description)
            where TOption : LocalSelectionComponentOption
        {
            component.Description = description;
            return component;
        }

        public static TOption WithEmoji<TOption>(this TOption component, LocalEmoji emoji)
            where TOption : LocalSelectionComponentOption
        {
            component.Emoji = emoji;
            return component;
        }

        public static TOption WithIsDefault<TOption>(this TOption component, bool isDefault = true)
            where TOption : LocalSelectionComponentOption
        {
            component.IsDefault = isDefault;
            return component;
        }
    }
}
