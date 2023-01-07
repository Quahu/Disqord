using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Utilities.Threading;
using Qommon;

namespace Disqord.Udp.Default;

public class DefaultUdpClient : IUdpClient
{
    private Cts? _cts;
    private readonly Socket _socket;

    public DefaultUdpClient()
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    }

    protected DefaultUdpClient(Socket socket)
    {
        _socket = socket;
    }

    public async ValueTask ConnectAsync(string hostName, int port, CancellationToken cancellationToken = default)
    {
        _cts = new Cts();
        var hostAddresses = await Dns.GetHostAddressesAsync(hostName, cancellationToken).ConfigureAwait(false);
        var hostAddress = Array.Find(hostAddresses, address => address.AddressFamily == AddressFamily.InterNetwork);
        if (hostAddress == null)
            Throw.InvalidOperationException($"Could not resolve the UDP client's host '{hostName}'.");

        await _socket.ConnectAsync(hostAddress, port, cancellationToken).ConfigureAwait(false);
    }

    public ValueTask CloseAsync(CancellationToken cancellationToken = default)
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _socket.Close();
        return default;
    }

    public ValueTask<int> SendAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
    {
        return _socket.SendAsync(buffer, SocketFlags.None, cancellationToken);
    }

    public ValueTask<int> ReceiveAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        return _socket.ReceiveAsync(buffer, SocketFlags.None, cancellationToken);
    }

    public void Dispose()
    {
        _cts?.Dispose();
        _socket.Dispose();
    }
}
