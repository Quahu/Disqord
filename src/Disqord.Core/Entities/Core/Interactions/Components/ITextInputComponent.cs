namespace Disqord
{
    /// <summary>
    ///     Represents a text input component.
    /// </summary>
    public interface ITextInputComponent : IComponent, ICustomIdentifiableEntity
    {
        /// <summary>
        ///     Gets the style of this text input component.
        /// </summary>
        TextInputComponentStyle ComponentStyle { get; }

        /// <summary>
        ///     Gets the label of this text input component.
        /// </summary>
        string Label { get; }

        /// <summary>
        ///     Gets the minimum length of text that must be entered in this text input component.
        /// </summary>
        int? MinimumLength { get; }

        /// <summary>
        ///     Gets the maximum length of text that must be entered in this text input component.
        /// </summary>
        int? MaximumLength { get; }

        /// <summary>
        ///     Gets whether it is required to fill this text input component.
        /// </summary>
        bool IsRequired { get; }

        /// <summary>
        ///     Gets the pre-filled text of this text input component
        ///     or the submitted value for a text input component when received in a modal submit interaction.
        ///     Returns <see langword="null"/> if not set.
        /// </summary>
        string Value { get; }

        /// <summary>
        ///     Gets the placeholder text of this text input component.
        ///     Returns <see langword="null"/> if not set.
        /// </summary>
        string Placeholder { get; }
    }
}
