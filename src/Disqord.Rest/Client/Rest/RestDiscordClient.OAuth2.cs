using System;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public partial class RestDiscordClient : IRestDiscordClient
    {
        public async Task<RestApplication> GetCurrentApplicationAsync(RestRequestOptions options = null)
        {
            if (TokenType != TokenType.Bot)
                throw new InvalidOperationException("Cannot download the current application without a bot authorization token.");

            var model = await ApiClient.GetCurrentApplicationInformationAsync(options).ConfigureAwait(false);
            return new RestApplication(this, model);
        }
    }
}
