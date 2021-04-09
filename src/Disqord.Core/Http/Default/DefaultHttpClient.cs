using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Http.Default
{
    public sealed partial class DefaultHttpClient : IHttpClient
    {
        public Uri BaseUri
        {
            get => _http.BaseAddress;
            set => _http.BaseAddress = value;
        }

        private readonly HttpClient _http;

        public DefaultHttpClient()
        {
            _http = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            })
            {
                Timeout = Timeout.InfiniteTimeSpan
            };
        }

        public async Task<IHttpResponse> SendAsync(IHttpRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            using (var requestMessage = GetHttpMessage(request))
            {
                var responseMessage = await _http.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
                return new DefaultHttpResponse(responseMessage);
            }
        }

        public void SetDefaultHeader(string name, string value)
        {
            _http.DefaultRequestHeaders.Set(name, value);
        }

        public void Dispose()
        {
            _http.Dispose();
        }
    }
}
