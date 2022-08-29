using Qommon;

namespace Disqord;

public class LocalTextInputComponent : LocalComponent, ILocalCustomIdentifiableEntity, ILocalConstruct<LocalTextInputComponent>
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

    /// <summary>
    ///     Instantiates a new <see cref="LocalTextInputComponent"/>.
    /// </summary>
    public LocalTextInputComponent()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalTextInputComponent"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
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

    /// <inheritdoc/>
    public override LocalTextInputComponent Clone()
    {
        return new(this);
    }

    /// <summary>
    ///     Converts the specified text input component to a <see cref="LocalTextInputComponent"/>.
    /// </summary>
    /// <param name="textInputComponent"> The text input component to convert. </param>
    /// <returns>
    ///     The output <see cref="LocalTextInputComponent"/>.
    /// </returns>
    public static LocalTextInputComponent CreateFrom(ITextInputComponent textInputComponent)
    {
        return new LocalTextInputComponent
        {
            Style = textInputComponent.Style,
            CustomId = textInputComponent.CustomId,
            Label = textInputComponent.Label,
            MinimumInputLength = Optional.FromNullable(textInputComponent.MinimumInputLength),
            MaximumInputLength = Optional.FromNullable(textInputComponent.MaximumInputLength),
            IsRequired = textInputComponent.IsRequired,
            PrefilledValue = Optional.FromNullable(textInputComponent.Value),
            Placeholder = Optional.FromNullable(textInputComponent.Placeholder)
        };
    }
}
