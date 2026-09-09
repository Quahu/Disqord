using Disqord.Http;

namespace Disqord.Tests.Fakes;

internal sealed class FakeHttpResponse : IHttpResponse
{
    public HttpResponseStatusCode StatusCode { get; init; } = HttpResponseStatusCode.OK;

    public string? ReasonPhrase { get; init; } = "OK";

    public byte[] Body { get; init; } = [];

    public IDictionary<string, string> Headers { get; } = new Dictionary<string, string>();

    public bool IsDisposed { get; private set; }

    public Task<Stream> ReadAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult<Stream>(new MemoryStream(Body, false));
    }

    public void Dispose()
    {
        IsDisposed = true;
    }
}
