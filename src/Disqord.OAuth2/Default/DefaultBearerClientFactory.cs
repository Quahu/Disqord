using System;
using Disqord.Rest;
using Disqord.Rest.Api;
using Disqord.Rest.Api.Default;
using Disqord.Rest.Default;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Qommon;

namespace Disqord.OAuth2.Default;

/// <inheritdoc/>
public class DefaultBearerClientFactory : IBearerClientFactory
{
    private readonly IServiceProvider _services;

    /// <summary>
    ///     Instantiates a new <see cref="DefaultBearerClientFactory"/>.
    /// </summary>
    public DefaultBearerClientFactory(IServiceProvider services)
    {
        _services = services;
    }

    public IBearerClient CreateClient(BearerToken token)
    {
        Guard.IsNotNull(token);

        var restApiClient = (_restApiClientFactory(_services, new object[]
        {
            token,
            ActivatorUtilities.CreateInstance<DefaultRestRateLimiter>(_services),
            ActivatorUtilities.CreateInstance<DefaultRestRequester>(_services)
        }) as IRestApiClient)!;

        var restClient = (_restClientFactory(_services, new object[]
        {
            Options.Create(new DefaultRestClientConfiguration
            {
                CachesDirectChannels = false
            }),
            restApiClient
        }) as IRestClient)!;

        return new DefaultBearerClient(restClient);
    }

    private static readonly ObjectFactory _restApiClientFactory;
    private static readonly ObjectFactory _restClientFactory;

    static DefaultBearerClientFactory()
    {
        _restApiClientFactory = ActivatorUtilities.CreateFactory(typeof(DefaultRestApiClient), new[] { typeof(Token), typeof(IRestRateLimiter), typeof(IRestRequester) });
        _restClientFactory = ActivatorUtilities.CreateFactory(typeof(DefaultRestClient), new[] { typeof(IOptions<DefaultRestClientConfiguration>), typeof(IRestApiClient) });
    }
}
