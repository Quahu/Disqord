using System.Threading;
using System.Threading.Tasks;
using Disqord.Api;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public interface IRestApiClient : IApiClient
    {
        IRestRateLimiter RateLimiter { get; }

        IRestRequester Requester { get; }

        IJsonSerializer Serializer { get; }

        Task ExecuteAsync(FormattedRoute route, IRestRequestContent content = null, IRestRequestOptions options = null, CancellationToken cancellationToken = default);

        Task<TModel> ExecuteAsync<TModel>(FormattedRoute route, IRestRequestContent content = null, IRestRequestOptions options = null, CancellationToken cancellationToken = default)
            where TModel : class;
    }
}
