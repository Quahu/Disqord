using Disqord.Http;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public interface IRestRequestContent
    {
        HttpRequestContent CreateHttpContent(IJsonSerializer serializer, IRestRequestOptions options = null);

        /// <summary>
        ///     Ensures that this request content is valid to be sent to the Discord API.
        /// </summary>
        void Validate();
    }
}
