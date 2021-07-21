using Disqord.Http;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class JsonObjectRestRequestContent<T> : IRestRequestContent
    {
        public T Object { get; }

        public JsonObjectRestRequestContent(T obj)
        {
            Object = obj;
        }

        public HttpRequestContent CreateHttpContent(IJsonSerializer serializer, IRestRequestOptions options = null)
            => JsonModelRestRequestContent.FromObject(Object, serializer);
    }
}
