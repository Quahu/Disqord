using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal abstract class JsonRequestContent : IRequestContent
    {
        public HttpContent Prepare(IJsonSerializer serializer, RestRequestOptions options)
            => PrepareFor(this, serializer, options);

        public static HttpContent PrepareFor(object obj, IJsonSerializer serializer, RestRequestOptions options)
        {
            Dictionary<string, object> additionalFields = null;
            if (options != null)
            {
                if (options.Password != null || options.MfaCode != null)
                {
                    additionalFields = new Dictionary<string, object>(2);
                    if (options.Password != null)
                    {
                        additionalFields.Add("password", options.Password);
                    }

                    if (options.MfaCode != null)
                    {
                        additionalFields.Add("code", options.MfaCode);
                    }
                }
            }

            var bytes = serializer.Serialize(obj, additionalFields);

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
