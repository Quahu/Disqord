using System;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace Disqord.Http.Default
{
    public class DefaultHttpClientFactory : IHttpClientFactory
    {
        private readonly SocketsHttpHandler _handler;

        public DefaultHttpClientFactory()
        {
            _handler = new SocketsHttpHandler
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
                PooledConnectionLifetime = TimeSpan.FromMinutes(2)
            };
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
}
