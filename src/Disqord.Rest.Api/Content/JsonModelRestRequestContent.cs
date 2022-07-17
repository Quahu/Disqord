using System.Text;
using Disqord.Http;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api;

public class JsonModelRestRequestContent : JsonModel, IRestRequestContent
{
    public JsonModelRestRequestContent()
    { }

    /// <inheritdoc/>
    public HttpRequestContent CreateHttpContent(IJsonSerializer serializer, IRestRequestOptions? options = null)
    {
        return FromObject(this, serializer);
    }

    public static HttpRequestContent FromObject(object obj, IJsonSerializer serializer)
    {
        var bytes = serializer.Serialize(obj);
        if (Library.Debug.DumpJson)
            Library.Debug.DumpWriter.WriteLine(Encoding.UTF8.GetString(bytes.Span));

        var content = new ReadOnlyMemoryHttpRequestContent(bytes);
        content.Headers["Content-Type"] = "application/json; charset=utf-8";
        return content;
    }
}
