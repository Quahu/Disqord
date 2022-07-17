using System.Collections.Generic;

namespace Disqord.Http;

public class MultipartFormDataHttpRequestContent : HttpRequestContent
{
    public string Boundary { get; }

    public List<(HttpRequestContent Content, string Name, string? FileName)> FormData { get; }

    public MultipartFormDataHttpRequestContent(string boundary)
    {
        Boundary = boundary;
        FormData = new List<(HttpRequestContent, string, string?)>();
    }

    public override void Dispose()
    {
        for (var i = 0; i < FormData.Count; i++)
        {
            var data = FormData[i];
            data.Content.Dispose();
        }
    }
}
