using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Qommon.Pooling;

namespace Disqord.Extensions.Voice;

internal class OggReader : IAsyncEnumerable<Memory<byte>>
{
    // 4 magic
    // 1 stream structure version
    // 1 header type flag
    // 8 granule position
    // 4 bitstream serial number
    // 4 page sequence number
    // 4 crc checksum
    // 1 page segment count
    // segment table

    private static ReadOnlySpan<byte> Magic => "OggS"u8;

    private const int HeaderSize = 27;
    private const int BufferSize = 8192;

    private readonly Stream _stream;

    private bool NeedsRead => _offset >= _bytesRead;

    private int _offset;
    private int _bytesRead;
    private readonly byte[] _buffer;
    private bool _returnedEnumerator;

    private ArraySegment<byte> BufferSegment => new(_buffer, _offset, _bytesRead - _offset);

    public OggReader(Stream stream)
    {
        _stream = stream;
        _buffer = new byte[BufferSize];
    }

    private async Task<bool> ReadAsync(CancellationToken cancellationToken)
    {
        _offset = 0;
        return (_bytesRead = await _stream.ReadAsync(_buffer, cancellationToken).ConfigureAwait(false)) != 0;
    }

    private async Task<bool> FindMagicAsync(CancellationToken cancellationToken)
    {
        if (NeedsRead)
        {
            if (!await ReadAsync(cancellationToken).ConfigureAwait(false))
                return false;
        }

        do
        {
            var bufferSegment = BufferSegment;
            for (var i = 0; i < bufferSegment.Count; i++)
            {
                var index = -1;
                var foundMagic = true;
                for (int x = 0, j = 0; x < Magic.Length; x++, j++)
                {
                    index = i + j;
                    if (index == bufferSegment.Count)
                    {
                        index = 0;
                        j = 0;
                        i = 0;

                        if (!await ReadAsync(cancellationToken).ConfigureAwait(false))
                            return false;

                        bufferSegment = BufferSegment;
                    }

                    if (bufferSegment[index] != Magic[x])
                    {
                        foundMagic = false;
                        break;
                    }
                }

                if (foundMagic)
                {
                    _offset += index + 1;
                    return true;
                }
            }
        }
        while (await ReadAsync(cancellationToken).ConfigureAwait(false));

        return false;
    }

    private async Task<bool> FindSegmentCountAsync(CancellationToken cancellationToken)
    {
        if (NeedsRead)
        {
            if (!await ReadAsync(cancellationToken).ConfigureAwait(false))
                return false;
        }

        var headerLengthLeft = HeaderSize - Magic.Length;
        do
        {
            var bufferSegmentCount = BufferSegment.Count;
            if (bufferSegmentCount < headerLengthLeft)
            {
                headerLengthLeft -= bufferSegmentCount;
            }
            else
            {
                _offset += headerLengthLeft - 1;
                return true;
            }
        }
        while (await ReadAsync(cancellationToken).ConfigureAwait(false));

        return false;
    }

    private async Task<bool> ReadSegmentLengthsAsync(RentedArray<byte> segmentLengths, CancellationToken cancellationToken)
    {
        _offset++;

        if (NeedsRead)
        {
            if (!await ReadAsync(cancellationToken).ConfigureAwait(false))
                return false;
        }

        var segmentLengthsLeft = segmentLengths.Length;
        do
        {
            var offset = segmentLengths.Length - segmentLengthsLeft;
            var bufferSegment = BufferSegment;
            if (bufferSegment.Count < segmentLengthsLeft)
            {
                bufferSegment.CopyTo(segmentLengths[offset..]);
                segmentLengthsLeft -= bufferSegment.Count;
            }
            else
            {
                bufferSegment[..segmentLengthsLeft].CopyTo(segmentLengths[offset..]);
                _offset += segmentLengthsLeft;
                return true;
            }
        }
        while (await ReadAsync(cancellationToken).ConfigureAwait(false));

        return false;
    }

    private async Task<bool> ReadPacketAsync(RentedArray<byte> packet, CancellationToken cancellationToken)
    {
        if (NeedsRead)
        {
            if (!await ReadAsync(cancellationToken).ConfigureAwait(false))
                return false;
        }

        var packetLengthLeft = packet.Length;
        do
        {
            var offset = packet.Length - packetLengthLeft;
            var bufferSegment = BufferSegment;
            if (bufferSegment.Count < packetLengthLeft)
            {
                bufferSegment.CopyTo(packet[offset..]);
                packetLengthLeft -= bufferSegment.Count;
            }
            else
            {
                bufferSegment[..packetLengthLeft].CopyTo(packet[offset..]);
                _offset += packetLengthLeft;
                return true;
            }
        }
        while (await ReadAsync(cancellationToken).ConfigureAwait(false));

        return false;
    }

    public async IAsyncEnumerator<Memory<byte>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        if (_returnedEnumerator)
            yield break;

        _returnedEnumerator = true;

        if (!await ReadAsync(cancellationToken).ConfigureAwait(false))
            yield break;

        while (true)
        {
            if (!await FindMagicAsync(cancellationToken).ConfigureAwait(false))
                yield break;

            if (!await FindSegmentCountAsync(cancellationToken).ConfigureAwait(false))
                yield break;

            var segmentCount = BufferSegment[0];
            using (var segmentLengths = RentedArray<byte>.Rent(segmentCount))
            {
                if (!await ReadSegmentLengthsAsync(segmentLengths, cancellationToken).ConfigureAwait(false))
                    yield break;

                var packetLength = 0;
                for (var i = 0; i < segmentCount; i++)
                {
                    var segmentLength = segmentLengths[i];
                    packetLength += segmentLength;
                    if (segmentLength == 255)
                        continue;

                    using (var packet = RentedArray<byte>.Rent(packetLength))
                    {
                        if (!await ReadPacketAsync(packet, cancellationToken).ConfigureAwait(false))
                        {
                            yield break;
                        }

                        packetLength = 0;
                        yield return packet;
                    }
                }
            }
        }
    }
}
