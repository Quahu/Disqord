namespace Disqord;

/// <inheritdoc/>
/// <remarks>
///     This is the modal-equivalent of <see cref="ILabelComponent"/>.
/// </remarks>
public interface IModalLabelComponent : IModalComponent
{
    /// <summary>
    ///     Gets the child component of this label component.
    /// </summary>
    IModalComponent Component { get; }
}
