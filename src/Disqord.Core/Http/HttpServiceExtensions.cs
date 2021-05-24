using System.Net;
using System.Net.Http;
using System.Threading;
using Disqord.DependencyInjection.Extensions;
using Disqord.Http.Default;
using Microsoft.Extensions.DependencyInjection;

namespace Disqord.Http
{
    public static class HttpServiceExtensions
    {
        public static IServiceCollection AddHttp(this IServiceCollection services)
        {
            if (services.TryAddSingleton<IHttpClientFactory, DefaultHttpClientFactory>())
                services.AddHttpClient(IHttpClientFactory.GlobalName, http =>
                    {
                        http.Timeout = Timeout.InfiniteTimeSpan;
                    })
                    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                    {
                        AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
                    });

            return services;
        }
    }
}
