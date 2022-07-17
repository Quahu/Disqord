using System.Collections.Generic;
using Qommon;

namespace Disqord;

public class LocalSelectionComponent : LocalComponent, ILocalCustomIdentifiableEntity
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

    public LocalSelectionComponent()
    { }

    protected LocalSelectionComponent(LocalSelectionComponent other)
    {
        CustomId = other.CustomId;
        Placeholder = other.Placeholder;
        MinimumSelectedOptions = other.MinimumSelectedOptions;
        MaximumSelectedOptions = other.MaximumSelectedOptions;
        Options = other.Options.DeepClone();
    }

    public override LocalSelectionComponent Clone()
    {
        return new(this);
    }
}
