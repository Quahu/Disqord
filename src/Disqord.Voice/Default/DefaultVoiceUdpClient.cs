using System;
using System.Buffers.Binary;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using Disqord.Udp;
using Qommon;
using Qommon.Pooling;

namespace Disqord.Voice.Default;

public class DefaultVoiceUdpClient : IVoiceUdpClient, IValueTaskSource
{
    public IUdpClient UdpClient { get; }

    public uint Ssrc { get; }

    public ReadOnlyMemory<byte> EncryptionKey => _encryptionKey;

    private readonly byte[] _encryptionKey;

    public string HostName { get; }

    public int Port { get; }

    public IVoiceEncryption Encryption { get; }

    public string? RemoteHostName { get; private set; }

    public ushort? RemotePort { get; private set; }

    public ushort Sequence => _sequence;

    public uint Timestamp => _timestamp;

    private ushort _sequence;
    private uint _timestamp;

    private const int TaskPendingState = 0;
    private const int TaskCompletedState = 1;

    private volatile bool _isDisposed;

    private int _taskState = TaskCompletedState;
    private ManualResetValueTaskSourceCore<bool> _mrvtsc = new()
    {
        RunContinuationsAsynchronously = true
    };

    public DefaultVoiceUdpClient(
        uint ssrc,
        byte[] encryptionKey,
        string hostName,
        int port,
        IVoiceEncryption encryption,
        IUdpClientFactory udpClientFactory)
    {
        Ssrc = ssrc;
        _encryptionKey = encryptionKey;
        HostName = hostName;
        Port = port;
        Encryption = encryption;

        UdpClient = udpClientFactory.CreateClient();
    }

    /// <inheritdoc/>
    public void OnSynchronizerTick()
    {
        if (Interlocked.Exchange(ref _taskState, TaskCompletedState) != TaskPendingState)
            return;

        _mrvtsc.SetResult(true);
    }

    /// <inheritdoc/>
    public async ValueTask ConnectAsync(CancellationToken cancellationToken = default)
    {
        await UdpClient.ConnectAsync(HostName, Port, cancellationToken).ConfigureAwait(false);

        using (var packetArray = RentedArray<byte>.Rent(74))
        {
            WriteDiscovery(packetArray);
            await UdpClient.SendAsync(packetArray, cancellationToken).ConfigureAwait(false);

            await UdpClient.ReceiveAsync(packetArray, cancellationToken).ConfigureAwait(false);
            ReadDiscovery(packetArray);
        }
    }

    /// <inheritdoc/>
    public ValueTask CloseAsync(CancellationToken cancellationToken = default)
    {
        return UdpClient.CloseAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async ValueTask SendAsync(ReadOnlyMemory<byte> opus, CancellationToken cancellationToken = default)
    {
        if (_isDisposed)
            Throw.ObjectDisposedException(GetType().Name);

        if (opus.Length == 0)
            return;

        if (Interlocked.Exchange(ref _taskState, TaskPendingState) != TaskCompletedState)
            Throw.InvalidOperationException($"Invalid {nameof(SendAsync)} call.");

        _mrvtsc.Reset();
        await new ValueTask(this, _mrvtsc.Version).ConfigureAwait(false);

        var encryptedLength = Encryption.GetEncryptedLength(opus.Length);
        using (var packetArray = RentedArray<byte>.Rent(VoiceConstants.RtpHeaderSize + encryptedLength))
        {
            WriteVoicePacket(packetArray, opus.Span);

            await UdpClient.SendAsync(packetArray, cancellationToken).ConfigureAwait(false);

            _sequence++;
            _timestamp += VoiceConstants.AudioSize;
        }
    }

    private void WriteDiscovery(Span<byte> packet)
    {
        BinaryPrimitives.WriteInt16BigEndian(packet, 1);
        BinaryPrimitives.WriteInt16BigEndian(packet[2..], 70);
        BinaryPrimitives.WriteUInt32BigEndian(packet[4..], Ssrc);
    }

    private unsafe void ReadDiscovery(ReadOnlySpan<byte> packet)
    {
        fixed (byte* ptr = packet[8..])
        {
            RemoteHostName = new string((sbyte*) ptr, 0, 64);
        }

        RemotePort = BinaryPrimitives.ReadUInt16BigEndian(packet[72..]);
    }

    private void WriteVoicePacket(Span<byte> packet, ReadOnlySpan<byte> opus)
    {
        packet[0] = 0x80;
        packet[1] = 0x78;
        BinaryPrimitives.WriteUInt16BigEndian(packet[2..], _sequence);
        BinaryPrimitives.WriteUInt32BigEndian(packet[4..], _timestamp);
        BinaryPrimitives.WriteUInt32BigEndian(packet[8..], Ssrc);

        try
        {
            Encryption.Encrypt(packet[..VoiceConstants.RtpHeaderSize], packet[VoiceConstants.RtpHeaderSize..], opus, _encryptionKey);
        }
        catch (Exception ex)
        {
            throw new VoiceEncryptionException("Failed to encrypt the voice packet.", ex);
        }
    }

    public void Dispose()
    {
        if (_isDisposed)
            return;

        if (Interlocked.Exchange(ref _taskState, TaskPendingState) == TaskPendingState)
        {
            _mrvtsc.SetException(new ObjectDisposedException(GetType().Name));
        }

        _isDisposed = true;
        UdpClient.Dispose();
    }

    void IValueTaskSource.GetResult(short token)
    {
        _mrvtsc.GetResult(token);
    }

    ValueTaskSourceStatus IValueTaskSource.GetStatus(short token)
    {
        return _mrvtsc.GetStatus(token);
    }

    void IValueTaskSource.OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags)
    {
        _mrvtsc.OnCompleted(continuation, state, token, flags);
    }
}
