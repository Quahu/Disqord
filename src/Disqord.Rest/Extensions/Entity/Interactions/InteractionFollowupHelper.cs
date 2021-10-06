using System;
using System.Threading;
using System.Threading.Tasks;
using Qommon;

namespace Disqord.Rest
{
    public class InteractionFollowupHelper
    {
        public IInteraction Interaction { get; }

        public InteractionFollowupHelper(IInteraction interaction)
        {
            Guard.IsNotNull(interaction);

            Interaction = interaction;
        }

        public Task<IUserMessage> FetchResponseAsync(
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = Interaction.GetRestClient();
            return client.FetchInteractionResponseAsync(Interaction.ApplicationId, Interaction.Token, options, cancellationToken);
        }

        public Task<IUserMessage> ModifyResponseAsync(
            Action<ModifyWebhookMessageActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = Interaction.GetRestClient();
            return client.ModifyInteractionResponseAsync(Interaction.ApplicationId, Interaction.Token, action, options, cancellationToken);
        }

        public Task DeleteResponseAsync(
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = Interaction.GetRestClient();
            return client.DeleteInteractionResponseAsync(Interaction.ApplicationId, Interaction.Token, options, cancellationToken);
        }

        public Task<IUserMessage> SendAsync(
            LocalInteractionFollowup followup,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = Interaction.GetRestClient();
            return client.CreateInteractionFollowupAsync(Interaction.ApplicationId, Interaction.Token, followup, options, cancellationToken);
        }

        public Task<IUserMessage> FetchAsync(
            Snowflake followupId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = Interaction.GetRestClient();
            return client.FetchInteractionFollowupAsync(Interaction.ApplicationId, Interaction.Token, followupId, options, cancellationToken);
        }

        public Task<IUserMessage> ModifyAsync(
            Snowflake followupId, Action<ModifyWebhookMessageActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = Interaction.GetRestClient();
            return client.ModifyInteractionFollowupAsync(Interaction.ApplicationId, Interaction.Token, followupId, action, options, cancellationToken);
        }

        public Task DeleteAsync(
            Snowflake followupId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var client = Interaction.GetRestClient();
            return client.DeleteInteractionFollowupAsync(Interaction.ApplicationId, Interaction.Token, followupId, options, cancellationToken);
        }
    }
}
