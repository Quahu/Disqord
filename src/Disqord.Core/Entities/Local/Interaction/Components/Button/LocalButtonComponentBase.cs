using Qommon;

namespace Disqord;

public abstract class LocalButtonComponentBase : LocalComponent, ILocalConstruct<LocalButtonComponentBase>
{
    /// <summary>
    ///     Gets or sets the label of this button.
    /// </summary>
    /// <remarks>
    ///     This property is required if <see cref="Emoji"/> is not set.
    /// </remarks>
    public Optional<string> Label { get; set; }

    /// <summary>
    ///     Gets or sets the emoji of this button.
    /// </summary>
    /// <remarks>
    ///     This property is required if <see cref="Label"/> is not set.
    /// </remarks>
    public Optional<LocalEmoji> Emoji { get; set; }

    /// <summary>
    ///     Gets or sets whether this button is disabled.
    /// </summary>
    public Optional<bool> IsDisabled { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalButtonComponentBase"/>.
    /// </summary>
    protected LocalButtonComponentBase()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalButtonComponentBase"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalButtonComponentBase(LocalButtonComponentBase other)
    {
        Label = other.Label;
        Emoji = other.Emoji;
    }

    /// <inheritdoc/>
    public abstract override LocalButtonComponentBase Clone();

    /// <summary>
    ///     Converts the specified button component to a <see cref="LocalButtonComponentBase"/>.
    /// </summary>
    /// <param name="buttonComponent"> The button component to convert. </param>
    /// <returns>
    ///     The output <see cref="LocalButtonComponentBase"/>.
    /// </returns>
    public static LocalButtonComponentBase CreateFrom(IButtonComponent buttonComponent)
    {
        return buttonComponent.Style == ButtonComponentStyle.Link
            ? LocalLinkButtonComponent.CreateFrom(buttonComponent)
            : LocalButtonComponent.CreateFrom(buttonComponent);
    }

    protected static void PopulateFrom(LocalButtonComponentBase localButtonComponent, IButtonComponent buttonComponent)
    {
        localButtonComponent.Label = Optional.FromNullable(buttonComponent.Label);
        localButtonComponent.Emoji = Optional.Conditional(buttonComponent.Emoji != null, LocalEmoji.FromEmoji, buttonComponent.Emoji)!;
        localButtonComponent.IsDisabled = buttonComponent.IsDisabled;
    }
}
