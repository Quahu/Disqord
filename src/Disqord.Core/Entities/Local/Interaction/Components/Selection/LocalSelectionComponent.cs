using System.Collections.Generic;
using Qommon;

namespace Disqord;

public class LocalSelectionComponent : LocalComponent, ILocalCustomIdentifiableEntity, ILocalConstruct<LocalSelectionComponent>
{
    /// <inheritdoc/>
    public Optional<string> CustomId { get; set; }

    /// <summary>
    ///     Gets or sets the placeholder of this selection.
    /// </summary>
    public Optional<string> Placeholder { get; set; }

    /// <summary>
    ///     Gets or sets the minimum amount of options of this selection.
    /// </summary>
    public Optional<int> MinimumSelectedOptions { get; set; }

    /// <summary>
    ///     Gets or sets the maximum amount of options of this selection.
    /// </summary>
    public Optional<int> MaximumSelectedOptions { get; set; }

    /// <summary>
    ///     Gets or sets whether this selection is disabled.
    /// </summary>
    public Optional<bool> IsDisabled { get; set; }

    /// <summary>
    ///     Gets or sets the options of this selection.
    /// </summary>
    public Optional<IList<LocalSelectionComponentOption>> Options { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalSelectionComponent"/>.
    /// </summary>
    public LocalSelectionComponent()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalSelectionComponent"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalSelectionComponent(LocalSelectionComponent other)
    {
        CustomId = other.CustomId;
        Placeholder = other.Placeholder;
        MinimumSelectedOptions = other.MinimumSelectedOptions;
        MaximumSelectedOptions = other.MaximumSelectedOptions;
        Options = other.Options.DeepClone();
    }

    /// <inheritdoc/>
    public override LocalSelectionComponent Clone()
    {
        return new(this);
    }
}
