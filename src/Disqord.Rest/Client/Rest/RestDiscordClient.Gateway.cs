using System;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public partial class RestDiscordClient : IRestDiscordClient
    {
        public async Task<string> GetGatewayUrlAsync(RestRequestOptions options = null)
        {
            var model = await ApiClient.GetGatewayAsync(options).ConfigureAwait(false);
            return model.Url;
        }

        public async Task<RestGatewayBotResponse> GetGatewayBotUrlAsync(RestRequestOptions options = null)
        {
            if (TokenType != TokenType.Bot)
                throw new InvalidOperationException("This endpoint can only be used with a bot token.");

            var model = await ApiClient.GetGatewayBotAsync(options).ConfigureAwait(false);
            return new RestGatewayBotResponse(this, model);
        }
    }
}
