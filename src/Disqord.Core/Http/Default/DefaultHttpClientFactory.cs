using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Options;

namespace Disqord.Http.Default;

public class DefaultHttpClientFactory : IHttpClientFactory, IDisposable
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
            Timeout = TimeSpan.FromMinutes(2)
        };

        return new DefaultHttpClient(client);
    }

    public void Dispose()
    {
        _handler.Dispose();
    }
}
