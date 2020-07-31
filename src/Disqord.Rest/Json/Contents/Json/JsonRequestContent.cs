using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal abstract class JsonRequestContent : JsonModel, IRequestContent
    {
        public HttpContent Prepare(IJsonSerializer serializer, RestRequestOptions options)
            => PrepareFor(this, serializer, options);

        public static HttpContent PrepareFor(object model, IJsonSerializer serializer, RestRequestOptions options)
        {
            var bytes = serializer.Serialize(model);
            if (Library.Debug.DumpJson)
                Library.Debug.DumpWriter.WriteLine(Encoding.UTF8.GetString(bytes.Span));

            var content = new ReadOnlyMemoryContent(bytes);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json")
            {
                CharSet = Encoding.UTF8.WebName
            };
            return content;
        }
    }
}
