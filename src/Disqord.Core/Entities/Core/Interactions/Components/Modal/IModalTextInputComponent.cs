namespace Disqord;

/// <inheritdoc cref="IModalComponent"/>
/// <remarks>
///     This is the modal-equivalent of <see cref="ITextInputComponent"/>.
/// </remarks>
public interface IModalTextInputComponent : IModalComponent, ICustomIdentifiableEntity
{
    /// <summary>
    ///     Gets the value submitted in the modal.
    /// </summary>
    string Value { get; }
}
