using System;

namespace Disqord
{
    public class LocalButtonComponent : LocalComponent
    {
        public const int MAX_LABEL_LENGTH = 80;

        public ButtonComponentStyle Style { get; set; }

        public string Label
        {
            get => _label;
            set
            {
                if (value != null && value.Length > MAX_LABEL_LENGTH)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The button's label must not be longer than {MAX_LABEL_LENGTH} characters.");

                _label = value;
            }
        }
        private string _label;

        public LocalEmoji Emoji { get; set; }

        public string CustomId { get; set; }

        public string Url { get; set; }

        public bool IsDisabled { get; set; }

        public LocalButtonComponent()
        { }

        private LocalButtonComponent(LocalButtonComponent other)
        {
            Style = other.Style;
            _label = other.Label;
            Emoji = other.Emoji?.Clone();
            CustomId = other.CustomId;
            Url = other.Url;
            IsDisabled = other.IsDisabled;
        }

        public override LocalButtonComponent Clone()
            => new(this);

        public override void Validate()
        {
            if (Style == ButtonComponentStyle.Link)
            {
                if (string.IsNullOrWhiteSpace(Url))
                    throw new InvalidOperationException("The link button's URL must be set.");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(CustomId))
                    throw new InvalidOperationException("The button's custom ID must be set.");
            }

            if (string.IsNullOrWhiteSpace(Label))
                throw new InvalidOperationException("The button's label ID must be set.");
        }
    }
}
