using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    public bool HasResponded => _hasResponded;

    /// <summary>
    ///     Gets the type of the response if <see cref="HasResponded"/> returned <see langword="true"/>.
    /// </summary>
    public InteractionResponseType ResponseType => _responseType;

    private volatile bool _hasResponded;
    private volatile InteractionResponseType _responseType;

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
        if (_hasResponded)
        {
            Throw.InvalidOperationException("This interaction has already been responded to.");
        }

        if (Interaction.IsResponseExpired())
        {
            throw new InteractionExpiredException(true);
        }
    }

    private void SetResponded(InteractionResponseType type)
    {
        _hasResponded = true;
        _responseType = type;
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
        {
            Throw.InvalidOperationException("The interaction must be a ping in order to respond with a pong.");
        }

        var response = new LocalInteractionMessageResponse
        {
            Type = InteractionResponseType.Pong
        };

        await CreateResponseAsync(response, withCallbackResponse: false, options, cancellationToken: cancellationToken).ConfigureAwait(false);
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

        var response = new LocalInteractionMessageResponse
        {
            Type = InteractionResponseType.DeferredChannelMessage,
            IsEphemeral = isEphemeral
        };

        await CreateResponseAsync(response, withCallbackResponse: false, options, cancellationToken).ConfigureAwait(false);
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

        var response = new LocalInteractionMessageResponse
        {
            Type = responseType,
            IsEphemeral = isEphemeral
        };

        await CreateResponseAsync(response, withCallbackResponse: false, options, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    ///     Responds to the interaction by sending an <see cref="InteractionResponseType.ChannelMessage"/>,
    ///     i.e. sends a message in the interaction's channel and allows sending followups.
    /// </summary>
    /// <param name="response"> The message response. </param>
    /// <param name="options"> The request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task"/> representing the request.
    /// </returns>
    /// <seealso cref="InteractionFollowupHelper"/>
    /// <seealso cref="InteractionFollowupHelper.FetchResponseAsync"/>
    public async Task<IUserMessage> SendMessageAsync(
        LocalInteractionMessageResponse response,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        ThrowIfInvalid();

        Guard.IsNotNull(response);

        response.Type = InteractionResponseType.ChannelMessage;

        var callbackResponse = await CreateResponseAsync(response, withCallbackResponse: true, options, cancellationToken).ConfigureAwait(false);
        return GetRequiredMessageResource(callbackResponse);
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
    public async Task<IUserMessage> ModifyMessageAsync(
        LocalInteractionMessageResponse response,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        ThrowIfInvalid();

        Guard.IsNotNull(response);

        response.Type = InteractionResponseType.MessageUpdate;

        var callbackResponse = await CreateResponseAsync(response, withCallbackResponse: true, options, cancellationToken).ConfigureAwait(false);
        return GetRequiredMessageResource(callbackResponse);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static IUserMessage GetRequiredMessageResource(IInteractionCallbackResponse? callbackResponse)
    {
        Guard.IsNotNull(callbackResponse);
        Guard.IsNotNull(callbackResponse.Resource);
        Guard.IsNotNull(callbackResponse.Resource.Message);

        return callbackResponse.Resource.Message;
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

        await CreateResponseAsync(response, withCallbackResponse: false, options, cancellationToken).ConfigureAwait(false);
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

        await CreateResponseAsync(response, withCallbackResponse: false, options, cancellationToken).ConfigureAwait(false);
    }

    private async Task<IInteractionCallbackResponse?> CreateResponseAsync(
        ILocalInteractionResponse response,
        bool withCallbackResponse,
        IRestRequestOptions? options, CancellationToken cancellationToken)
    {
        var client = Interaction.GetRestClient();
        var callbackResponse = await client.CreateInteractionResponseAsync(Interaction.Id, Interaction.Token, response, withCallbackResponse, options, cancellationToken).ConfigureAwait(false);

        SetResponded(response.Type);

        return callbackResponse;
    }
}
