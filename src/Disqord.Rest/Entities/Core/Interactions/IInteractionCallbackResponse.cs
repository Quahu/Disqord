namespace Disqord.Rest;

/// <summary>
///     Represents the immediate interaction callback response from
///     <see cref="RestClientExtensions.CreateInteractionResponseAsync(Disqord.Rest.IRestClient,Disqord.Snowflake,string,Disqord.ILocalInteractionResponse,bool,Disqord.Rest.IRestRequestOptions?,System.Threading.CancellationToken)"/>.
/// </summary>
public interface IInteractionCallbackResponse : IEntity
{
    /// <summary>
    ///     Gets the interaction information of this response.
    /// </summary>
    ICallbackInteraction Interaction { get; }

    /// <summary>
    ///     Gets the interaction resource of this response.
    /// </summary>
    ICallbackResource? Resource { get; }
}
