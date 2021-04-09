using System;
using Disqord.DependencyInjection.Extensions;
using Disqord.Serialization.Json;
using Disqord.Serialization.Json.Default;
using Microsoft.Extensions.DependencyInjection;

namespace Disqord
{
    public static class CoreServiceCollectionExtensions
    {
        public static IServiceCollection AddToken(this IServiceCollection services, Token token)
        {
            services.TryAddSingleton(token);
            return services;
        }

        public static IServiceCollection AddJsonSerializer(this IServiceCollection services, Action<DefaultJsonSerializerConfiguration> action = null)
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
}
