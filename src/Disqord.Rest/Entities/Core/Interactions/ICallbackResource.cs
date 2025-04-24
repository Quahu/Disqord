namespace Disqord.Rest;

/// <summary>
///     Represents the callback resource of <see cref="IInteractionCallbackResponse"/>.
/// </summary>
public interface ICallbackResource : IEntity
{
    InteractionResponseType Type { get; }

    IUserMessage? Message { get; }
}
