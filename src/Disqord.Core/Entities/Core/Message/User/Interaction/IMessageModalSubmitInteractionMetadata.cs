namespace Disqord;

/// <inheritdoc/>
public interface IMessageModalSubmitInteractionMetadata : IMessageInteractionMetadata
{
    /// <summary>
    ///     Gets the interaction metadata of the interaction that opened the modal.
    /// </summary>
    IMessageInteractionMetadata TriggeringInteractionMetadata { get; }
}
