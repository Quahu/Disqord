using System;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public partial interface IRestDiscordClient : IDisposable
    {
        Task<string> GetGatewayUrlAsync(RestRequestOptions options = null);

        Task<RestGatewayBotResponse> GetGatewayBotUrlAsync(RestRequestOptions options = null);
    }
}
