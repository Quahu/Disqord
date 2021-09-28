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
        ///     Gets the type of the response if <see cref="HasResponded"/> is <see langword="true"/>.
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

        public async Task PongAsync(IRestRequestOptions options = null)
        {
            if (Interaction.Type != InteractionType.Ping)
                Throw.InvalidOperationException("The interaction type must be a ping to pong it.");

            ThrowIfResponded();
            var client = Interaction.GetRestClient();
            await client.CreateInteractionResponseAsync(Interaction.Id, Interaction.Token, new LocalInteractionResponse(InteractionResponseType.Pong), options).ConfigureAwait(false);
            SetResponded(InteractionResponseType.Pong);
        }

        public async Task DeferAsync(bool isEphemeral = false, IRestRequestOptions options = null)
        {
            if (Interaction is IComponentInteraction)
            {
                await DeferAsync(true, isEphemeral, options).ConfigureAwait(false);
                return;
            }

            ThrowIfResponded();
            var client = Interaction.GetRestClient();
            await client.CreateInteractionResponseAsync(Interaction.Id, Interaction.Token, new LocalInteractionResponse(InteractionResponseType.DeferredChannelMessage)
                .WithIsEphemeral(isEphemeral), options).ConfigureAwait(false);

            SetResponded(InteractionResponseType.DeferredChannelMessage);
        }

        public async Task DeferAsync(bool deferViaMessageUpdate, bool isEphemeral = false, IRestRequestOptions options = null)
        {
            ThrowIfResponded();
            var client = Interaction.GetRestClient();
            var responseType = deferViaMessageUpdate
                ? InteractionResponseType.DeferredMessageUpdate
                : InteractionResponseType.DeferredChannelMessage;

            await client.CreateInteractionResponseAsync(Interaction.Id, Interaction.Token, new LocalInteractionResponse(responseType)
                .WithIsEphemeral(isEphemeral), options).ConfigureAwait(false);

            SetResponded(responseType);
        }

        public async Task SendMessageAsync(LocalInteractionResponse response, IRestRequestOptions options = null)
        {
            response.Type = InteractionResponseType.ChannelMessage;
            var client = Interaction.GetRestClient();
            await client.CreateInteractionResponseAsync(Interaction.Id, Interaction.Token, response, options).ConfigureAwait(false);
            SetResponded(InteractionResponseType.ChannelMessage);
        }

        public async Task ModifyMessageAsync(LocalInteractionResponse response, IRestRequestOptions options = null)
        {
            response.Type = InteractionResponseType.MessageUpdate;
            var client = Interaction.GetRestClient();
            await client.CreateInteractionResponseAsync(Interaction.Id, Interaction.Token, response, options).ConfigureAwait(false);
            SetResponded(InteractionResponseType.MessageUpdate);
        }
    }
}
