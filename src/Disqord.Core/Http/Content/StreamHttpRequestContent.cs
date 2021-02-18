using System;
using System.IO;

namespace Disqord.Http.Default
{
    public class StreamHttpRequestContent : HttpRequestContent
    {
        public Stream Stream { get; }

        public bool ShouldDispose { get; }

        public StreamHttpRequestContent(Stream stream, bool shouldDispose = false)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            Stream = stream;
            ShouldDispose = shouldDispose;
        }

        public override void Dispose()
        {
            if (ShouldDispose)
                Stream.Dispose();
        }
    }
}
