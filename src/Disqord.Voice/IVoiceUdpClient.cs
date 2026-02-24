using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Udp;

namespace Disqord.Voice;

public interface IVoiceUdpClient : IDisposable
{
    IUdpClient UdpClient { get; }

    uint Ssrc { get; }

    ReadOnlyMemory<byte> EncryptionKey { get; }

    string HostName { get; }

    int Port { get; }

    IVoiceEncryption Encryption { get; }

    string? RemoteHostName { get; }

    ushort? RemotePort { get; }

    ushort Sequence { get; }

    uint Timestamp { get; }

    void Initialize(byte[] encryptionKey, DaveEncryptor? daveEncryptor);

    void OnSynchronizerTick();

    ValueTask ConnectAsync(CancellationToken cancellationToken = default);

    ValueTask CloseAsync(CancellationToken cancellationToken = default);

    ValueTask SendAsync(ReadOnlyMemory<byte> opus, CancellationToken cancellationToken = default);
}
