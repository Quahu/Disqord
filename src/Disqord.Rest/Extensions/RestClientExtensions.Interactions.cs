using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Http;
using Disqord.Models;
using Disqord.Rest.Api;
using Qommon;

namespace Disqord.Rest;

public static partial class RestClientExtensions
{
    public static Task CreateInteractionResponseAsync(this IRestClient client,
        Snowflake interactionId, string interactionToken, ILocalInteractionResponse response,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var content = response.ToContent(client.ApiClient.Serializer, out var attachments);

        Task task;
        if (attachments.Count != 0)
        {
            var multipartContent = new AttachmentJsonPayloadRestRequestContent<CreateInitialInteractionResponseJsonRestRequestContent>(content, attachments);
            task = client.ApiClient.CreateInitialInteractionResponseAsync(interactionId, interactionToken, multipartContent, options, cancellationToken);
        }
        else
        {
            task = client.ApiClient.CreateInitialInteractionResponseAsync(interactionId, interactionToken, content, options, cancellationToken);
        }

        return task;
    }

    public static async Task<IUserMessage> FetchInteractionResponseAsync(this IRestClient client,
        Snowflake applicationId, string interactionToken,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var model = await client.ApiClient.FetchInitialInteractionResponseAsync(applicationId, interactionToken, options, cancellationToken).ConfigureAwait(false);
        return new TransientUserMessage(client, model);
    }

    public static Task<IUserMessage> ModifyInteractionResponseAsync(this IRestClient client,
        Snowflake applicationId, string interactionToken, Action<ModifyWebhookMessageActionProperties> action,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        return client.InternalModifyInteractionResponseAsync(applicationId, interactionToken, null, action, options, cancellationToken);
    }

    public static Task DeleteInteractionResponseAsync(this IRestClient client,
        Snowflake applicationId, string interactionToken,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        return client.ApiClient.DeleteInitialInteractionResponseAsync(applicationId, interactionToken, options, cancellationToken);
    }

    public static async Task<IUserMessage> CreateInteractionFollowupAsync(this IRestClient client,
        Snowflake applicationId, string interactionToken, LocalInteractionFollowup followup,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(followup);

        var messageContent = new CreateFollowupMessageJsonRestRequestContent
        {
            Content = followup.Content,
            Tts = followup.IsTextToSpeech,
            Embeds = Optional.Convert(followup.Embeds, embeds => embeds.Select(embed => embed.ToModel()).ToArray()),
            AllowedMentions = Optional.Convert(followup.AllowedMentions, allowedMentions => allowedMentions.ToModel()),
            Components = Optional.Convert(followup.Components, components => components.Select(component => component.ToModel()).ToArray()),
            Flags = followup.Flags
        };

        Task<MessageJsonModel> task;
        if (followup.Attachments.TryGetValue(out var attachments) && attachments.Count != 0)
        {
            // If there are attachments, we must send them via multipart HTTP content.
            // Our `messageContent` will be serialized into a "payload_json" form data field.
            var content = new AttachmentJsonPayloadRestRequestContent<CreateFollowupMessageJsonRestRequestContent>(messageContent, attachments);
            task = client.ApiClient.CreateFollowupInteractionResponseAsync(applicationId, interactionToken, content, options, cancellationToken);
        }
        else
        {
            task = client.ApiClient.CreateFollowupInteractionResponseAsync(applicationId, interactionToken, messageContent, options, cancellationToken);
        }

        var model = await task.ConfigureAwait(false);
        return new TransientUserMessage(client, model);
    }

    public static async Task<IUserMessage?> FetchInteractionFollowupAsync(this IRestClient client,
        Snowflake applicationId, string interactionToken, Snowflake messageId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var model = await client.ApiClient.FetchFollowupInteractionResponseAsync(applicationId, interactionToken, messageId, options, cancellationToken).ConfigureAwait(false);
            return new TransientUserMessage(client, model);
        }
        catch (RestApiException ex) when (ex.StatusCode == HttpResponseStatusCode.NotFound && ex.IsError(RestApiErrorCode.UnknownMessage))
        {
            return null;
        }
    }

    public static Task<IUserMessage> ModifyInteractionFollowupAsync(this IRestClient client,
        Snowflake applicationId, string interactionToken, Snowflake messageId, Action<ModifyWebhookMessageActionProperties> action,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        return client.InternalModifyInteractionResponseAsync(applicationId, interactionToken, messageId, action, options, cancellationToken);
    }

    private static async Task<IUserMessage> InternalModifyInteractionResponseAsync(this IRestClient client,
        Snowflake applicationId, string interactionToken, Snowflake? messageId, Action<ModifyWebhookMessageActionProperties> action,
        IRestRequestOptions? options, CancellationToken cancellationToken)
    {
        var properties = new ModifyWebhookMessageActionProperties();
        action.Invoke(properties);
        var messageContent = new ModifyWebhookMessageJsonRestRequestContent
        {
            Content = properties.Content,
            Embeds = Optional.Convert(properties.Embeds, embeds => embeds.Select(embed => embed.ToModel()).ToArray()),
            AllowedMentions = Optional.Convert(properties.AllowedMentions, allowedMentions => allowedMentions.ToModel()),
            Attachments = Optional.Convert(properties.Attachments, attachments => attachments.Select(attachment => attachment.ToModel()).ToArray() as IList<PartialAttachmentJsonModel>),
            Components = Optional.Convert(properties.Components, x => x.Select(x => x.ToModel()).ToArray())
        };

        Task<MessageJsonModel> task;
        if (properties.Attachments.TryGetFullAttachments(out var attachments))
        {
            // If there are attachments, we must send it via multipart HTTP content.
            // Our `messageContent` will be serialized into a "payload_json" form data field.
            var content = new AttachmentJsonPayloadRestRequestContent<ModifyWebhookMessageJsonRestRequestContent>(messageContent, attachments);
            task = messageId == null
                ? client.ApiClient.ModifyInitialInteractionResponseAsync(applicationId, interactionToken, content, options, cancellationToken)
                : client.ApiClient.ModifyFollowupInteractionResponseAsync(applicationId, interactionToken, messageId.Value, content, options, cancellationToken);
        }
        else
        {
            task = messageId == null
                ? client.ApiClient.ModifyInitialInteractionResponseAsync(applicationId, interactionToken, messageContent, options, cancellationToken)
                : client.ApiClient.ModifyFollowupInteractionResponseAsync(applicationId, interactionToken, messageId.Value, messageContent, options, cancellationToken);
        }

        var model = await task.ConfigureAwait(false);
        return new TransientUserMessage(client, model);
    }

    public static Task DeleteInteractionFollowupAsync(this IRestClient client,
        Snowflake applicationId, string interactionToken, Snowflake messageId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        return client.ApiClient.DeleteFollowupInteractionResponseAsync(applicationId, interactionToken, messageId, options, cancellationToken);
    }
}
