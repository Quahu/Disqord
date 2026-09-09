using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Http;

namespace Disqord.Rest;

internal sealed class HttpResponseStream(IHttpResponse response, Stream stream) : Stream
{
    public override bool CanRead => stream.CanRead;

    public override bool CanSeek => stream.CanSeek;

    public override bool CanWrite => false;

    public override long Length => stream.Length;

    public override long Position
    {
        get => stream.Position;
        set => stream.Position = value;
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        return stream.Read(buffer, offset, count);
    }

    public override int Read(Span<byte> buffer)
    {
        return stream.Read(buffer);
    }

    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        return stream.ReadAsync(buffer, offset, count, cancellationToken);
    }

    public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        return stream.ReadAsync(buffer, cancellationToken);
    }

    public override int ReadByte()
    {
        return stream.ReadByte();
    }

    public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
    {
        return stream.CopyToAsync(destination, bufferSize, cancellationToken);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        return stream.Seek(offset, origin);
    }

    public override void Flush()
    {
        stream.Flush();
    }

    public override void SetLength(long value)
    {
        throw new NotSupportedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotSupportedException();
    }

    public override void Write(ReadOnlySpan<byte> buffer)
    {
        throw new NotSupportedException();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            stream.Dispose();
            response.Dispose();
        }

        base.Dispose(disposing);
    }

    public override async ValueTask DisposeAsync()
    {
        await stream.DisposeAsync().ConfigureAwait(false);
        response.Dispose();
    }
}
