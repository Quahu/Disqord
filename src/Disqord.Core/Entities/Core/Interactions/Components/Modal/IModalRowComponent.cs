using System.Collections.Generic;

namespace Disqord;

/// <inheritdoc/>
/// <remarks>
///     This is the modal-equivalent of <see cref="IRowComponent"/>.
/// </remarks>
public interface IModalRowComponent : IModalComponent
{
    /// <summary>
    ///     Gets the child components of this row component.
    /// </summary>
    IReadOnlyList<IModalComponent> Components { get; }
}
