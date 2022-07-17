using System;
using System.Threading;
using System.Threading.Tasks;
using Qommon;

namespace Disqord.Rest;

/// <summary>
///     Represents a helper type used for responding to an interaction.
/// </summary>
/// <seealso cref="InteractionResponseHelper"/>
public class InteractionFollowupHelper
{
    /// <summary>
    ///     Gets the interaction this helper is for.
    /// </summary>
    public IInteraction Interaction { get; }

    /// <summary>
    ///     Instantiates a new followup helper.
    /// </summary>
    /// <param name="interaction"> The interaction this helper is for. </param>
    protected internal InteractionFollowupHelper(IInteraction interaction)
    {
        Guard.IsNotNull(interaction);

        Interaction = interaction;
    }

    private void ThrowIfInvalid()
    {
        if (Interaction.IsExpired())
            throw new InteractionExpiredException(false);
    }

    /// <summary>
    ///     Fetches the response message that was sent using <see cref="InteractionResponseHelper.SendMessageAsync"/>.
    /// </summary>
    /// <param name="options"> The request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the request
    /// with the result being the fetched message.
    /// </returns>
    public Task<IUserMessage> FetchResponseAsync(
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        ThrowIfInvalid();

        var client = Interaction.GetRestClient();
        return client.FetchInteractionResponseAsync(Interaction.ApplicationId, Interaction.Token, options, cancellationToken);
    }

    /// <summary>
    ///     Modifies the response message that was sent using <see cref="InteractionResponseHelper.SendMessageAsync"/>.
    /// </summary>
    /// <param name="action"> The action representing the properties to be modified. </param>
    /// <param name="options"> The request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the request
    /// with the result being the modified message.
    /// </returns>
    public Task<IUserMessage> ModifyResponseAsync(
        Action<ModifyWebhookMessageActionProperties> action,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        ThrowIfInvalid();

        var client = Interaction.GetRestClient();
        return client.ModifyInteractionResponseAsync(Interaction.ApplicationId, Interaction.Token, action, options, cancellationToken);
    }

    /// <summary>
    ///     Deletes the response message that was sent using <see cref="InteractionResponseHelper.SendMessageAsync"/>.
    /// </summary>
    /// <param name="options"> The request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task"/> representing the request.
    /// </returns>
    public Task DeleteResponseAsync(
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        ThrowIfInvalid();

        var client = Interaction.GetRestClient();
        return client.DeleteInteractionResponseAsync(Interaction.ApplicationId, Interaction.Token, options, cancellationToken);
    }

    /// <summary>
    ///     Sends a followup message.
    /// </summary>
    /// <param name="followup"> The followup message. </param>
    /// <param name="options"> The request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the request
    /// with the result being the sent message.
    /// </returns>
    public Task<IUserMessage> SendAsync(
        LocalInteractionFollowup followup,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        ThrowIfInvalid();

        var client = Interaction.GetRestClient();
        return client.CreateInteractionFollowupAsync(Interaction.ApplicationId, Interaction.Token, followup, options, cancellationToken);
    }

    /// <summary>
    ///     Fetches a followup message with the specified ID.
    /// </summary>
    /// <param name="followupId"> The ID of the followup message. </param>
    /// <param name="options"> The request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the request
    /// with the result being the fetched message.
    /// </returns>
    public Task<IUserMessage?> FetchAsync(
        Snowflake followupId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        ThrowIfInvalid();

        var client = Interaction.GetRestClient();
        return client.FetchInteractionFollowupAsync(Interaction.ApplicationId, Interaction.Token, followupId, options, cancellationToken);
    }

    /// <summary>
    ///     Modifies a followup message with the specified ID.
    /// </summary>
    /// <param name="followupId"> The ID of the followup message. </param>
    /// <param name="action"> The action representing the properties to be modified. </param>
    /// <param name="options"> The request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the request
    /// with the result being the modified message.
    /// </returns>
    public Task<IUserMessage> ModifyAsync(
        Snowflake followupId, Action<ModifyWebhookMessageActionProperties> action,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        ThrowIfInvalid();

        var client = Interaction.GetRestClient();
        return client.ModifyInteractionFollowupAsync(Interaction.ApplicationId, Interaction.Token, followupId, action, options, cancellationToken);
    }

    /// <summary>
    ///     Deletes a followup message with the specified ID.
    /// </summary>
    /// <param name="followupId"> The ID of the followup message. </param>
    /// <param name="options"> The request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task"/> representing the request.
    /// </returns>
    public Task DeleteAsync(
        Snowflake followupId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        ThrowIfInvalid();

        var client = Interaction.GetRestClient();
        return client.DeleteInteractionFollowupAsync(Interaction.ApplicationId, Interaction.Token, followupId, options, cancellationToken);
    }
}
