using System.Text;

namespace Disqord.Http;

public class StringHttpRequestContent : ReadOnlyMemoryHttpRequestContent
{
    public StringHttpRequestContent(string content)
        : base(Encoding.UTF8.GetBytes(content))
    {
        Headers["Content-Type"] = "text/plain; charset=utf-8";
    }
}