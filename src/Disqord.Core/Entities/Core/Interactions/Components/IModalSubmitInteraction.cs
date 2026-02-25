using System.Collections.Generic;

namespace Disqord;

/// <summary>
///     Represents a modal being submitted.
/// </summary>
public interface IModalSubmitInteraction : IEntityInteraction, ICustomIdentifiableEntity
{
    /// <summary>
    ///     Gets the components of this interaction.
    /// </summary>
    IReadOnlyList<IModalComponent> Components { get; }
}
