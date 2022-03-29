using System;

namespace Disqord
{
    public class LocalSelectionComponentOption : ILocalConstruct
    {
        public Optional<string> Label { get; set; }

        public Optional<string> Value { get; set; }

        public Optional<string> Description { get; set; }

        public Optional<LocalEmoji> Emoji { get; set; }

        public Optional<bool> IsDefault { get; set; }

        public LocalSelectionComponentOption()
        { }

        public LocalSelectionComponentOption(string label, string value)
        {
            Label = label;
            Value = value;
        }

        protected LocalSelectionComponentOption(LocalSelectionComponentOption other)
        {
            Label = other.Label;
            Value = other.Value;
            Description = other.Description;
            Emoji = other.Emoji;
            IsDefault = other.IsDefault;
        }

        public LocalSelectionComponentOption Clone()
            => new(this);

        object ICloneable.Clone()
            => Clone();
    }
}
