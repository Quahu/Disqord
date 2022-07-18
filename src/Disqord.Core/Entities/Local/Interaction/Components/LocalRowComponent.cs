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
}
