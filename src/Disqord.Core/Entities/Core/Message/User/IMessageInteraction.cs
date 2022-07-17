namespace Disqord;

/// <summary>
///     Represents interaction information of a message that was sent
///     as a response to an interaction when there was no existing message.
///     This does not exist for component interactions as components exist on pre-existing messages.
/// </summary>
public interface IMessageInteraction : IIdentifiableEntity, INamableEntity
{
    /// <summary>
    ///     Gets the type of the interaction of the message.
    /// </summary>
    InteractionType Type { get; }

    /// <summary>
    ///     Gets the author of the interaction.
    /// </summary>
    IUser Author { get; }
}
