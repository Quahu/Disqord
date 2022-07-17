using System.Threading;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest.Api;

public static partial class RestApiClientExtensions
{
    public static Task CreateInitialInteractionResponseAsync(this IRestApiClient client,
        Snowflake interactionId, string interactionToken, AttachmentJsonPayloadRestRequestContent<CreateInitialInteractionResponseJsonRestRequestContent> content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Interactions.CreateInitialResponse, interactionId, interactionToken);
        return client.ExecuteAsync(route, content, options, cancellationToken);
    }

    public static Task CreateInitialInteractionResponseAsync(this IRestApiClient client,
        Snowflake interactionId, string interactionToken, CreateInitialInteractionResponseJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Interactions.CreateInitialResponse, interactionId, interactionToken);
        return client.ExecuteAsync(route, content, options, cancellationToken);
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