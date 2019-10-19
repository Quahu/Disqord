using System;
using System.Net.Http;
using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class RawJsonContent : IRequestContent
    {
        public ReadOnlyMemory<byte> Json { get; }

        public RawJsonContent(ReadOnlyMemory<byte> json)
        {
            Json = json;
        }

        public HttpContent Prepare(IJsonSerializer serializer, RestRequestOptions options)
            => new ReadOnlyMemoryContent(Json);
    }
}
