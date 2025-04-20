using Qommon;

namespace Disqord;

public class LocalSeparatorComponent : LocalComponent, ILocalConstruct<LocalSeparatorComponent>
{
    /// <summary>
    ///     Gets or sets whether this separator is a divider.
    /// </summary>
    public Optional<bool> IsDivider { get; set; }

    /// <summary>
    ///     Gets or sets the spacing size of this separator.
    /// </summary>
    public Optional<SeparatorComponentSpacingSize> SpacingSize { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalSeparatorComponent"/>.
    /// </summary>
    public LocalSeparatorComponent()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalSeparatorComponent"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalSeparatorComponent(LocalSeparatorComponent other)
    {
        IsDivider = other.IsDivider;
        SpacingSize = other.SpacingSize;
    }

    /// <inheritdoc/>
    public override LocalSeparatorComponent Clone()
    {
        return new(this);
    }

    public static LocalSeparatorComponent CreateFrom(ISeparatorComponent separatorComponent)
    {
        return new LocalSeparatorComponent
        {
            Id = separatorComponent.Id,
            IsDivider = separatorComponent.IsDivider,
            SpacingSize = separatorComponent.SpacingSize
        };
    }
}
