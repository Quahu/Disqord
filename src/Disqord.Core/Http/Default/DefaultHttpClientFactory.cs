using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using Microsoft.Extensions.Options;

namespace Disqord.Http.Default;

public class DefaultHttpClientFactory : IHttpClientFactory
{
    private readonly SocketsHttpHandler _handler;

    public DefaultHttpClientFactory(
        IOptions<DefaultHttpClientFactoryConfiguration> options)
    {
        _handler = new SocketsHttpHandler
        {
            AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
            PooledConnectionLifetime = TimeSpan.FromMinutes(2)
        };

        options.Value.ClientConfiguration?.Invoke(_handler);
    }

    public IHttpClient CreateClient()
    {
        var client = new HttpClient(_handler, false)
        {
            Timeout = Timeout.InfiniteTimeSpan
        };

        return new DefaultHttpClient(client);
    }
}