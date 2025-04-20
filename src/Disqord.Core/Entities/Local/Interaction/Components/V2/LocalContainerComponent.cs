using System.Collections.Generic;
using System.Linq;
using Qommon;

namespace Disqord;

public class LocalContainerComponent : LocalComponent, ILocalSpoilerable, ILocalComponentContainer, ILocalConstruct<LocalContainerComponent>
{
    /// <summary>
    ///     Gets or sets the accent color of this container.
    /// </summary>
    public Optional<Color?> AccentColor { get; set; }

    /// <summary>
    ///     Gets or sets whether this container is a spoiler.
    /// </summary>
    public Optional<bool> IsSpoiler { get; set; }

    /// <summary>
    ///     Gets or sets the components of this container.
    /// </summary>
    public Optional<IList<LocalComponent>> Components { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalContainerComponent"/>.
    /// </summary>
    public LocalContainerComponent()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalContainerComponent"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalContainerComponent(LocalContainerComponent other)
    {
        AccentColor = other.AccentColor;
        IsSpoiler = other.IsSpoiler;
        Components = other.Components.DeepClone();
    }

    /// <inheritdoc/>
    public override LocalContainerComponent Clone()
    {
        return new(this);
    }

    public static LocalContainerComponent CreateFrom(IContainerComponent containerComponent)
    {
        return new LocalContainerComponent
        {
            Id = containerComponent.Id,
            AccentColor = containerComponent.AccentColor,
            Components = containerComponent.Components.Select(CreateFrom).ToArray(),
            IsSpoiler = containerComponent.IsSpoiler
        };
    }
}
