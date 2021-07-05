using System;

namespace Disqord
{
    public class LocalSelectionComponentOption : ILocalConstruct
    {
        public const int MaxLabelLength = 25;

        public const int MaxValueLength = 100;

        public const int MaxDescriptionLength = 50;

        public string Label
        {
            get => _label;
            set
            {
                if (value != null && value.Length > MaxLabelLength)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The selection option's label must not be longer than {MaxLabelLength} characters.");

                _label = value;
            }
        }
        private string _label;

        public string Value
        {
            get => _value;
            set
            {
                if (value != null && value.Length > MaxValueLength)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The selection option's value must not be longer than {MaxValueLength} characters.");

                _value = value;
            }
        }
        private string _value;

        public string Description
        {
            get => _description;
            set
            {
                if (value != null && value.Length > MaxDescriptionLength)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The selection option's value must not be longer than {MaxDescriptionLength} characters.");

                _description = value;
            }
        }
        private string _description;

        public LocalEmoji Emoji { get; set; }

        public bool IsDefault { get; set; }

        public LocalSelectionComponentOption()
        { }

        public LocalSelectionComponentOption(string label, string value)
        {
            Label = label;
            Value = value;
        }

        protected LocalSelectionComponentOption(LocalSelectionComponentOption other)
        {
            _label = other._label;
            _value = other._value;
            _description = other._description;
            Emoji = other.Emoji;
            IsDefault = other.IsDefault;
        }

        public LocalSelectionComponentOption Clone()
            => new(this);

        object ICloneable.Clone()
            => Clone();

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(_label))
                throw new InvalidOperationException("The selection option's label must be set.");

            if (string.IsNullOrWhiteSpace(_value))
                throw new InvalidOperationException("The selection option's value must be set.");
        }
    }
}
