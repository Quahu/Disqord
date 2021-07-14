﻿using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest.Api
{
    public static partial class RestApiClientExtensions
    {
        public static Task CreateInitialInteractionResponseAsync(this IRestApiClient client, Snowflake interactionId, string interactionToken, MultipartJsonPayloadRestRequestContent<CreateInitialInteractionResponseJsonRestRequestContent> content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.CreateInitialResponse, interactionId, interactionToken);
            return client.ExecuteAsync(route, content, options);
        }

        public static Task CreateInitialInteractionResponseAsync(this IRestApiClient client, Snowflake interactionId, string interactionToken, CreateInitialInteractionResponseJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.CreateInitialResponse, interactionId, interactionToken);
            return client.ExecuteAsync(route, content, options);
        }

        public static Task<MessageJsonModel> FetchInitialInteractionResponseAsync(this IRestApiClient client, Snowflake applicationId, string interactionToken, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.GetInitialResponse, applicationId, interactionToken);
            return client.ExecuteAsync<MessageJsonModel>(route, null, options);
        }

        public static Task<MessageJsonModel> ModifyInitialInteractionResponseAsync(this IRestApiClient client, Snowflake applicationId, string interactionToken, MultipartJsonPayloadRestRequestContent<ModifyWebhookMessageJsonRestRequestContent> content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.ModifyInitialResponse, applicationId, interactionToken);
            return client.ExecuteAsync<MessageJsonModel>(route, content, options);
        }

        public static Task<MessageJsonModel> ModifyInitialInteractionResponseAsync(this IRestApiClient client, Snowflake applicationId, string interactionToken, ModifyWebhookMessageJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.ModifyInitialResponse, applicationId, interactionToken);
            return client.ExecuteAsync<MessageJsonModel>(route, content, options);
        }

        public static Task DeleteInitialInteractionResponseAsync(this IRestApiClient client, Snowflake applicationId, string interactionToken, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.DeleteInitialResponse, applicationId, interactionToken);
            return client.ExecuteAsync(route, null, options);
        }

        public static Task<MessageJsonModel> CreateFollowupInteractionResponseAsync(this IRestApiClient client, Snowflake applicationId, string interactionToken, MultipartJsonPayloadRestRequestContent<ExecuteWebhookJsonRestRequestContent> content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.CreateFollowupResponse, applicationId, interactionToken);
            return client.ExecuteAsync<MessageJsonModel>(route, content, options);
        }

        public static Task<MessageJsonModel> CreateFollowupInteractionResponseAsync(this IRestApiClient client, Snowflake applicationId, string interactionToken, ExecuteWebhookJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.CreateFollowupResponse, applicationId, interactionToken);
            return client.ExecuteAsync<MessageJsonModel>(route, content, options);
        }

        public static Task<MessageJsonModel> ModifyFollowupInteractionResponseAsync(this IRestApiClient client, Snowflake applicationId, string interactionToken, Snowflake messageId, MultipartJsonPayloadRestRequestContent<ModifyWebhookMessageJsonRestRequestContent> content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.ModifyFollowupResponse, applicationId, interactionToken, messageId);
            return client.ExecuteAsync<MessageJsonModel>(route, content, options);
        }

        public static Task<MessageJsonModel> ModifyFollowupInteractionResponseAsync(this IRestApiClient client, Snowflake applicationId, string interactionToken, Snowflake messageId, ModifyWebhookMessageJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.ModifyFollowupResponse, applicationId, interactionToken, messageId);
            return client.ExecuteAsync<MessageJsonModel>(route, content, options);
        }

        public static Task DeleteFollowupInteractionResponseAsync(this IRestApiClient client, Snowflake applicationId, string interactionToken, Snowflake messageId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Interactions.DeleteFollowupResponse, applicationId, interactionToken, messageId);
            return client.ExecuteAsync(route, null, options);
        }
    }
}
