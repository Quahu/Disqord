using System.ComponentModel;
using Disqord.Rest;

namespace Disqord.Webhook.Default
{
    /// <inheritdoc/>
    public class DefaultWebhookClient : IWebhookClient
    {
        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IRestClient RestClient { get; }

        /// <inheritdoc/>
        public Snowflake Id { get; }

        /// <inheritdoc/>
        public string Token { get; }

        /// <summary>
        ///     Instantiates a new <see cref="DefaultWebhookClient"/> for the given REST client
        ///     and ID and token of the webhook.
        /// </summary>
        /// <param name="restClient"> The REST client to use. </param>
        /// <param name="id"> The ID of the webhook. </param>
        /// <param name="token"> The token of the webhook. </param>
        public DefaultWebhookClient(
            IRestClient restClient,
            Snowflake id,
            string token)
        {
            RestClient = restClient;
            Id = id;
            Token = token;
        }
    }
}
