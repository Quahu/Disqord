using System;
using System.Buffers;
using System.Buffers.Binary;
using System.IO;
using System.Text;
using Disqord.Voice;

namespace Disqord.Extensions.Voice;

/// <summary>
///     Writes Opus audio packets into an Ogg container (RFC 7845).
/// </summary>
public sealed class OggOpusWriter : IDisposable
{
    private static readonly uint[] CrcLookupTable = CreateCrcLookupTable();

    private readonly Stream _stream;
    private readonly uint _serialNumber;
    private uint _pageSequence;
    private long _granulePosition;
    private uint _firstRtpTimestamp;
    private bool _hasFirstTimestamp;

    private bool _isCompleted;
    private bool _isDisposed;

    /// <summary>
    ///     Gets the number of Opus packets written.
    /// </summary>
    public int PacketCount { get; private set; }

    /// <summary>
    ///     Initializes a new <see cref="OggOpusWriter"/> that writes to the specified stream.
    /// </summary>
    /// <param name="stream"> The stream to write the Ogg Opus data to. </param>
    /// <param name="channels"> The number of audio channels (1 for mono, 2 for stereo). </param>
    /// <param name="vendor"> The vendor string to write in the Ogg Opus tags. </param>
    public OggOpusWriter(Stream stream, int channels = 2, string vendor = "Disqord")
    {
        _stream = stream;
        _serialNumber = CreateSerialNumber();

        WriteIdentificationPage(channels);
        WriteTagsPage(vendor);
    }

    /// <summary>
    ///     Resets RTP timestamp tracking used by <see cref="WritePacket(ReadOnlySpan{byte}, uint)"/>.
    /// </summary>
    public void ResetRtpTimestampBase()
    {
        _firstRtpTimestamp = 0;
        _hasFirstTimestamp = false;
    }

    /// <summary>
    ///     Writes Opus DTX silence packets for the specified duration.
    /// </summary>
    /// <param name="duration"> The duration of silence to write. </param>
    public void WriteSilence(TimeSpan duration)
    {
        if (_isCompleted)
            throw new InvalidOperationException("Cannot write packets after completion.");

        var silenceFrameCount = duration.Ticks / (TimeSpan.TicksPerMillisecond * VoiceConstants.DurationMilliseconds);
        if (silenceFrameCount <= 0)
            return;

        WriteSilenceFrames(silenceFrameCount);
    }

    /// <summary>
    ///     Writes an Opus audio packet to the Ogg stream using RTP timestamp for timing.
    ///     Silence frames are automatically inserted to fill gaps between non-contiguous timestamps.
    /// </summary>
    /// <param name="packet"> The Opus packet data. </param>
    /// <param name="rtpTimestamp"> The RTP timestamp of the packet. </param>
    public void WritePacket(ReadOnlySpan<byte> packet, uint rtpTimestamp)
    {
        if (_isCompleted)
            throw new InvalidOperationException("Cannot write packets after completion.");

        if (_hasFirstTimestamp)
        {
            var delta = (long) (rtpTimestamp - _firstRtpTimestamp) - _granulePosition;
            if (delta > VoiceConstants.AudioSize)
            {
                WriteSilenceFrames(delta / VoiceConstants.AudioSize);
            }
        }
        else
        {
            _firstRtpTimestamp = rtpTimestamp;
            _hasFirstTimestamp = true;
        }

        _granulePosition += VoiceConstants.AudioSize;
        PacketCount++;
        WritePage(packet, headerType: 0, granulePosition: _granulePosition);
    }

    /// <summary>
    ///     Writes an Opus audio packet to the Ogg stream.
    ///     Each packet advances the granule position by one frame (960 samples / 20ms).
    /// </summary>
    /// <param name="packet"> The Opus packet data. </param>
    public void WritePacket(ReadOnlySpan<byte> packet)
    {
        if (_isCompleted)
            throw new InvalidOperationException("Cannot write packets after completion.");

        _granulePosition += VoiceConstants.AudioSize;
        PacketCount++;
        WritePage(packet, headerType: 0, granulePosition: _granulePosition);
    }

    /// <summary>
    ///     Writes the end-of-stream page, completing the Ogg stream.
    ///     This is called automatically by <see cref="Dispose"/>.
    /// </summary>
    public void Complete()
    {
        if (_isCompleted)
        {
            return;
        }

        _isCompleted = true;
        WritePage(ReadOnlySpan<byte>.Empty, headerType: 0x04, granulePosition: _granulePosition);
        _stream.Flush();
    }

    private void WriteSilenceFrames(long silenceFrameCount)
    {
        for (var i = 0L; i < silenceFrameCount; i++)
        {
            _granulePosition += VoiceConstants.AudioSize;
            WritePage(VoiceConstants.SilencePacket.Span, headerType: 0, granulePosition: _granulePosition);
        }
    }

    private void WriteIdentificationPage(int channels)
    {
        Span<byte> packet = stackalloc byte[19];
        "OpusHead"u8.CopyTo(packet);
        packet[8] = 1;                                             // Version
        packet[9] = (byte) channels;                               // Channels
        BinaryPrimitives.WriteUInt16LittleEndian(packet[10..], 0); // Pre-skip
        BinaryPrimitives.WriteUInt32LittleEndian(packet[12..], (uint) VoiceConstants.SamplingRate);
        BinaryPrimitives.WriteUInt16LittleEndian(packet[16..], 0); // Output gain
        packet[18] = 0;                                            // Channel mapping family

        WritePage(packet, headerType: 0x02, granulePosition: 0);
    }

    private void WriteTagsPage(string vendor)
    {
        var vendorBytes = Encoding.UTF8.GetBytes(vendor);
        var packet = new byte[8 + 4 + vendorBytes.Length + 4];

        "OpusTags"u8.CopyTo(packet);
        BinaryPrimitives.WriteUInt32LittleEndian(packet.AsSpan(8), (uint) vendorBytes.Length);
        vendorBytes.CopyTo(packet.AsSpan(12));
        BinaryPrimitives.WriteUInt32LittleEndian(packet.AsSpan(12 + vendorBytes.Length), 0);

        WritePage(packet, headerType: 0, granulePosition: 0);
    }

    private void WritePage(ReadOnlySpan<byte> packet, byte headerType, long granulePosition)
    {
        // OGG lacing: segments of up to 255 bytes. A segment of exactly 255 signals
        // "continues on the next segment." Packets whose length is a multiple of 255 need
        // a terminating zero-length segment to indicate completion.
        var segmentCount = (packet.Length / 255) + 1;
        var headerSize = 27 + segmentCount;
        var pageSize = headerSize + packet.Length;

        byte[]? rentedPage = null;
        try
        {
            var page = pageSize <= 8192
                ? stackalloc byte[pageSize]
                : rentedPage = ArrayPool<byte>.Shared.Rent(pageSize);

            page[..pageSize].Clear();
            "OggS"u8.CopyTo(page);
            page[4] = 0;
            page[5] = headerType;
            BinaryPrimitives.WriteInt64LittleEndian(page[6..], granulePosition);
            BinaryPrimitives.WriteUInt32LittleEndian(page[14..], _serialNumber);
            BinaryPrimitives.WriteUInt32LittleEndian(page[18..], _pageSequence++);
            page[26] = (byte) segmentCount;

            var remaining = packet.Length;
            for (var i = 0; i < segmentCount; i++)
            {
                var segmentLength = Math.Min(255, remaining);
                page[27 + i] = (byte) segmentLength;
                remaining -= segmentLength;
            }

            packet.CopyTo(page[headerSize..]);

            var checksum = ComputeCrc(page[..pageSize]);
            BinaryPrimitives.WriteUInt32LittleEndian(page[22..], checksum);

            _stream.Write(page[..pageSize]);
        }
        finally
        {
            if (rentedPage != null)
            {
                ArrayPool<byte>.Shared.Return(rentedPage);
            }
        }
    }

    private static uint CreateSerialNumber()
    {
        Span<byte> buffer = stackalloc byte[4];
        Random.Shared.NextBytes(buffer);
        return BinaryPrimitives.ReadUInt32LittleEndian(buffer);
    }

    private static uint ComputeCrc(ReadOnlySpan<byte> data)
    {
        var crc = 0u;
        foreach (var b in data)
        {
            crc = (crc << 8) ^ CrcLookupTable[((crc >> 24) & 0xFF) ^ b];
        }

        return crc;
    }

    private static uint[] CreateCrcLookupTable()
    {
        var table = new uint[256];
        for (var i = 0; i < table.Length; i++)
        {
            var remainder = (uint) i << 24;
            for (var j = 0; j < 8; j++)
            {
                remainder = (remainder & 0x80000000) != 0
                    ? (remainder << 1) ^ 0x04C11DB7
                    : remainder << 1;
            }

            table[i] = remainder;
        }

        return table;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;
        Complete();
    }
}
