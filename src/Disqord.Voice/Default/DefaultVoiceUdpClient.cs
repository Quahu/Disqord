using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using Disqord.Udp;
using Disqord.Voice.Api;
using Microsoft.Extensions.Logging;
using Qommon;
using Qommon.Pooling;

namespace Disqord.Voice.Default;

public class DefaultVoiceUdpClient : IVoiceUdpClient, IValueTaskSource
{
    private const int MaxUdpDatagramSize = 65535;
    private const int ReceivePacketPoolCapacity = 16;

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
    private readonly ObjectPool<VoiceReceivePacket.PacketState> _receivePacketPool;
    private byte[] _encryptionKey = Array.Empty<byte>();
    private DaveEncryptor? _daveEncryptor;
    private volatile DaveProtocolHandler? _daveHandler;
    private volatile IVoiceGatewayClient? _gateway;
    private readonly byte[] _receiveBuffer = new byte[MaxUdpDatagramSize];

    private ushort _sequence;
    private uint _timestamp;

    private const int TaskPendingState = 0;
    private const int TaskCompletedState = 1;

    private int _isDisposed;

    private int _isSending;
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
        var receivePacketPoolPolicy = new ReceivePacketStatePoolPolicy();
        _receivePacketPool = new DefaultObjectPool<VoiceReceivePacket.PacketState>(receivePacketPoolPolicy, ReceivePacketPoolCapacity);
        receivePacketPoolPolicy.Bind(_receivePacketPool);

        UdpClient = udpClientFactory.CreateClient();
    }

    public void Initialize(byte[] encryptionKey, DaveEncryptor? daveEncryptor)
    {
        _encryptionKey = encryptionKey;
        _daveEncryptor = daveEncryptor;
        daveEncryptor?.AssignSsrcToCodec(Ssrc, Dave.Codec.Opus);
    }

    public void SetDaveHandler(DaveProtocolHandler? handler, IVoiceGatewayClient? gateway)
    {
        _daveHandler = handler;
        _gateway = gateway;
    }

    /// <inheritdoc/>
    public void OnSynchronizerTick()
    {
        if (Interlocked.Exchange(ref _taskState, TaskCompletedState) != TaskPendingState)
        {
            return;
        }

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
        if (Volatile.Read(ref _isDisposed) != 0)
        {
            Throw.ObjectDisposedException(GetType().Name);
        }

        if (opus.Length == 0)
        {
            return;
        }

        if (Interlocked.Exchange(ref _isSending, 1) != 0)
        {
            Throw.InvalidOperationException($"Invalid concurrent {nameof(SendAsync)} call.");
        }

        try
        {
            _mrvtsc.Reset();

            if (Interlocked.Exchange(ref _taskState, TaskPendingState) != TaskCompletedState)
            {
                Throw.InvalidOperationException($"Invalid {nameof(SendAsync)} state.");
            }

            await new ValueTask(this, _mrvtsc.Version).ConfigureAwait(false);

            using (var packet = CreateVoicePacket(opus.Span))
            {
                await UdpClient.SendAsync(packet, cancellationToken).ConfigureAwait(false);
            }

            _sequence++;
            _timestamp += VoiceConstants.AudioSize;
        }
        finally
        {
            Volatile.Write(ref _isSending, 0);
        }
    }

    /// <inheritdoc/>
    public async ValueTask<VoiceReceivePacket?> ReceiveAsync(CancellationToken cancellationToken = default)
    {
        if (Volatile.Read(ref _isDisposed) != 0)
        {
            Throw.ObjectDisposedException(GetType().Name);
        }

        var bytesRead= await UdpClient.ReceiveAsync(_receiveBuffer, cancellationToken).ConfigureAwait(false);
        if (bytesRead == 0)
        {
            return null;
        }

        var packet = _receiveBuffer.AsSpan(0, bytesRead);
        if (packet.Length <= 8)
        {
            return null;
        }

        // Discovery response packet.
        if (packet.Length == 74 && packet[1] == 2)
        {
            return null;
        }

        if ((packet[0] >> 6) != 2)
        {
            return null;
        }

        // RTCP packet types use the full second byte (RFC 5761).
        // Do not mask with the RTP marker bit here, otherwise RTCP packets can be misclassified as RTP.
        var packetType = packet[1];
        if (packetType is >= 192 and <= 223)
        {
            return null;
        }

        var csrcCount = packet[0] & 0x0F;
        var rtpHeaderLength = VoiceConstants.RtpHeaderSize + csrcCount * 4;
        if (packet.Length <= rtpHeaderLength)
        {
            return null;
        }

        var sequence = BinaryPrimitives.ReadUInt16BigEndian(packet[2..]);
        var timestamp = BinaryPrimitives.ReadUInt32BigEndian(packet[4..]);
        var ssrc = BinaryPrimitives.ReadUInt32BigEndian(packet[8..]);

        var hasRtpExtension = (packet[0] & 0x10) != 0;
        var extensionPreambleLength = hasRtpExtension ? 4 : 0;
        var encryptedAudioOffset = rtpHeaderLength + extensionPreambleLength;
        if (packet.Length <= encryptedAudioOffset)
        {
            return null;
        }

        var encryptedAudio = packet[encryptedAudioOffset..];
        var decryptedLength = Encryption.GetDecryptedLength(encryptedAudio.Length);
        if (decryptedLength <= 0)
        {
            return null;
        }

        byte[]? rentedTransportBuffer = null;
        try
        {
            var transportAudio = decryptedLength <= 4096
                ? stackalloc byte[decryptedLength]
                : rentedTransportBuffer = ArrayPool<byte>.Shared.Rent(decryptedLength);

            Encryption.Decrypt(packet[..encryptedAudioOffset], transportAudio[..decryptedLength], encryptedAudio, _encryptionKey);

            // Strip RTP padding if present (RFC 3550 §5.1).
            // Padding bytes are at the end of the decrypted payload; the last byte indicates the count.
            // Padding must be stripped BEFORE extension stripping and DAVE decrypt; otherwise
            // the extra bytes at the end corrupt the DAVE supplemental structure (magic marker [0xFA, 0xFA]).
            var decryptedPayload = transportAudio[..decryptedLength];
            var hasPadding = (packet[0] & 0x20) != 0;
            if (hasPadding && decryptedPayload.Length > 0)
            {
                var paddingLength = decryptedPayload[^1];
                if (paddingLength > 0 && paddingLength <= decryptedPayload.Length)
                {
                    decryptedPayload = decryptedPayload[..^paddingLength];
                }
            }

            var media = StripRtpHeaderExtensions(packet, rtpHeaderLength, decryptedPayload);
            if (media.Length == 0)
            {
                return null;
            }

            if (_daveHandler != null)
            {
                // DAVE requires per-user decryption. Map SSRC -> UserId -> DaveDecryptor.
                // If the mapping or decryptor isn't available yet (Speaking/ClientConnect not
                // yet processed), drop the packet. AudioReceiver fills the resulting gap with
                // silence, and the lost packets are typically warm-up silence frames anyway.
                var mappedUserId = _gateway != null && _gateway.TryGetUserId(ssrc, out var uid) ? uid : (Snowflake?) null;
                if (!mappedUserId.HasValue)
                {
                    return null;
                }

                using var lease = _daveHandler.GetDecryptor(mappedUserId.Value);
                if (lease.Decryptor == null)
                {
                    return null;
                }

                if (!TryDecryptDavePacket(lease.Decryptor, media, hasRtpExtension, ssrc, out var rentedOpusArray, out var opusLength))
                {
                    return null;
                }

                return rentedOpusArray != null
                    ? RentReceivePacket(sequence, timestamp, ssrc, mappedUserId, rentedOpusArray, opusLength)
                    : RentReceivePacket(sequence, timestamp, ssrc, mappedUserId, VoiceConstants.SilencePacket);
            }

            var rentedMedia = ArrayPool<byte>.Shared.Rent(media.Length);
            media.CopyTo(rentedMedia);
            return RentReceivePacket(sequence, timestamp, ssrc, userId: null, rentedMedia, media.Length);
        }
        catch (VoiceEncryptionException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new VoiceEncryptionException("Failed to decrypt a received voice packet.", ex);
        }
        finally
        {
            if (rentedTransportBuffer != null)
            {
                ArrayPool<byte>.Shared.Return(rentedTransportBuffer);
            }
        }
    }

    private bool TryDecryptDavePacket(DaveDecryptor daveDecryptor, ReadOnlySpan<byte> media, bool hasRtpExtension, uint ssrc, out byte[]? rentedOpusArray, out int opusLength)
    {
        rentedOpusArray = null;
        opusLength = 0;

        var maxDavePlaintextByteSize = (int) daveDecryptor.GetMaxPlaintextByteSize(Dave.MediaType.Audio, (nuint) media.Length);
        if (maxDavePlaintextByteSize <= 0)
        {
            return false;
        }

        // Rent the output buffer directly - decrypt into it, no intermediate copy.
        rentedOpusArray = ArrayPool<byte>.Shared.Rent(maxDavePlaintextByteSize);

        var result = daveDecryptor.Decrypt(Dave.MediaType.Audio, media, rentedOpusArray.AsSpan(0, maxDavePlaintextByteSize), out var bytesWritten);

        if (result != Dave.DecryptorResultCode.Success)
        {
            _daveHandler?.OnDecryptResult(false);
            ArrayPool<byte>.Shared.Return(rentedOpusArray);
            rentedOpusArray = null;

            // Log at Debug - but only when not transitioning. During transitions, decrypt failures
            // are expected and self-recover once the new key ratchet is committed.
            if (_logger.IsEnabled(LogLevel.Debug) && !(_daveHandler?.IsTransitioning ?? false))
            {
                _logger.LogDebug(
                    "DAVE decryption failed for SSRC {Ssrc}, result={Result}, MediaLen={MediaLen}.",
                    ssrc, result, media.Length);
            }

            return false;
        }

        _daveHandler?.OnDecryptResult(true);

        if (bytesWritten == 0)
        {
            ArrayPool<byte>.Shared.Return(rentedOpusArray);
            rentedOpusArray = null;
        }
        else
        {
            opusLength = (int) bytesWritten;
        }

        return true;
    }

    private VoiceReceivePacket RentReceivePacket(ushort sequence, uint timestamp, uint ssrc, Snowflake? userId, ReadOnlyMemory<byte> opus)
    {
        var state = _receivePacketPool.Rent();
        return state.Initialize(sequence, timestamp, ssrc, userId, opus, rentedArray: null);
    }

    private VoiceReceivePacket RentReceivePacket(ushort sequence, uint timestamp, uint ssrc, Snowflake? userId, byte[] rentedArray, int length)
    {
        var state = _receivePacketPool.Rent();
        return state.Initialize(sequence, timestamp, ssrc, userId, new ReadOnlyMemory<byte>(rentedArray, 0, length), rentedArray);
    }

    private static ReadOnlySpan<byte> StripRtpHeaderExtensions(ReadOnlySpan<byte> packet, int rtpHeaderLength, ReadOnlySpan<byte> payload)
    {
        if ((packet[0] & 0x10) == 0)
        {
            return payload;
        }

        if (packet.Length < rtpHeaderLength + 4)
        {
            return ReadOnlySpan<byte>.Empty;
        }

        // Discord encrypts the RTP header extension contents but leaves the 4-byte extension preamble
        // in cleartext and authenticates it as part of the RTP header.
        var extensionLengthWords = BinaryPrimitives.ReadUInt16BigEndian(packet[(rtpHeaderLength + 2)..]);
        var extensionLength = extensionLengthWords * 4;

        return extensionLength >= payload.Length
            ? ReadOnlySpan<byte>.Empty
            : payload[extensionLength..];
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
        {
            addressSpan = addressSpan[..nullIndex];
        }

        RemoteHostName = Encoding.UTF8.GetString(addressSpan);
        RemotePort = BinaryPrimitives.ReadUInt16BigEndian(packet[72..]);
    }

    public void Dispose()
    {
        if (Interlocked.Exchange(ref _isDisposed, 1) != 0)
        {
            return;
        }

        if (Interlocked.Exchange(ref _taskState, TaskPendingState) == TaskPendingState)
        {
            _mrvtsc.SetException(new ObjectDisposedException(GetType().Name));
        }

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

    private sealed class ReceivePacketStatePoolPolicy : PooledObjectPolicy<VoiceReceivePacket.PacketState>
    {
        private ObjectPool<VoiceReceivePacket.PacketState>? _pool;

        public void Bind(ObjectPool<VoiceReceivePacket.PacketState> pool)
        {
            _pool = pool;
        }

        public override VoiceReceivePacket.PacketState Create()
        {
            return new VoiceReceivePacket.PacketState(_pool!);
        }

        public override bool OnReturn(VoiceReceivePacket.PacketState obj)
        {
            obj.ResetForPooling();
            return true;
        }
    }
}
