using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Qommon.Pooling;

namespace Disqord.Extensions.Voice;

internal class OggReader : IAsyncEnumerable<RentedArray<byte>>
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
    private readonly RentedArray<byte> _buffer;
    private bool _returnedEnumerator;

    private ArraySegment<byte> BufferSegment => _buffer.AsArraySegment().Slice(_offset, _bytesRead - _offset);

    public OggReader(Stream stream)
    {
        _stream = stream;
        _buffer = RentedArray<byte>.Rent(BufferSize);
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
            for (var i = 0; i < BufferSegment.Count; i++)
            {
                var index = -1;
                var foundMagic = true;
                for (int x = 0, j = 0; x < Magic.Length; x++, j++)
                {
                    index = i + j;
                    if (index == BufferSegment.Count)
                    {
                        index = 0;
                        j = 0;
                        i = 0;

                        if (!await ReadAsync(cancellationToken).ConfigureAwait(false))
                            return false;
                    }

                    if (BufferSegment[index] != Magic[x])
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
            if (BufferSegment.Count < headerLengthLeft)
            {
                headerLengthLeft -= BufferSegment.Count;
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
            if (BufferSegment.Count < segmentLengthsLeft)
            {
                BufferSegment.CopyTo(segmentLengths[offset..]);
                segmentLengthsLeft -= BufferSegment.Count;
            }
            else
            {
                BufferSegment[..segmentLengthsLeft].CopyTo(segmentLengths[offset..]);
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
            if (BufferSegment.Count < packetLengthLeft)
            {
                BufferSegment.CopyTo(packet[offset..]);
                packetLengthLeft -= BufferSegment.Count;
            }
            else
            {
                BufferSegment[..packetLengthLeft].CopyTo(packet[offset..]);
                _offset += packetLengthLeft;
                return true;
            }
        }
        while (await ReadAsync(cancellationToken).ConfigureAwait(false));

        return false;
    }

    public async IAsyncEnumerator<RentedArray<byte>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        using (_buffer)
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
                        if (segmentLength >= 255)
                            continue;

                        var packet = RentedArray<byte>.Rent(packetLength);
                        try
                        {
                            if (!await ReadPacketAsync(packet, cancellationToken).ConfigureAwait(false))
                            {
                                yield break;
                            }
                        }
                        catch
                        {
                            packet.Dispose();
                            throw;
                        }

                        packetLength = 0;
                        yield return packet;
                    }
                }
            }
        }
    }
}
