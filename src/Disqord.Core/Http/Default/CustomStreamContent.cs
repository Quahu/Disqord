using System.IO;
using System.Net.Http;

namespace Disqord.Http.Default;

internal class CustomStreamContent : StreamContent
{
    private readonly bool _dispose;

    public CustomStreamContent(Stream stream, bool dispose = false)
        : base(stream)
    {
        _dispose = dispose;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(_dispose);
    }
}
