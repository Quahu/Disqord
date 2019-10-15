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

            var bytes = serializer.Serialize(this, additionalFields);
            var headerValue = new MediaTypeHeaderValue("application/json")
            {
                CharSet = Encoding.UTF8.WebName
            };
            var content = new ByteArrayContent(bytes);
            content.Headers.ContentType = headerValue;
            return content;
        }
    }
}
