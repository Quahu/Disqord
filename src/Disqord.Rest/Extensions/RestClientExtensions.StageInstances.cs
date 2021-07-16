using System;
using System.Threading.Tasks;
using Disqord.Http;
using Disqord.Rest.Api;

namespace Disqord.Rest
{
    public static partial class RestClientExtensions
    {
        public static async Task<IStageInstance> CreateStageInstanceAsync(this IRestClient client, Snowflake channelId, string topic, Action<CreateStageInstanceActionProperties> action, IRestRequestOptions options = null)
        {
            var properties = new CreateStageInstanceActionProperties();
            action?.Invoke(properties);

            var content = new CreateStageInstanceJsonRestRequestContent
            {
                ChannelId = channelId,
                Topic = topic,
                PrivacyLevel = properties.PrivacyLevel
            };
            var model = await client.ApiClient.CreateStageInstanceAsync(channelId, content, options).ConfigureAwait(false);
            return new TransientStageInstance(client, model);
        }

        public static async Task<IStageInstance> FetchStageInstanceAsync(this IRestClient client, Snowflake channelId, IRestRequestOptions options = null)
        {
            try
            {
                var model = await client.ApiClient.FetchStageInstanceAsync(channelId, options).ConfigureAwait(false);
                return new TransientStageInstance(client, model);
            }
            catch (RestApiException ex) when (ex.StatusCode == HttpResponseStatusCode.NotFound)
            {
                return null;
            }
        }

        public static async Task<IStageInstance> ModifyStageInstanceAsync(this IRestClient client, Snowflake channelId, Action<ModifyStageInstanceActionProperties> action, IRestRequestOptions options = null)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var properties = new ModifyStageInstanceActionProperties();
            action(properties);
            var content = new ModifyStageInstanceJsonRestRequestContent()
            {
                Topic = properties.Topic,
                PrivacyLevel = properties.PrivacyLevel
            };
            var model = await client.ApiClient.ModifyStageInstanceAsync(channelId, content, options).ConfigureAwait(false);
            return new TransientStageInstance(client, model);
        }

        public static Task DeleteStageInstanceAsync(this IRestClient client, Snowflake channelId, IRestRequestOptions options = null)
            => client.ApiClient.DeleteStageInstanceAsync(channelId, options);
    }
}
