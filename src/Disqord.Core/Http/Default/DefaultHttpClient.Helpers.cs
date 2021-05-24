using System;
using System.Net.Http;

namespace Disqord.Http.Default
{
    public sealed partial class DefaultHttpClient
    {
        public static HttpMethod GetHttpMethod(HttpRequestMethod method) => method switch
        {
            HttpRequestMethod.Get => HttpMethod.Get,
            HttpRequestMethod.Post => HttpMethod.Post,
            HttpRequestMethod.Patch => HttpMethod.Patch,
            HttpRequestMethod.Put => HttpMethod.Put,
            HttpRequestMethod.Delete => HttpMethod.Delete,
            _ => throw new ArgumentOutOfRangeException(nameof(method)),
        };

        public static HttpContent GetHttpContent(HttpRequestContent content)
        {
            switch (content)
            {
                case ReadOnlyMemoryHttpRequestContent readOnlyMemoryHttpRequestContent:
                    return new ReadOnlyMemoryContent(readOnlyMemoryHttpRequestContent.Memory);

                case StreamHttpRequestContent streamHttpRequestContent:
                    return new CustomStreamContent(streamHttpRequestContent.Stream, streamHttpRequestContent.ShouldDispose);

                case MultipartFormDataHttpRequestContent multipartFormDataHttpRequestContent:
                {
                    var multipartFormDataContent = new MultipartFormDataContent(multipartFormDataHttpRequestContent.Boundary);
                    for (var i = 0; i < multipartFormDataHttpRequestContent.FormData.Count; i++)
                    {
                        var formData = multipartFormDataHttpRequestContent.FormData[i];
                        var httpContent = GetHttpContent(formData.Content);
                        if (formData.FileName != null)
                            multipartFormDataContent.Add(httpContent, formData.Name, formData.FileName);
                        else
                            multipartFormDataContent.Add(httpContent, formData.Name);
                    }

                    return multipartFormDataContent;
                }

                default:
                    throw new InvalidOperationException("Unsupported HTTP request content type.");
            }
        }

        public static HttpRequestMessage GetHttpMessage(IHttpRequest request)
        {
            var requestMessage = new HttpRequestMessage(GetHttpMethod(request.Method), request.Uri);
            if (request.Headers != null)
            {
                foreach (var header in request.Headers)
                    requestMessage.Headers.Set(header.Key, header.Value);
            }

            if (request.Content != null)
            {
                requestMessage.Content = GetHttpContent(request.Content);
                if (request.Content.Headers != null)
                {
                    foreach (var header in request.Content.Headers)
                        requestMessage.Content.Headers.Set(header.Key, header.Value);
                }
            }

            return requestMessage;
        }
    }
}
