using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Http;
using Disqord.Rest.Api;
using Qommon;

namespace Disqord.Rest
{
    public static partial class RestClientExtensions
    {
        public static async Task<IStage> CreateStageAsync(this IRestClient client,
            Snowflake channelId, string topic, Action<CreateStageActionProperties> action = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var properties = new CreateStageActionProperties();
            action?.Invoke(properties);

            var content = new CreateStageInstanceJsonRestRequestContent
            {
                ChannelId = channelId,
                Topic = topic,
                PrivacyLevel = properties.PrivacyLevel,
                SendStartNotification = properties.NotifyEveryone
            };

            var model = await client.ApiClient.CreateStageInstanceAsync(channelId, content, options, cancellationToken).ConfigureAwait(false);
            return new TransientStage(client, model);
        }

        public static async Task<IStage> FetchStageAsync(this IRestClient client,
            Snowflake channelId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var model = await client.ApiClient.FetchStageInstanceAsync(channelId, options, cancellationToken).ConfigureAwait(false);
                return new TransientStage(client, model);
            }
            catch (RestApiException ex) when (ex.StatusCode == HttpResponseStatusCode.NotFound)
            {
                return null;
            }
        }

        public static async Task<IStage> ModifyStageAsync(this IRestClient client,
            Snowflake channelId, Action<ModifyStageActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(action);

            var properties = new ModifyStageActionProperties();
            action(properties);
            var content = new ModifyStageInstanceJsonRestRequestContent()
            {
                Topic = properties.Topic,
                PrivacyLevel = properties.PrivacyLevel
            };

            var model = await client.ApiClient.ModifyStageInstanceAsync(channelId, content, options, cancellationToken).ConfigureAwait(false);
            return new TransientStage(client, model);
        }

        public static Task DeleteStageAsync(this IRestClient client,
            Snowflake channelId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            return client.ApiClient.DeleteStageInstanceAsync(channelId, options, cancellationToken);
        }
    }
}
