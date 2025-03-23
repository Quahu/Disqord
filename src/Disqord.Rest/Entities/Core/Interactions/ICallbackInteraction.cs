namespace Disqord.Rest;

/// <summary>
///     Represents the callback interaction of <see cref="IInteractionCallbackResponse"/>.
/// </summary>
public interface ICallbackInteraction : IIdentifiableEntity
{
    /// <summary>
    ///     Gets the type of the interaction.
    /// </summary>
    InteractionType Type { get; }

    /// <summary>
    ///     Gets the ID of the response message.
    /// </summary>
    Snowflake? ResponseMessageId { get; }

    /// <summary>
    ///     Gets whether the response message is "loading", i.e. whether it is deferred.
    /// </summary>
    bool? IsResponseMessageLoading { get; }

    /// <summary>
    ///     Gets whether the response message is ephemeral, i.e. only visible to the interaction author.
    /// </summary>
    bool? IsResponseMessageEphemeral { get; }
}
