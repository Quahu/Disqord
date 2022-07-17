using System;
using Disqord.DependencyInjection.Extensions;
using Disqord.Rest.Api.Default;
using Microsoft.Extensions.DependencyInjection;

namespace Disqord.Rest.Api;

public static class RestApiServiceCollectionExtensions
{
    public static IServiceCollection AddRestApiClient(this IServiceCollection services, Action<DefaultRestApiClientConfiguration>? configure = null)
    {
        if (services.TryAddSingleton<IRestApiClient, DefaultRestApiClient>())
        {
            var options = services.AddOptions<DefaultRestApiClientConfiguration>();
            if (configure != null)
                options.Configure(configure);
        }

        services.AddRestRateLimiter();
        services.AddRestRequester();
        services.AddJsonSerializer();

        return services;
    }

    public static IServiceCollection AddRestRateLimiter(this IServiceCollection services, Action<DefaultRestRateLimiterConfiguration>? configure = null)
    {
        if (services.TryAddSingleton<IRestRateLimiter, DefaultRestRateLimiter>())
        {
            var options = services.AddOptions<DefaultRestRateLimiterConfiguration>();
            if (configure != null)
                options.Configure(configure);
        }

        return services;
    }

    public static IServiceCollection AddRestRequester(this IServiceCollection services, Action<DefaultRestRequesterConfiguration>? configure = null)
    {
        if (services.TryAddSingleton<IRestRequester, DefaultRestRequester>())
        {
            var options = services.AddOptions<DefaultRestRequesterConfiguration>();
            if (configure != null)
                options.Configure(configure);
        }

        services.AddHttpClientFactory();

        return services;
    }
}
