using System.Collections.Generic;
using Qommon;

namespace Disqord;

public class LocalRowComponent : LocalComponent, ILocalConstruct<LocalRowComponent>
{
    /// <summary>
    ///     Gets or sets the components of this row.
    /// </summary>
    public Optional<IList<LocalComponent>> Components { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalRowComponent"/>.
    /// </summary>
    public LocalRowComponent()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalRowComponent"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalRowComponent(LocalRowComponent other)
    {
        Components = other.Components.DeepClone();
    }

    /// <inheritdoc/>
    public override LocalRowComponent Clone()
    {
        return new(this);
    }

    /// <summary>
    ///     Converts the specified row component to a <see cref="LocalRowComponent"/>.
    /// </summary>
    /// <param name="rowComponent"> The row component to convert. </param>
    /// <returns>
    ///     The output <see cref="LocalRowComponent"/>.
    /// </returns>
    public static LocalRowComponent CreateFrom(IRowComponent rowComponent)
    {
        var components = rowComponent.Components;
        var componentCount = components.Count;
        var localComponents = new List<LocalComponent>(componentCount);
        for (var i = 0; i < componentCount; i++)
        {
            var component = components[i];
            localComponents.Add(CreateFrom(component));
        }

        return new LocalRowComponent
        {
            Components = localComponents
        };
    }
}
