using System.Collections.Generic;
using System.Linq;
using Qommon;

namespace Disqord;

public class LocalSectionComponent : LocalComponent, ILocalComponentContainer, ILocalConstruct<LocalSectionComponent>
{
    /// <summary>
    ///     Gets or sets the components of this section.
    /// </summary>
    public Optional<IList<LocalComponent>> Components { get; set; }

    /// <summary>
    ///     Gets or sets the accessory of this section.
    /// </summary>
    public Optional<LocalComponent> Accessory { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalSectionComponent"/>.
    /// </summary>
    public LocalSectionComponent()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalSectionComponent"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalSectionComponent(LocalSectionComponent other)
    {
        Components = other.Components.DeepClone();
        Accessory = other.Accessory.Clone();
    }

    /// <inheritdoc/>
    public override LocalSectionComponent Clone()
    {
        return new(this);
    }

    public static LocalSectionComponent CreateFrom(ISectionComponent sectionComponent)
    {
        return new LocalSectionComponent
        {
            Id = sectionComponent.Id,
            Components = sectionComponent.Components.Select(CreateFrom).ToArray(),
            Accessory = CreateFrom(sectionComponent.Accessory)
        };
    }
}
