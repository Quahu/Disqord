using Disqord.Rest;
using Disqord.Rest.Api;
using Disqord.Serialization.Json;
using Microsoft.Extensions.Logging;

namespace Disqord.Tests.Fakes;

internal sealed class FakeRestApiClient : IRestApiClient
{
    private readonly IRestRequester? _requester;
    private readonly Token? _token;

    public IRestRequester Requester
    {
        get => _requester ?? throw new NotSupportedException();
        init => _requester = value;
    }

    public Token Token
    {
        get => _token ?? throw new NotSupportedException();
        init => _token = value;
    }

    public ILogger Logger => throw new NotSupportedException();

    public IRestRateLimiter RateLimiter => throw new NotSupportedException();

    public IJsonSerializer Serializer => throw new NotSupportedException();

    public Task ExecuteAsync(IFormattedRoute route, IRestRequestContent? content = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }

    public Task<TModel> ExecuteAsync<TModel>(IFormattedRoute route, IRestRequestContent? content = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
        where TModel : class
    {
        throw new NotSupportedException();
    }
}
