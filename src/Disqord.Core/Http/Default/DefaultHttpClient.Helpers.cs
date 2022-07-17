using System;
using System.Net.Http;

namespace Disqord.Http.Default;

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
        HttpContent httpContent;
        switch (content)
        {
            case ReadOnlyMemoryHttpRequestContent readOnlyMemoryHttpRequestContent:
            {
                httpContent = new ReadOnlyMemoryContent(readOnlyMemoryHttpRequestContent.Memory);
                break;
            }
            case StreamHttpRequestContent streamHttpRequestContent:
            {
                httpContent = new CustomStreamContent(streamHttpRequestContent.Stream, streamHttpRequestContent.ShouldDispose);
                break;
            }
            case MultipartFormDataHttpRequestContent multipartFormDataHttpRequestContent:
            {
                var multipartFormDataContent = new MultipartFormDataContent(multipartFormDataHttpRequestContent.Boundary);
                for (var i = 0; i < multipartFormDataHttpRequestContent.FormData.Count; i++)
                {
                    var formData = multipartFormDataHttpRequestContent.FormData[i];
                    var formContent = GetHttpContent(formData.Content);
                    if (formData.FileName != null)
                        multipartFormDataContent.Add(formContent, formData.Name, formData.FileName);
                    else
                        multipartFormDataContent.Add(formContent, formData.Name);
                }

                httpContent = multipartFormDataContent;
                break;
            }
            default:
            {
                throw new InvalidOperationException("Unsupported HTTP request content type.");
            }
        }

        if (content.Headers != null)
        {
            foreach (var header in content.Headers)
                httpContent.Headers.Set(header.Key, header.Value);
        }

        return httpContent;
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
            requestMessage.Content = GetHttpContent(request.Content);

        return requestMessage;
    }
}