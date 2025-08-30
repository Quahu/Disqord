using System.Collections.Generic;
using System.Linq;
using Qommon;

namespace Disqord;

public class LocalSelectionComponent : LocalComponent, ILocalCustomIdentifiableEntity, ILocalConstruct<LocalSelectionComponent>
{
    /// <inheritdoc/>
    public Optional<string> CustomId { get; set; }

    /// <summary>
    ///     Gets or sets the type of this selection.
    /// </summary>
    /// <remarks>
    ///     Defaults to <see cref="SelectionComponentType.String"/>.
    /// </remarks>
    public SelectionComponentType Type { get; set; } = SelectionComponentType.String;

    /// <summary>
    ///     Gets or sets the channel types available in this selection.
    /// </summary>
    /// <remarks>
    ///     This is only valid for channel entity selection components.
    /// </remarks>
    public Optional<IList<ChannelType>> ChannelTypes { get; set; }

    /// <summary>
    ///     Gets or sets the placeholder of this selection.
    /// </summary>
    public Optional<string> Placeholder { get; set; }

    /// <summary>
    ///     Gets or sets the default values of this selection.
    /// </summary>
    /// <remarks>
    ///     This is only valid for <see cref="SelectionComponentType.User"/>,
    ///     <see cref="SelectionComponentType.Role"/>, or <see cref="SelectionComponentType.Channel"/> selections.
    /// </remarks>
    public Optional<IList<LocalDefaultSelectionValue>> DefaultValues { get; set; }

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
    ///     Gets or sets the pre-defined options of this selection.
    /// </summary>
    /// <remarks>
    ///     This is only valid for <see cref="SelectionComponentType.String"/> selections.
    /// </remarks>
    public Optional<IList<LocalSelectionComponentOption>> Options { get; set; }

    /// <summary>
    ///     Gets or sets whether this selection is required.
    /// </summary>
    /// <remarks>
    ///     This is only used when the selection component is a part of a modal.
    /// </remarks>
    public Optional<bool> IsRequired { get; set; }

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
        Type = other.Type;
        ChannelTypes = other.ChannelTypes.Clone();
        Placeholder = other.Placeholder;
        DefaultValues = other.DefaultValues.DeepClone();
        MinimumSelectedOptions = other.MinimumSelectedOptions;
        MaximumSelectedOptions = other.MaximumSelectedOptions;
        Options = other.Options.DeepClone();
        IsRequired = other.IsRequired;
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
        var selection = new LocalSelectionComponent
        {
            CustomId = selectionComponent.CustomId,
            Type = selectionComponent.Type,
            ChannelTypes = selectionComponent.ChannelTypes.ToArray(),
            MinimumSelectedOptions = selectionComponent.MinimumSelectedOptions,
            MaximumSelectedOptions = selectionComponent.MaximumSelectedOptions,
            IsDisabled = selectionComponent.IsDisabled,
            IsRequired = selectionComponent.IsRequired
        };

        if (selectionComponent.Type == SelectionComponentType.String)
        {
            var options = selectionComponent.Options;
            var optionCount = options.Count;
            var localOptions = new List<LocalSelectionComponentOption>(optionCount);
            for (var i = 0; i < optionCount; i++)
            {
                var option = options[i];
                localOptions.Add(LocalSelectionComponentOption.CreateFrom(option));
            }

            selection.Options = localOptions;
        }
        else
        {
            var defaultValues = selectionComponent.DefaultValues;
            var defaultValueCount = defaultValues.Count;
            var localDefaultValues = new List<LocalDefaultSelectionValue>(defaultValueCount);
            for (var i = 0; i < defaultValueCount; i++)
            {
                var defaultValue = defaultValues[i];
                localDefaultValues.Add(new(defaultValue.Id, defaultValue.Type));
            }

            selection.DefaultValues = localDefaultValues;
        }

        return selection;
    }
}
