using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Utilities.Threading;
using Qommon;

namespace Disqord.Udp.Default;

public sealed class DefaultUdpClient : IUdpClient
{
    private Cts? _cts;
    private Socket? _socket;

    public DefaultUdpClient()
    { }

    public async ValueTask ConnectAsync(string hostName, int port, CancellationToken cancellationToken = default)
    {
        _cts = new Cts();
        var hostAddresses = await Dns.GetHostAddressesAsync(hostName, cancellationToken).ConfigureAwait(false);
        var hostAddress = Array.Find(hostAddresses, x => x.AddressFamily == AddressFamily.InterNetwork);
        if (hostAddress == null)
            Throw.InvalidOperationException($"Could not resolve the UDP client's host '{hostName}'.");

        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        await _socket.ConnectAsync(hostAddress, port, cancellationToken).ConfigureAwait(false);
    }

    public ValueTask CloseAsync(CancellationToken cancellationToken = default)
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _socket?.Close();
        return default;
    }

    public ValueTask<int> SendAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
    {
        return _socket!.SendAsync(buffer, SocketFlags.None, cancellationToken);
    }

    public ValueTask<int> ReceiveAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        return _socket!.ReceiveAsync(buffer, SocketFlags.None, cancellationToken);
    }

    public void Dispose()
    {
        _cts?.Dispose();
        _socket?.Dispose();
    }
}