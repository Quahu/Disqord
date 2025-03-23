﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Models;
using Disqord.Rest.Api.Models;

namespace Disqord.Rest.Api;

public static partial class RestApiClientExtensions
{
    public static Task<InteractionCallbackResponseJsonModel?> CreateInitialInteractionResponseAsync(this IRestApiClient client,
        Snowflake interactionId, string interactionToken, AttachmentJsonPayloadRestRequestContent<CreateInitialInteractionResponseJsonRestRequestContent> content,
        bool withResponse,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        return CreateInitialInteractionResponseCoreAsync(client, interactionId, interactionToken, content, withResponse, options, cancellationToken);
    }

    public static Task<InteractionCallbackResponseJsonModel?> CreateInitialInteractionResponseAsync(this IRestApiClient client,
        Snowflake interactionId, string interactionToken, CreateInitialInteractionResponseJsonRestRequestContent content,
        bool withResponse,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        return CreateInitialInteractionResponseCoreAsync(client, interactionId, interactionToken, content, withResponse, options, cancellationToken);
    }

    private static async Task<InteractionCallbackResponseJsonModel?> CreateInitialInteractionResponseCoreAsync(IRestApiClient client,
        Snowflake interactionId, string interactionToken, IRestRequestContent content,
        bool withResponse,
        IRestRequestOptions? options, CancellationToken cancellationToken)
    {
        var route = Format(Route.Interactions.CreateInitialResponse, withResponse
            ? new Dictionary<string, object>
            {
                ["with_response"] = true
            }
            : null, interactionId, interactionToken);

        if (withResponse)
        {
            return await client.ExecuteAsync<InteractionCallbackResponseJsonModel>(route, content, options, cancellationToken).ConfigureAwait(false);
        }

        await client.ExecuteAsync(route, content, options, cancellationToken).ConfigureAwait(false);
        return null;
    }

    public static Task<MessageJsonModel> FetchInitialInteractionResponseAsync(this IRestApiClient client,
        Snowflake applicationId, string interactionToken,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Interactions.GetInitialResponse, applicationId, interactionToken);
        return client.ExecuteAsync<MessageJsonModel>(route, null, options, cancellationToken);
    }

    public static Task<MessageJsonModel> ModifyInitialInteractionResponseAsync(this IRestApiClient client,
        Snowflake applicationId, string interactionToken, AttachmentJsonPayloadRestRequestContent<ModifyWebhookMessageJsonRestRequestContent> content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Interactions.ModifyInitialResponse, applicationId, interactionToken);
        return client.ExecuteAsync<MessageJsonModel>(route, content, options, cancellationToken);
    }

    public static Task<MessageJsonModel> ModifyInitialInteractionResponseAsync(this IRestApiClient client,
        Snowflake applicationId, string interactionToken, ModifyWebhookMessageJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Interactions.ModifyInitialResponse, applicationId, interactionToken);
        return client.ExecuteAsync<MessageJsonModel>(route, content, options, cancellationToken);
    }

    public static Task DeleteInitialInteractionResponseAsync(this IRestApiClient client,
        Snowflake applicationId, string interactionToken,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Interactions.DeleteInitialResponse, applicationId, interactionToken);
        return client.ExecuteAsync(route, null, options, cancellationToken);
    }

    public static Task<MessageJsonModel> CreateFollowupInteractionResponseAsync(this IRestApiClient client,
        Snowflake applicationId, string interactionToken, AttachmentJsonPayloadRestRequestContent<CreateFollowupMessageJsonRestRequestContent> content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Interactions.CreateFollowupResponse, applicationId, interactionToken);
        return client.ExecuteAsync<MessageJsonModel>(route, content, options, cancellationToken);
    }

    public static Task<MessageJsonModel> CreateFollowupInteractionResponseAsync(this IRestApiClient client,
        Snowflake applicationId, string interactionToken, CreateFollowupMessageJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Interactions.CreateFollowupResponse, applicationId, interactionToken);
        return client.ExecuteAsync<MessageJsonModel>(route, content, options, cancellationToken);
    }

    public static Task<MessageJsonModel> FetchFollowupInteractionResponseAsync(this IRestApiClient client,
        Snowflake applicationId, string interactionToken, Snowflake messageId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Interactions.GetFollowupResponse, applicationId, interactionToken, messageId);
        return client.ExecuteAsync<MessageJsonModel>(route, null, options, cancellationToken);
    }

    public static Task<MessageJsonModel> ModifyFollowupInteractionResponseAsync(this IRestApiClient client,
        Snowflake applicationId, string interactionToken, Snowflake messageId, AttachmentJsonPayloadRestRequestContent<ModifyWebhookMessageJsonRestRequestContent> content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Interactions.ModifyFollowupResponse, applicationId, interactionToken, messageId);
        return client.ExecuteAsync<MessageJsonModel>(route, content, options, cancellationToken);
    }

    public static Task<MessageJsonModel> ModifyFollowupInteractionResponseAsync(this IRestApiClient client,
        Snowflake applicationId, string interactionToken, Snowflake messageId, ModifyWebhookMessageJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Interactions.ModifyFollowupResponse, applicationId, interactionToken, messageId);
        return client.ExecuteAsync<MessageJsonModel>(route, content, options, cancellationToken);
    }

    public static Task DeleteFollowupInteractionResponseAsync(this IRestApiClient client,
        Snowflake applicationId, string interactionToken, Snowflake messageId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Interactions.DeleteFollowupResponse, applicationId, interactionToken, messageId);
        return client.ExecuteAsync(route, null, options, cancellationToken);
    }
}
