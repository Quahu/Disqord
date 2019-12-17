using System;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public partial interface IRestDiscordClient : IDisposable
    {
        Task<string> GetGatewayUrlAsync(RestRequestOptions options = null);

        Task<RestGatewayBotResponse> GetGatewayBotUrlAsync(RestRequestOptions options = null);
    }
}
