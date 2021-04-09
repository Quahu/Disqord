using Disqord.DependencyInjection.Extensions;
using Disqord.Http.Default;
using Microsoft.Extensions.DependencyInjection;

namespace Disqord.Http
{
    public static class HttpServiceExtensions
    {
        public static IServiceCollection AddHttp(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpClient, DefaultHttpClient>();
            return services;
        }
    }
}
