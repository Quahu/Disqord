using System.Collections.Generic;

namespace Disqord;

/// <inheritdoc cref="IModalComponent"/>
/// <remarks>
///     This is the modal-equivalent of <see cref="ISelectionComponent"/>.
/// </remarks>
public interface IModalSelectionComponent : IModalComponent, ICustomIdentifiableEntity
{
    /// <summary>
    ///     Gets the selected values submitted in the modal.
    /// </summary>
    IReadOnlyList<string> Values { get; }
}
