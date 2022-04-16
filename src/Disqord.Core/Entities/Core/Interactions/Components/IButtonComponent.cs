namespace Disqord
{
    /// <summary>
    ///     Represents a button message component.
    /// </summary>
    public interface IButtonComponent : IComponent, ICustomIdentifiableEntity
    {
        /// <summary>
        ///     Gets the style of this button component.
        /// </summary>
        ButtonComponentStyle Style { get; }

        /// <summary>
        ///     Gets the label of this button component.
        /// </summary>
        string Label { get; }

        /// <summary>
        ///     Gets the emoji of this button component.
        ///     Returns <see langword="null"/> if the button has no emoji.
        /// </summary>
        IEmoji Emoji { get; }

        /// <summary>
        ///     Gets the URL of this button component.
        ///     Returns <see langword="null"/> if the button is not a <see cref="ButtonComponentStyle.Link"/>.
        /// </summary>
        string Url { get; }

        /// <summary>
        ///     Gets whether this button component is disabled.
        /// </summary>
        bool IsDisabled { get; }
    }
}
