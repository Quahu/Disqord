using System.Net.Http;
using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class RawJsonContent : IRequestContent
    {
        public byte[] Json { get; }

        public RawJsonContent(byte[] json)
        {
            Json = json;
        }

        public HttpContent Prepare(IJsonSerializer serializer, RestRequestOptions options)
            => new ByteArrayContent(Json);
    }
}
