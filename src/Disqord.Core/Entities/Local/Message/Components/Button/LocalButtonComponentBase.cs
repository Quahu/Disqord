using System;

namespace Disqord
{
    public abstract class LocalButtonComponentBase : LocalNestedComponent
    {
        public const int MaxLabelLength = 80;

        public string Label
        {
            get => _label;
            set
            {
                if (value != null && value.Length > MaxLabelLength)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The button's label must not be longer than {MaxLabelLength} characters.");

                _label = value;
            }
        }
        private string _label;

        public LocalEmoji Emoji { get; set; }

        protected LocalButtonComponentBase()
        { }

        protected LocalButtonComponentBase(LocalButtonComponentBase other)
        {
            _label = other._label;
            Emoji = other.Emoji;
        }

        public override void Validate()
        {
            if (string.IsNullOrWhiteSpace(Label) && Emoji == null)
                throw new InvalidOperationException("The button's label or emoji must be set.");
        }
    }
}
