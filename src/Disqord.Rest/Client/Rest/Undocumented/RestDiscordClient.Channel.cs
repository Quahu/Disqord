using System.Threading.Tasks;

namespace Disqord.Rest
{
    public partial class RestDiscordClient : IRestDiscordClient
    {
        public async Task MarkMessageAsReadAsync(Snowflake channelId, Snowflake messageId, RestRequestOptions options = null)
        {
            _ackToken = await ApiClient.AckMessageAsync(channelId, messageId, _ackToken, options).ConfigureAwait(false);
        }
    }
}
