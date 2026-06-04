using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Qommon.Pooling;

namespace Disqord.Voice;

/// <summary>
///     Represents a received voice packet containing decrypted Opus audio data.
/// </summary>
public readonly struct VoiceReceivePacket : IDisposable
{
    /// <summary>
    ///     Gets the RTP sequence number of this packet.
    /// </summary>
    public ushort Sequence => State.GetSequence(_generation);

    /// <summary>
    ///     Gets the RTP timestamp of this packet.
    /// </summary>
    public uint Timestamp => State.GetTimestamp(_generation);

    /// <summary>
    ///     Gets the SSRC (synchronization source) identifier of the audio stream.
    /// </summary>
    public uint Ssrc => State.GetSsrc(_generation);

    /// <summary>
    ///     Gets or sets the ID of the user who sent this packet,
    ///     or <see langword="null"/> if the SSRC has not yet been mapped to a user.
    /// </summary>
    public Snowflake? UserId
    {
        get => State.GetUserId(_generation);
        set => State.SetUserId(_generation, value);
    }

    /// <summary>
    ///     Gets the decrypted Opus audio payload.
    /// </summary>
    public ReadOnlyMemory<byte> Opus => State.GetOpus(_generation);

    private PacketState State
    {
        get
        {
            var state = _state;
            if (state == null)
            {
                ThrowDisposed();
            }

            return state;
        }
    }

    private readonly PacketState? _state;
    private readonly int _generation;

    internal VoiceReceivePacket(PacketState state, int generation)
    {
        _state = state;
        _generation = generation;
    }

    [DoesNotReturn]
    private static void ThrowDisposed()
    {
        throw new ObjectDisposedException(nameof(VoiceReceivePacket));
    }

    /// <summary>
    ///     Returns this packet's pooled resources to their pools.
    ///     Safe to call multiple times across struct copies.
    /// </summary>
    public void Dispose()
    {
        _state?.Dispose(_generation);
    }

    internal sealed class PacketState
    {
        private readonly ObjectPool<PacketState> _pool;

        private int _nextGeneration;
        private int _version;

        private ushort _sequence;
        private uint _timestamp;
        private uint _ssrc;
        private Snowflake? _userId;
        private ReadOnlyMemory<byte> _opus;
        private byte[]? _rentedArray;

        internal PacketState(ObjectPool<PacketState> pool)
        {
            _pool = pool;
        }

        internal VoiceReceivePacket Initialize(ushort sequence, uint timestamp, uint ssrc, Snowflake? userId, ReadOnlyMemory<byte> opus, byte[]? rentedArray)
        {
            var generation = unchecked(_nextGeneration + 1);
            if (generation == 0 || generation == int.MinValue)
            {
                generation = 1;
            }

            _nextGeneration = generation;
            _sequence = sequence;
            _timestamp = timestamp;
            _ssrc = ssrc;
            _userId = userId;
            _opus = opus;
            _rentedArray = rentedArray;
            Volatile.Write(ref _version, generation);

            return new VoiceReceivePacket(this, generation);
        }

        internal void ResetForPooling()
        {
            _sequence = 0;
            _timestamp = 0;
            _ssrc = 0;
            _userId = null;
            _opus = default;
            _rentedArray = null;
            Volatile.Write(ref _version, 0);
        }

        internal ushort GetSequence(int expectedGeneration)
        {
            Validate(expectedGeneration);
            return _sequence;
        }

        internal uint GetTimestamp(int expectedGeneration)
        {
            Validate(expectedGeneration);
            return _timestamp;
        }

        internal uint GetSsrc(int expectedGeneration)
        {
            Validate(expectedGeneration);
            return _ssrc;
        }

        internal Snowflake? GetUserId(int expectedGeneration)
        {
            Validate(expectedGeneration);
            return _userId;
        }

        internal void SetUserId(int expectedGeneration, Snowflake? userId)
        {
            Validate(expectedGeneration);
            _userId = userId;
        }

        internal ReadOnlyMemory<byte> GetOpus(int expectedGeneration)
        {
            Validate(expectedGeneration);
            return _opus;
        }

        private void Validate(int expectedGeneration)
        {
            if (Volatile.Read(ref _version) != expectedGeneration)
            {
                ThrowDisposed();
            }
        }

        internal void Dispose(int expectedGeneration)
        {
            if (Interlocked.CompareExchange(ref _version, -expectedGeneration, expectedGeneration) != expectedGeneration)
            {
                return;
            }

            var rentedArray = Interlocked.Exchange(ref _rentedArray, null);
            if (rentedArray != null)
            {
                ArrayPool<byte>.Shared.Return(rentedArray);
            }

            _pool.Return(this);
        }
    }
}
