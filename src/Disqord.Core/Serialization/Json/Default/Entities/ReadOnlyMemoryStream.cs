using System;
using System.IO;

namespace Disqord.Serialization.Json.Default;

internal sealed class ReadOnlyMemoryStream : Stream
{
    public override bool CanRead => true;

    public override bool CanSeek => true;

    public override bool CanWrite => false;

    public override long Length => _memory.Length;

    public override long Position
    {
        get => _position;
        set => throw new NotSupportedException();
    }

    private readonly ReadOnlyMemory<byte> _memory;
    private int _position;

    public ReadOnlyMemoryStream(ReadOnlyMemory<byte> memory)
    {
        _memory = memory;
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        return Read(buffer.AsSpan(offset, count));
    }

    public override int Read(Span<byte> buffer)
    {
        var length = _memory.Length - _position;
        if (length > 0)
        {
            if (length > buffer.Length)
                length = buffer.Length;

            _memory.Span.Slice(_position, length).CopyTo(buffer);
            _position += length;
        }

        return length;
    }

    public override void Flush()
        => throw new NotSupportedException();

    public override long Seek(long offset, SeekOrigin origin)
        => throw new NotSupportedException();

    public override void SetLength(long value)
        => throw new NotSupportedException();

    public override void Write(byte[] buffer, int offset, int count)
        => throw new NotSupportedException();
}
