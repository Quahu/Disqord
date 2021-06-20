using System;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public class InteractionFollowupHelper
    {
        public IInteraction Interaction { get; }

        public InteractionFollowupHelper(IInteraction interaction)
        {
            Interaction = interaction;
        }

        public Task<IUserMessage> FetchResponseAsync(IRestRequestOptions options = null)
        {
            var client = Interaction.GetRestClient();
            return client.FetchInteractionResponseAsync(Interaction.ApplicationId, Interaction.Token, options);
        }

        public Task<IUserMessage> ModifyResponseAsync(Action<ModifyWebhookMessageActionProperties> action, IRestRequestOptions options = null)
        {
            var client = Interaction.GetRestClient();
            return client.ModifyInteractionResponseAsync(Interaction.ApplicationId, Interaction.Token, action, options);
        }

        public Task DeleteResponseAsync(IRestRequestOptions options = null)
        {
            var client = Interaction.GetRestClient();
            return client.DeleteInteractionResponseAsync(Interaction.ApplicationId, Interaction.Token, options);
        }

        public Task<IUserMessage> SendAsync(LocalInteractionFollowup followup, IRestRequestOptions options = null)
        {
            var client = Interaction.GetRestClient();
            return client.CreateInteractionFollowupAsync(Interaction.ApplicationId, Interaction.Token, followup, options);
        }

        public Task<IUserMessage> ModifyAsync(Snowflake followupId, Action<ModifyWebhookMessageActionProperties> action, IRestRequestOptions options = null)
        {
            var client = Interaction.GetRestClient();
            return client.ModifyInteractionFollowupAsync(Interaction.ApplicationId, Interaction.Token, followupId, action, options);
        }

        public Task DeleteAsync(Snowflake followupId, IRestRequestOptions options = null)
        {
            var client = Interaction.GetRestClient();
            return client.DeleteInteractionFollowupAsync(Interaction.ApplicationId, Interaction.Token, followupId, options);
        }
    }
}
