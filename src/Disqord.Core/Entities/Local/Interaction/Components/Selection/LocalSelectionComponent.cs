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

    /// <summary>
    ///     Converts the specified selection component to a <see cref="LocalSelectionComponent"/>.
    /// </summary>
    /// <param name="selectionComponent"> The selection component to convert. </param>
    /// <returns>
    ///     The output <see cref="LocalSelectionComponent"/>.
    /// </returns>
    public static LocalSelectionComponent CreateFrom(ISelectionComponent selectionComponent)
    {
        var options = selectionComponent.Options;
        var optionCount = options.Count;
        var localOptions = new List<LocalSelectionComponentOption>(optionCount);
        for (var i = 0; i < optionCount; i++)
        {
            var option = options[i];
            localOptions.Add(LocalSelectionComponentOption.CreateFrom(option));
        }

        return new LocalSelectionComponent
        {
            CustomId = selectionComponent.CustomId,
            MinimumSelectedOptions = selectionComponent.MinimumSelectedOptions,
            MaximumSelectedOptions = selectionComponent.MaximumSelectedOptions,
            IsDisabled = selectionComponent.IsDisabled,
            Options = localOptions
        };
    }
}
