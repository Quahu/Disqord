using Disqord.Http;

namespace Disqord.Rest.Api.Default
{
    public class DefaultRestResponse : IRestResponse
    {
        public IHttpResponse HttpResponse { get; }

        public DefaultRestResponse(IHttpResponse response)
        {
            HttpResponse = response;
        }

        public void Dispose()
        {
            HttpResponse.Dispose();
        }
    }
}
