using Qommon;

namespace Disqord
{
    public class LocalTextInputComponent : LocalComponent, ILocalCustomIdentifiableEntity
    {
        /// <summary>
        ///     Gets or sets the style of this text input.
        /// </summary>
        /// <remarks>
        ///     This property is required.
        /// </remarks>
        public Optional<TextInputComponentStyle> Style { get; set; }

        /// <inheritdoc/>
        public Optional<string> CustomId { get; set; }

        /// <summary>
        ///     Gets or sets the label of this text input.
        /// </summary>
        /// <remarks>
        ///     This property is required.
        /// </remarks>
        public Optional<string> Label { get; set; }

        /// <summary>
        ///     Gets or sets the minimum input length of this text input.
        /// </summary>
        public Optional<int> MinimumInputLength { get; set; }

        /// <summary>
        ///     Gets or sets the maximum input length of this text input.
        /// </summary>
        public Optional<int> MaximumInputLength { get; set; }

        /// <summary>
        ///     Gets or sets whether this text input is required.
        /// </summary>
        public Optional<bool> IsRequired { get; set; }

        /// <summary>
        ///     Gets or sets the prefilled value of this text input.
        /// </summary>
        public Optional<string> PrefilledValue { get; set; }

        /// <summary>
        ///     Gets or sets the placeholder of this text input.
        /// </summary>
        public Optional<string> Placeholder { get; set; }

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
    }
}
