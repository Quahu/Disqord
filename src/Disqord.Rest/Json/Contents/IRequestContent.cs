using System.Net.Http;
using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal interface IRequestContent
    {
        HttpContent Prepare(IJsonSerializer serializer, RestRequestOptions options);
    }
}
