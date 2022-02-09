using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Qommon;

namespace Disqord.Rest
{
    public class InteractionResponseHelper
    {
        public IInteraction Interaction { get; }

        /// <summary>
        ///     Gets whether the interaction has been responded to via this helper.
        /// </summary>
        public bool HasResponded { get; private set; }

        /// <summary>
        ///     Gets the type of the response if <see cref="HasResponded"/> returns <see langword="true"/>.
        /// </summary>
        public InteractionResponseType ResponseType { get; private set; }

        public InteractionResponseHelper(IInteraction interaction)
        {
            Guard.IsNotNull(interaction);

            Interaction = interaction;
        }

        private void ThrowIfResponded()
        {
            if (HasResponded)
                Throw.InvalidOperationException("This interaction has already been responded to.");
        }

        private void SetResponded(InteractionResponseType type)
        {
            HasResponded = true;
            ResponseType = type;
        }

        public async Task PongAsync(
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            if (Interaction.Type != InteractionType.Ping)
                Throw.InvalidOperationException("The interaction type must be a ping to pong it.");

            ThrowIfResponded();

            var client = Interaction.GetRestClient();
            var response = new LocalInteractionMessageResponse(InteractionResponseType.Pong);
            await client.CreateInteractionResponseAsync(Interaction.Id, Interaction.Token, response, options, cancellationToken: cancellationToken).ConfigureAwait(false);

            SetResponded(InteractionResponseType.Pong);
        }

        public async Task DeferAsync(
            bool isEphemeral = false,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            if (Interaction is IComponentInteraction)
            {
                await DeferAsync(true, isEphemeral, options, cancellationToken).ConfigureAwait(false);
                return;
            }

            ThrowIfResponded();
            var client = Interaction.GetRestClient();
            var response = new LocalInteractionMessageResponse(InteractionResponseType.DeferredChannelMessage)
                .WithIsEphemeral(isEphemeral);

            await client.CreateInteractionResponseAsync(Interaction.Id, Interaction.Token, response, options, cancellationToken).ConfigureAwait(false);

            SetResponded(InteractionResponseType.DeferredChannelMessage);
        }

        public async Task DeferAsync(
            bool deferViaMessageUpdate, bool isEphemeral = false,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            ThrowIfResponded();

            var client = Interaction.GetRestClient();
            var responseType = deferViaMessageUpdate
                ? InteractionResponseType.DeferredMessageUpdate
                : InteractionResponseType.DeferredChannelMessage;

            var response = new LocalInteractionMessageResponse(responseType)
                .WithIsEphemeral(isEphemeral);

            await client.CreateInteractionResponseAsync(Interaction.Id, Interaction.Token, response, options, cancellationToken).ConfigureAwait(false);

            SetResponded(responseType);
        }

        public async Task SendMessageAsync(
            LocalInteractionMessageResponse response,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(response);

            response.Type = InteractionResponseType.ChannelMessage;

            var client = Interaction.GetRestClient();
            await client.CreateInteractionResponseAsync(Interaction.Id, Interaction.Token, response, options, cancellationToken).ConfigureAwait(false);

            SetResponded(InteractionResponseType.ChannelMessage);
        }

        public async Task ModifyMessageAsync(
            LocalInteractionMessageResponse response,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(response);

            response.Type = InteractionResponseType.MessageUpdate;

            var client = Interaction.GetRestClient();
            await client.CreateInteractionResponseAsync(Interaction.Id, Interaction.Token, response, options, cancellationToken).ConfigureAwait(false);

            SetResponded(InteractionResponseType.MessageUpdate);
        }

        public async Task AutoCompleteAsync(
            IEnumerable<LocalSlashCommandOptionChoice> choices,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(choices);

            var response = new LocalInteractionAutoCompleteResponse();
            response.WithChoices(choices);

            var client = Interaction.GetRestClient();
            await client.CreateInteractionResponseAsync(Interaction.Id, Interaction.Token, response, options, cancellationToken).ConfigureAwait(false);

            SetResponded(InteractionResponseType.ApplicationCommandAutoComplete);
        }
    }
}
