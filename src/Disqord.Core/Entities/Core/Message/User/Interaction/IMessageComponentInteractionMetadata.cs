namespace Disqord;

/// <inheritdoc/>
public interface IMessageComponentInteractionMetadata : IMessageInteractionMetadata
{
    /// <summary>
    ///     Gets the ID of the  message the contained the interactive component.
    /// </summary>
    Snowflake InteractedMessageId { get; }
}
