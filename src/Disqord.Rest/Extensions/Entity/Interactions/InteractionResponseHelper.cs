using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Qommon;

namespace Disqord.Rest;

/// <summary>
///     Represents a helper type used for responding to an interaction.
/// </summary>
/// <seealso cref="InteractionFollowupHelper"/>
public class InteractionResponseHelper
{
    /// <summary>
    ///     Gets the interaction this helper is for.
    /// </summary>
    public IInteraction Interaction { get; }

    /// <summary>
    ///     Gets whether the interaction has been responded to via this helper.
    /// </summary>
    public bool HasResponded { get; private set; }

    /// <summary>
    ///     Gets the type of the response if <see cref="HasResponded"/> returned <see langword="true"/>.
    /// </summary>
    public InteractionResponseType ResponseType { get; private set; }

    /// <summary>
    ///     Instantiates a new response helper.
    /// </summary>
    /// <param name="interaction"> The interaction this helper is for. </param>
    protected internal InteractionResponseHelper(IInteraction interaction)
    {
        Guard.IsNotNull(interaction);

        Interaction = interaction;
    }

    private void ThrowIfInvalid()
    {
        if (HasResponded)
            Throw.InvalidOperationException("This interaction has already been responded to.");

        if (Interaction.IsResponseExpired())
            throw new InteractionExpiredException(true);
    }

    private void SetResponded(InteractionResponseType type)
    {
        HasResponded = true;
        ResponseType = type;
    }

    /// <summary>
    ///     Responds to the interaction by sending an <see cref="InteractionResponseType.Pong"/> response,
    ///     i.e. acknowledges it.
    /// </summary>
    /// <param name="options"> The request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <exception cref="InvalidOperationException">
    ///     The interaction type must be <see cref="InteractionType.Ping"/> in order to respond with <see cref="InteractionResponseType.Pong"/>.
    /// </exception>
    /// <returns>
    ///     A <see cref="Task"/> representing the request.
    /// </returns>
    public async Task PongAsync(
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        ThrowIfInvalid();

        if (Interaction.Type != InteractionType.Ping)
            Throw.InvalidOperationException("The interaction must be a ping in order to respond with a pong.");

        var response = new LocalInteractionMessageResponse(InteractionResponseType.Pong);

        var client = Interaction.GetRestClient();
        await client.CreateInteractionResponseAsync(Interaction.Id, Interaction.Token, response, options, cancellationToken: cancellationToken).ConfigureAwait(false);

        SetResponded(InteractionResponseType.Pong);
    }

    /// <summary>
    ///     Responds to the interaction by sending an <see cref="InteractionResponseType.DeferredChannelMessage"/>
    ///     or <see cref="InteractionResponseType.DeferredMessageUpdate"/> based on the interaction type,
    ///     i.e. acknowledges the interaction and allows sending followups.
    /// </summary>
    /// <remarks>
    ///     <see cref="IComponentInteraction"/> uses <see cref="DeferralType.MessageUpdate"/> and
    ///     all other interaction types use <see cref="DeferralType.ChannelMessage"/>.
    ///     <para/>
    ///     See <see cref="DeferAsync(DeferralType,bool,IRestRequestOptions,CancellationToken)"/> for more information on deferrals.
    /// </remarks>
    /// <param name="isEphemeral"> Whether the followup should be ephemeral. </param>
    /// <param name="options"> The request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task"/> representing the request.
    /// </returns>
    /// <seealso cref="InteractionFollowupHelper"/>
    public async Task DeferAsync(
        bool isEphemeral = false,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        if (Interaction is IComponentInteraction)
        {
            await DeferAsync(DeferralType.MessageUpdate, isEphemeral, options, cancellationToken).ConfigureAwait(false);
            return;
        }

        ThrowIfInvalid();

        var response = new LocalInteractionMessageResponse(InteractionResponseType.DeferredChannelMessage)
            .WithIsEphemeral(isEphemeral);

        var client = Interaction.GetRestClient();
        await client.CreateInteractionResponseAsync(Interaction.Id, Interaction.Token, response, options, cancellationToken).ConfigureAwait(false);

        SetResponded(InteractionResponseType.DeferredChannelMessage);
    }

    /// <summary>
    ///     Responds to the interaction by sending an <see cref="InteractionResponseType.DeferredChannelMessage"/>
    ///     or <see cref="InteractionResponseType.DeferredMessageUpdate"/> based on the specified <see cref="DeferralType"/>,
    ///     i.e. acknowledges the interaction and allows sending followups.
    /// </summary>
    /// <remarks>
    ///     <see cref="DeferralType.ChannelMessage"/> specifies that sending a followup should
    ///     send a new message in the interaction's channel.
    ///     <para/>
    ///     <see cref="DeferralType.MessageUpdate"/> specifies that sending a followup should update
    ///     the original message the interaction is for (invalid for application commands).
    /// </remarks>
    /// <param name="deferralType"> The type of deferral. </param>
    /// <param name="isEphemeral"> Whether the followup should be ephemeral. </param>
    /// <param name="options"> The request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task"/> representing the request.
    /// </returns>
    /// <seealso cref="InteractionFollowupHelper"/>
    public async Task DeferAsync(
        DeferralType deferralType, bool isEphemeral = false,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        ThrowIfInvalid();

        var responseType = deferralType switch
        {
            DeferralType.ChannelMessage => InteractionResponseType.DeferredChannelMessage,
            DeferralType.MessageUpdate => InteractionResponseType.DeferredMessageUpdate,
            _ => Throw.ArgumentOutOfRangeException<InteractionResponseType>(nameof(deferralType))
        };

        var response = new LocalInteractionMessageResponse(responseType)
            .WithIsEphemeral(isEphemeral);

        var client = Interaction.GetRestClient();
        await client.CreateInteractionResponseAsync(Interaction.Id, Interaction.Token, response, options, cancellationToken).ConfigureAwait(false);

        SetResponded(responseType);
    }

    /// <summary>
    ///     Responds to the interaction by sending an <see cref="InteractionResponseType.ChannelMessage"/>,
    ///     i.e. sends a message in the interaction's channel and allows sending followups.
    /// </summary>
    /// <remarks>
    ///     Unlike when sending normal channel message, this request does not
    ///     return the sent message.
    ///     Instead it can be retrieved using <see cref="InteractionFollowupHelper.FetchResponseAsync"/>.
    /// </remarks>
    /// <param name="response"> The message response. </param>
    /// <param name="options"> The request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task"/> representing the request.
    /// </returns>
    /// <seealso cref="InteractionFollowupHelper"/>
    /// <seealso cref="InteractionFollowupHelper.FetchResponseAsync"/>
    public async Task SendMessageAsync(
        LocalInteractionMessageResponse response,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        ThrowIfInvalid();

        Guard.IsNotNull(response);

        response.Type = InteractionResponseType.ChannelMessage;

        var client = Interaction.GetRestClient();
        await client.CreateInteractionResponseAsync(Interaction.Id, Interaction.Token, response, options, cancellationToken).ConfigureAwait(false);

        SetResponded(InteractionResponseType.ChannelMessage);
    }

    /// <summary>
    ///     Responds to the interaction by sending an <see cref="InteractionResponseType.MessageUpdate"/>,
    ///     i.e. modifies the original message the interaction is for and allows sending followups.
    /// </summary>
    /// <param name="response"> The message response. </param>
    /// <param name="options"> The request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task"/> representing the request.
    /// </returns>
    public async Task ModifyMessageAsync(
        LocalInteractionMessageResponse response,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        ThrowIfInvalid();

        Guard.IsNotNull(response);

        response.Type = InteractionResponseType.MessageUpdate;

        var client = Interaction.GetRestClient();
        await client.CreateInteractionResponseAsync(Interaction.Id, Interaction.Token, response, options, cancellationToken).ConfigureAwait(false);

        SetResponded(InteractionResponseType.MessageUpdate);
    }

    /// <summary>
    ///     Responds to the interaction by sending an <see cref="InteractionResponseType.ApplicationCommandAutoComplete"/>,
    ///     i.e. auto-completes user input with matching choices.
    /// </summary>
    /// <param name="choices"> The matching choices. </param>
    /// <param name="options"> The request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <exception cref="InvalidOperationException">
    ///     The interaction type must be <see cref="InteractionType.ApplicationCommandAutoComplete"/>
    ///     in order to respond with <see cref="InteractionResponseType.ApplicationCommandAutoComplete"/>.
    /// </exception>
    /// <returns>
    ///     A <see cref="Task"/> representing the request.
    /// </returns>
    public async Task AutoCompleteAsync(
        IEnumerable<KeyValuePair<string, object>> choices,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        ThrowIfInvalid();

        Guard.IsNotNull(choices);

        if (Interaction.Type != InteractionType.ApplicationCommandAutoComplete)
            Throw.InvalidOperationException("The interaction must be an auto-complete in order to respond with an auto-complete response.");

        var response = new LocalInteractionAutoCompleteResponse()
            .WithChoices(choices);

        var client = Interaction.GetRestClient();
        await client.CreateInteractionResponseAsync(Interaction.Id, Interaction.Token, response, options, cancellationToken).ConfigureAwait(false);

        SetResponded(InteractionResponseType.ApplicationCommandAutoComplete);
    }

    /// <summary>
    ///     Responds to the interaction by sending an <see cref="InteractionResponseType.Modal"/>,
    ///     i.e. opens a modal for the user that they can submit with required data.
    /// </summary>
    /// <param name="response"> The modal response. </param>
    /// <param name="options"> The request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task"/> representing the request.
    /// </returns>
    public async Task SendModalAsync(
        LocalInteractionModalResponse response,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        ThrowIfInvalid();

        Guard.IsNotNull(response);

        var client = Interaction.GetRestClient();
        await client.CreateInteractionResponseAsync(Interaction.Id, Interaction.Token, response, options, cancellationToken).ConfigureAwait(false);

        SetResponded(InteractionResponseType.Modal);
    }
}
