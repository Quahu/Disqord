using System.IO;
using Qommon;

namespace Disqord.Http.Default;

public class StreamHttpRequestContent : HttpRequestContent
{
    public Stream Stream { get; }

    public bool ShouldDispose { get; }

    public StreamHttpRequestContent(Stream stream, bool shouldDispose = false)
    {
        Guard.IsNotNull(stream);

        Stream = stream;
        ShouldDispose = shouldDispose;
    }

    public override void Dispose()
    {
        if (ShouldDispose)
            Stream.Dispose();
    }
}