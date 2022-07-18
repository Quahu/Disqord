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
}
