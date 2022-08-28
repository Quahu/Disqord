namespace Disqord;

/// <summary>
///     Represents a text input component.
/// </summary>
public interface ITextInputComponent : IComponent, ICustomIdentifiableEntity
{
    /// <summary>
    ///     Gets the style of this text input component.
    /// </summary>
    TextInputComponentStyle Style { get; }

    /// <summary>
    ///     Gets the label of this text input component.
    /// </summary>
    string Label { get; }

    /// <summary>
    ///     Gets the minimum length of text that must be entered in this text input component.
    /// </summary>
    int? MinimumInputLength { get; }

    /// <summary>
    ///     Gets the maximum length of text that must be entered in this text input component.
    /// </summary>
    int? MaximumInputLength { get; }

    /// <summary>
    ///     Gets whether it is required to fill this text input component.
    /// </summary>
    bool IsRequired { get; }

    /// <summary>
    ///     Gets the pre-filled text of this text input component
    ///     or the submitted value when received in a modal submit interaction.
    /// </summary>
    string? Value { get; }

    /// <summary>
    ///     Gets the placeholder text of this text input component.
    /// </summary>
    string? Placeholder { get; }
}
