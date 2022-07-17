using System;

namespace Disqord.Http;

public class ReadOnlyMemoryHttpRequestContent : HttpRequestContent
{
    public ReadOnlyMemory<byte> Memory { get; }

    public ReadOnlyMemoryHttpRequestContent(ReadOnlyMemory<byte> memory)
    {
        Memory = memory;
    }
}