using System.Collections.Generic;

namespace Disqord;

/// <summary>
///     Represents a selection component interaction.
/// </summary>
public interface ISelectionComponentInteraction : IComponentInteraction
{
    /// <summary>
    ///     Gets the type of the selection component.
    /// </summary>
    new SelectionComponentType ComponentType { get; }

    /// <summary>
    ///     Gets the selected values of this interaction.
    /// </summary>
    /// <remarks>
    ///     For entity selection components the returned <see cref="string"/>s
    ///     are the unparsed IDs of the entities.
    /// </remarks>
    IReadOnlyList<string> SelectedValues { get; }
}
