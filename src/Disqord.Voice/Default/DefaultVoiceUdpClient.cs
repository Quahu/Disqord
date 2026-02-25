using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using Disqord.Udp;
using Microsoft.Extensions.Logging;
using Qommon;
using Qommon.Pooling;

namespace Disqord.Voice.Default;

public class DefaultVoiceUdpClient : IVoiceUdpClient, IValueTaskSource
{
    public IUdpClient UdpClient { get; }

    public uint Ssrc { get; }

    public ReadOnlyMemory<byte> EncryptionKey => _encryptionKey;

    public string HostName { get; }

    public int Port { get; }

    public IVoiceEncryption Encryption { get; }

    public string? RemoteHostName { get; private set; }

    public ushort? RemotePort { get; private set; }

    public ushort Sequence => _sequence;

    public uint Timestamp => _timestamp;

    private readonly ILogger _logger;
    private byte[] _encryptionKey = Array.Empty<byte>();
    private DaveEncryptor? _daveEncryptor;

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
        string hostName,
        int port,
        ILogger logger,
        IVoiceEncryption encryption,
        IUdpClientFactory udpClientFactory)
    {
        Ssrc = ssrc;
        HostName = hostName;
        Port = port;
        _logger = logger;
        Encryption = encryption;

        UdpClient = udpClientFactory.CreateClient();
    }

    public void Initialize(byte[] encryptionKey, DaveEncryptor? daveEncryptor)
    {
        _encryptionKey = encryptionKey;
        _daveEncryptor = daveEncryptor;
        daveEncryptor?.AssignSsrcToCodec(Ssrc, Dave.Codec.Opus);
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

            _logger.LogDebug("Awaiting discovery packet...");
            await UdpClient.ReceiveAsync(packetArray, cancellationToken).ConfigureAwait(false);
            ReadDiscovery(packetArray);

            _logger.LogDebug("Discovery packet received: {RemoteHostName}:{RemotePort}.", RemoteHostName, RemotePort);
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

        using (var packet = CreateVoicePacket(opus.Span))
        {
            await UdpClient.SendAsync(packet, cancellationToken).ConfigureAwait(false);
        }

        _sequence++;
        _timestamp += VoiceConstants.AudioSize;
    }

    private RentedArray<byte> CreateVoicePacket(ReadOnlySpan<byte> opus)
    {
        byte[]? rentedDaveBuffer = null;
        RentedArray<byte> packetArray = default;
        try
        {
            // With DAVE, opus is first encrypted before voice encryption.
            if (_daveEncryptor != null)
            {
                var maxDaveSize = (int) _daveEncryptor.GetMaxCiphertextByteSize(Dave.MediaType.Audio, (nuint) opus.Length);
                var daveBuffer = maxDaveSize <= 4096
                    ? stackalloc byte[maxDaveSize]
                    : rentedDaveBuffer = ArrayPool<byte>.Shared.Rent(maxDaveSize);

                var result = _daveEncryptor.Encrypt(Dave.MediaType.Audio, Ssrc, opus, daveBuffer, out var bytesWritten);
                if (result != Dave.EncryptorResultCode.Success)
                {
                    throw new VoiceEncryptionException($"DAVE encryption failed with result: {result}.");
                }

                var ciphertext = daveBuffer[..(int) bytesWritten];
                packetArray = RentPacketAndEncrypt(ciphertext);
            }
            else
            {
                packetArray = RentPacketAndEncrypt(opus);
            }

            return packetArray;
        }
        catch (VoiceEncryptionException)
        {
            packetArray.Dispose();
            throw;
        }
        catch (Exception ex)
        {
            packetArray.Dispose();
            throw new VoiceEncryptionException("Failed to encrypt the voice packet.", ex);
        }
        finally
        {
            if (rentedDaveBuffer != null)
            {
                ArrayPool<byte>.Shared.Return(rentedDaveBuffer);
            }
        }
    }

    private RentedArray<byte> RentPacketAndEncrypt(ReadOnlySpan<byte> audio)
    {
        var packetSize = VoiceConstants.RtpHeaderSize + Encryption.GetEncryptedLength(audio.Length);
        var packetArray = RentedArray<byte>.Rent(packetSize);
        try
        {
            var packet = packetArray.AsSpan();
            packet[0] = 0x80;
            packet[1] = 0x78;
            BinaryPrimitives.WriteUInt16BigEndian(packet[2..], _sequence);
            BinaryPrimitives.WriteUInt32BigEndian(packet[4..], _timestamp);
            BinaryPrimitives.WriteUInt32BigEndian(packet[8..], Ssrc);

            Encryption.Encrypt(
                packet[..VoiceConstants.RtpHeaderSize],
                packet[VoiceConstants.RtpHeaderSize..],
                audio, _encryptionKey);

            return packetArray;
        }
        catch
        {
            packetArray.Dispose();
            throw;
        }
    }

    private void WriteDiscovery(Span<byte> packet)
    {
        packet.Clear();
        BinaryPrimitives.WriteInt16BigEndian(packet, 1);
        BinaryPrimitives.WriteInt16BigEndian(packet[2..], 70);
        BinaryPrimitives.WriteUInt32BigEndian(packet[4..], Ssrc);
    }

    private void ReadDiscovery(ReadOnlySpan<byte> packet)
    {
        var addressSpan = packet.Slice(8, 64);
        var nullIndex = addressSpan.IndexOf((byte) 0);
        if (nullIndex >= 0)
            addressSpan = addressSpan[..nullIndex];

        RemoteHostName = Encoding.UTF8.GetString(addressSpan);
        RemotePort = BinaryPrimitives.ReadUInt16BigEndian(packet[72..]);
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
