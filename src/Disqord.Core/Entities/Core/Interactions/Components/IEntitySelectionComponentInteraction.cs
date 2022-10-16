using System.Collections.Generic;

namespace Disqord;

/// <summary>
///     Represents an entity selection component interaction.
/// </summary>
public interface IEntitySelectionComponentInteraction : ISelectionComponentInteraction, IEntityInteraction
{
    /// <summary>
    ///     Gets the IDs of the selected entities of this interaction.
    /// </summary>
    new IReadOnlyList<Snowflake> SelectedValues { get; }
}
