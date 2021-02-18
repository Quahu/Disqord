using Disqord.Http;

namespace Disqord.Rest.Api
{
    public class JsonObjectRestRequestContent<T> : IRestRequestContent
    {
        public T Object { get; }

        public JsonObjectRestRequestContent(T obj)
        {
            Object = obj;
        }

        public HttpRequestContent CreateHttpContent(IRestApiClient client, IRestRequestOptions options = null)
            => JsonModelRestRequestContent.FromObject(Object, client.Serializer);
    }
}
