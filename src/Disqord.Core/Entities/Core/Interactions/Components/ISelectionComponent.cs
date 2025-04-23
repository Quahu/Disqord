using System.Collections.Generic;

namespace Disqord;

/// <summary>
///     Represents a selection message component.
/// </summary>
public interface ISelectionComponent : IComponent, ICustomIdentifiableEntity
{
    /// <summary>
    ///     Gets the type of this selection component.
    /// </summary>
    new SelectionComponentType Type { get; }

    /// <summary>
    ///     Gets the channel types of this selection component.
    /// </summary>
    /// <remarks>
    ///     This is only valid for channel entity selection components.
    /// </remarks>
    IReadOnlyList<ChannelType> ChannelTypes { get; }

    /// <summary>
    ///     Gets the placeholder text of this selection component.
    ///     Returns <see langword="null"/> if not set.
    /// </summary>
    string? Placeholder { get; }

    /// <summary>
    ///     Gets the default values that auto-populate this selection component.
    /// </summary>
    /// <remarks>
    ///     This is only valid for <see cref="SelectionComponentType.User"/>,
    ///     <see cref="SelectionComponentType.Role"/>, or <see cref="SelectionComponentType.Channel"/> selections.
    /// </remarks>
    IReadOnlyList<IDefaultSelectionValue> DefaultValues { get; }

    /// <summary>
    ///     Gets the minimum amount of options that must be selected at once of this selection component.
    /// </summary>
    int MinimumSelectedOptions { get; }

    /// <summary>
    ///     Gets the maximum amount of options that can be selected at once of this selection component.
    /// </summary>
    int MaximumSelectedOptions { get; }

    /// <summary>
    ///     Gets the selectable options of this selection component.
    /// </summary>
    /// <remarks>
    ///     This is only valid for <see cref="SelectionComponentType.String"/> selection components.
    /// </remarks>
    IReadOnlyList<ISelectionComponentOption> Options { get; }

    /// <summary>
    ///     Gets whether this selection component is disabled.
    /// </summary>
    bool IsDisabled { get; }
}
