namespace Disqord
{
    public interface ILocalInteractionResponse : ILocalConstruct
    {
        /// <summary>
        ///     Gets or sets the type of this response.
        /// </summary>
        InteractionResponseType Type { get; }
    }
}
