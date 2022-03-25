using System;

namespace Disqord
{
    public class LocalTextInputComponent : LocalNestedComponent, ILocalInteractiveComponent
    {
        public const int MinLabelLength = 1;

        public const int MaxLabelLength = 40;

        public const int MinMinimumInputLength = 0;

        public const int MaxMinimumInputLength = 4000;

        public const int MinMaximumInputLength = 1;

        public const int MaxMaximumInputLength = 4000;

        public const int MaxPrefilledValueLength = 4000;

        public const int MaxPlaceholderLength = 100;

        /// <summary>
        ///     Gets or sets the style of this text input.
        /// </summary>
        public TextInputComponentStyle Style { get; set; } = TextInputComponentStyle.Short;

        /// <inheritdoc/>
        public string CustomId
        {
            get => _customId;
            set
            {
                if (value != null && value.Length > ILocalInteractiveComponent.MaxCustomIdLength)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The text input's custom ID must not be longer than {ILocalInteractiveComponent.MaxCustomIdLength} characters.");

                _customId = value;
            }
        }
        private string _customId;

        /// <summary>
        ///     Gets or sets the label of this text input.
        /// </summary>
        public string Label
        {
            get => _label;
            set
            {
                if (value != null && (value.Length < MinLabelLength || value.Length > MaxLabelLength))
                    throw new ArgumentOutOfRangeException(nameof(value), $"The text input's label must be between {MinLabelLength} and {MaxLabelLength} characters.");

                _label = value;
            }
        }
        private string _label;

        /// <summary>
        ///     Gets or sets the minimum input length of this text input.
        /// </summary>
        public Optional<int> MinimumInputLength
        {
            get => _minimumInputLength;
            set
            {
                if (value.HasValue && (value.Value < MinMinimumInputLength || value.Value > MaxMinimumInputLength))
                    throw new ArgumentOutOfRangeException(nameof(value), $"The text input's minimum input length must be between {MinMinimumInputLength} and {MaxMinimumInputLength} characters.");

                _minimumInputLength = value;
            }
        }
        private Optional<int> _minimumInputLength;

        /// <summary>
        ///     Gets or sets the maximum input length of this text input.
        /// </summary>
        public Optional<int> MaximumInputLength
        {
            get => _maximumInputLength;
            set
            {
                if (value.HasValue && (value.Value < MinMaximumInputLength || value.Value > MaxMaximumInputLength))
                    throw new ArgumentOutOfRangeException(nameof(value), $"The text input's maximum input length must be between {MinMaximumInputLength} and {MaxMaximumInputLength} characters.");

                _maximumInputLength = value;
            }
        }
        private Optional<int> _maximumInputLength;

        /// <summary>
        ///     Gets or sets whether this text input is required.
        /// </summary>
        public Optional<bool> IsRequired { get; set; }

        /// <summary>
        ///     Gets or sets the prefilled value of this text input.
        /// </summary>
        public Optional<string> PrefilledValue
        {
            get => _prefilledValue;
            set
            {
                if (value.HasValue && value.Value.Length > MaxPrefilledValueLength)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The text input's prefilled value must not be longer than {MaxPrefilledValueLength} characters.");

                _prefilledValue = value;
            }
        }
        private Optional<string> _prefilledValue;

        /// <summary>
        ///     Gets or sets the placeholder of this text input.
        /// </summary>
        public Optional<string> Placeholder
        {
            get => _placeholder;
            set
            {
                if (value.HasValue && value.Value.Length > MaxPlaceholderLength)
                    throw new ArgumentOutOfRangeException(nameof(value), $"The text input's placeholder must not be longer than {MaxPlaceholderLength} characters.");

                _placeholder = value;
            }
        }
        private Optional<string> _placeholder;

        public LocalTextInputComponent()
        { }

        protected LocalTextInputComponent(LocalTextInputComponent other)
        {
            Style = other.Style;
            CustomId = other.CustomId;
            Label = other.Label;
            MinimumInputLength = other.MinimumInputLength;
            MaximumInputLength = other.MaximumInputLength;
            IsRequired = other.IsRequired;
            PrefilledValue = other.PrefilledValue;
            Placeholder = other.Placeholder;
        }

        public override LocalTextInputComponent Clone()
            => new(this);

        public override void Validate()
        {
            if (string.IsNullOrWhiteSpace(CustomId))
                throw new InvalidOperationException("The text input's custom ID must be set.");

            if (string.IsNullOrWhiteSpace(Label))
                throw new InvalidOperationException("The text input's custom ID must be set.");

            if ((MinimumInputLength.HasValue && MaximumInputLength.HasValue) && MinimumInputLength.Value > MaximumInputLength.Value)
                throw new InvalidOperationException("The text input's minimum input length cannot be greater than the maximum input length.");
        }
    }
}
