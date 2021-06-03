using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest.Api
{
    public static partial class RestApiClientExtensions
    {
        public static Task<MessageJsonModel> CreateInitialInteractionResponseAsync(this IRestApiClient client, Snowflake interactionId, string interactionToken, CreateInitialInteractionResponseJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.CreateInitialResponse, interactionId, interactionToken);
            return client.ExecuteAsync<MessageJsonModel>(route, content, options);
        }

        public static Task<MessageJsonModel> ModifyInitialInteractionResponseAsync(this IRestApiClient client, Snowflake interactionId, string interactionToken, MultipartJsonPayloadRestRequestContent<ModifyWebhookMessageJsonRestRequestContent> content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.ModifyInitialResponse, interactionId, interactionToken);
            return client.ExecuteAsync<MessageJsonModel>(route, content, options);
        }

        public static Task<MessageJsonModel> ModifyInitialInteractionResponseAsync(this IRestApiClient client, Snowflake interactionId, string interactionToken, ModifyWebhookMessageJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.ModifyInitialResponse, interactionId, interactionToken);
            return client.ExecuteAsync<MessageJsonModel>(route, content, options);
        }

        public static Task DeleteInitialInteractionResponseAsync(this IRestApiClient client, Snowflake interactionId, string interactionToken, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.DeleteInitialResponse, interactionId, interactionToken);
            return client.ExecuteAsync(route, null, options);
        }

        public static Task<MessageJsonModel> CreateFollowupInteractionResponseAsync(this IRestApiClient client, Snowflake interactionId, string interactionToken, MultipartJsonPayloadRestRequestContent<ExecuteWebhookJsonRestRequestContent> content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.CreateFollowupResponse, interactionId, interactionToken);
            return client.ExecuteAsync<MessageJsonModel>(route, content, options);
        }

        public static Task<MessageJsonModel> CreateFollowupInteractionResponseAsync(this IRestApiClient client, Snowflake interactionId, string interactionToken, ExecuteWebhookJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.CreateFollowupResponse, interactionId, interactionToken);
            return client.ExecuteAsync<MessageJsonModel>(route, content, options);
        }

        public static Task<MessageJsonModel> ModifyFollowupInteractionResponseAsync(this IRestApiClient client, Snowflake interactionId, string interactionToken, Snowflake messageId, MultipartJsonPayloadRestRequestContent<ModifyWebhookMessageJsonRestRequestContent> content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.ModifyFollowupResponse, interactionId, interactionToken, messageId);
            return client.ExecuteAsync<MessageJsonModel>(route, content, options);
        }

        public static Task<MessageJsonModel> ModifyFollowupInteractionResponseAsync(this IRestApiClient client, Snowflake interactionId, string interactionToken, Snowflake messageId, ModifyWebhookMessageJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.ModifyFollowupResponse, interactionId, interactionToken, messageId);
            return client.ExecuteAsync<MessageJsonModel>(route, content, options);
        }

        public static Task DeleteFollowupInteractionResponseAsync(this IRestApiClient client, Snowflake interactionId, string interactionToken, Snowflake messageId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.DeleteFollowupResponse, interactionId, interactionToken, messageId);
            return client.ExecuteAsync(route, null, options);
        }
    }
}
