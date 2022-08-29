using Qommon;

namespace Disqord;

public class LocalButtonComponent : LocalButtonComponentBase, ILocalCustomIdentifiableEntity, ILocalConstruct<LocalButtonComponent>
{
    /// <summary>
    ///     Gets or sets the style of this button.
    /// </summary>
    /// <remarks>
    ///     This property is required.
    /// </remarks>
    public Optional<LocalButtonComponentStyle> Style { get; set; } = LocalButtonComponentStyle.Primary;

    /// <inheritdoc/>
    public Optional<string> CustomId { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalButtonComponent"/>.
    /// </summary>
    public LocalButtonComponent()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalButtonComponent"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalButtonComponent(LocalButtonComponent other)
        : base(other)
    {
        Style = other.Style;
        CustomId = other.CustomId;
    }

    /// <inheritdoc/>
    public override LocalButtonComponent Clone()
    {
        return new(this);
    }

    /// <summary>
    ///     Converts the specified button component to a <see cref="LocalButtonComponent"/>.
    /// </summary>
    /// <remarks>
    ///     The button component must not be a link button.
    /// </remarks>
    /// <param name="buttonComponent"> The button component to convert. </param>
    /// <returns>
    ///     The output <see cref="LocalButtonComponent"/>.
    /// </returns>
    public new static LocalButtonComponent CreateFrom(IButtonComponent buttonComponent)
    {
        Guard.IsTrue(buttonComponent.Style != ButtonComponentStyle.Link);

        var localButtonComponent = new LocalButtonComponent
        {
            Style = (LocalButtonComponentStyle) buttonComponent.Style,
            CustomId = buttonComponent.CustomId
        };

        PopulateFrom(localButtonComponent, buttonComponent);
        return localButtonComponent;
    }
}
