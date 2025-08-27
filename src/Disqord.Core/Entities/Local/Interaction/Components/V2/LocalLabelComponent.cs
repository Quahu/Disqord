using Qommon;

namespace Disqord;

public class LocalLabelComponent : LocalComponent, ILocalConstruct<LocalLabelComponent>
{
    /// <summary>
    ///     gets or sets the label of this label component.
    /// </summary>
    public new Optional<string> Label { get; set; }

    /// <summary>
    ///     gets or sets the description of this label component.
    /// </summary>
    public Optional<string> Description { get; set; }

    /// <summary>
    ///     Gets or sets the component of label component.
    /// </summary>
    public Optional<LocalComponent> Component { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalLabelComponent"/>.
    /// </summary>
    public LocalLabelComponent()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalLabelComponent"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalLabelComponent(LocalLabelComponent other)
    {
        Label = other.Label;
        Description = other.Description;
    }

    /// <inheritdoc/>
    public override LocalLabelComponent Clone()
    {
        return new(this);
    }

    public static LocalLabelComponent CreateFrom(ILabelComponent labelComponent)
    {
        return new LocalLabelComponent
        {
            Id = labelComponent.Id,
            Label = labelComponent.Label,
            Description = Optional.FromNullable(labelComponent.Description),
            Component = CreateFrom(labelComponent.Component)
        };
    }
}
