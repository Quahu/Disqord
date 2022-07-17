using System;
using Disqord.DependencyInjection.Extensions;
using Disqord.Http;
using Disqord.Http.Default;
using Disqord.Serialization.Json;
using Disqord.Serialization.Json.Default;
using Disqord.WebSocket;
using Disqord.WebSocket.Default;
using Microsoft.Extensions.DependencyInjection;

namespace Disqord;

public static class CoreServiceCollectionExtensions
{
    public static IServiceCollection AddToken(this IServiceCollection services, Token token)
    {
        services.TryAddSingleton(token);
        return services;
    }

    public static IServiceCollection AddHttpClientFactory(this IServiceCollection services)
    {
        services.TryAddSingleton<IHttpClientFactory, DefaultHttpClientFactory>();
        return services;
    }

    public static IServiceCollection AddWebSocketClientFactory(this IServiceCollection services)
    {
        services.TryAddSingleton<IWebSocketClientFactory, DefaultWebSocketClientFactory>();
        return services;
    }

    public static IServiceCollection AddJsonSerializer(this IServiceCollection services, Action<DefaultJsonSerializerConfiguration>? action = null)
    {
        if (services.TryAddSingleton<IJsonSerializer, DefaultJsonSerializer>())
        {
            services.AddOptions<DefaultJsonSerializerConfiguration>();
            if (action != null)
                services.Configure(action);
        }

        return services;
    }
}