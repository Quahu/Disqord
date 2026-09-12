using Disqord.Http;
using Disqord.Rest.Api;
using Disqord.Rest.Api.Default;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Qommon.Binding;

namespace Disqord.Tests.Fakes;

internal sealed class FakeRestRateLimiter : IRestRateLimiter
{
    public IRestApiClient ApiClient => _binder.Value;

    public ILogger Logger => NullLogger.Instance;

    public required FakeHttpResponse HttpResponse { get; init; }

    public List<IRestRequest> Requests { get; } = [];

    private readonly Binder<IRestApiClient> _binder;

    public FakeRestRateLimiter()
    {
        _binder = new Binder<IRestApiClient>(this);
    }

    public void Bind(IRestApiClient value)
    {
        _binder.Bind(value);
    }

    public bool IsRateLimited(IFormattedRoute? route = null)
    {
        return false;
    }

    public Task<IRestResponse> ExecuteAsync(IRestRequest request, CancellationToken cancellationToken)
    {
        Requests.Add(request);
        return Task.FromResult<IRestResponse>(new DefaultRestResponse(HttpResponse));
    }
}
