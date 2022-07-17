using System;
using Disqord.DependencyInjection.Extensions;
using Disqord.Rest.Api;
using Disqord.Rest.Default;
using Microsoft.Extensions.DependencyInjection;

namespace Disqord.Rest;

public static class RestServiceCollectionExtensions
{
    public static IServiceCollection AddRestClient(this IServiceCollection services, Action<DefaultRestClientConfiguration>? action = null)
    {
        if (services.TryAddSingleton<IRestClient, DefaultRestClient>())
        {
            var options = services.AddOptions<DefaultRestClientConfiguration>();
            if (action != null)
                options.Configure(action);
        }

        services.AddRestApiClient();

        return services;
    }
}