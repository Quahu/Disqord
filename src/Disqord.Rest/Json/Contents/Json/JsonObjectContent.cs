using System.Net.Http;
using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class JsonObjectContent : IRequestContent
    {
        public object Object { get; }

        public JsonObjectContent(object obj)
        {
            Object = obj;
        }

        public HttpContent Prepare(IJsonSerializer serializer, RestRequestOptions options)
            => JsonRequestContent.PrepareFor(Object, serializer, options);
    }
}
