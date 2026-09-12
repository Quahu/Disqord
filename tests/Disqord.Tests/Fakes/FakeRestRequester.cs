using Disqord.Http;
using Disqord.Rest.Api;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Qommon.Binding;

namespace Disqord.Tests.Fakes;

internal sealed class FakeRestRequester : IRestRequester
{
    public IRestApiClient ApiClient => _binder.Value;

    public ILogger Logger => NullLogger.Instance;

    public int Version => 10;

    public IHttpClient HttpClient => throw new NotSupportedException();

    private readonly Binder<IRestApiClient> _binder;

    public FakeRestRequester()
    {
        _binder = new Binder<IRestApiClient>(this);
    }

    public void Bind(IRestApiClient value)
    {
        _binder.Bind(value);
    }

    public Task<IRestResponse> ExecuteAsync(IRestRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }

    public Task<IHttpResponse> ExecuteAuxiliaryAsync(IHttpRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }
}
