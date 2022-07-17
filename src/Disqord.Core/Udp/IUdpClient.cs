using System;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Udp;

public interface IUdpClient : IDisposable
{
    ValueTask ConnectAsync(string hostName, int port, CancellationToken cancellationToken = default);

    ValueTask CloseAsync(CancellationToken cancellationToken = default);

    ValueTask<int> SendAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default);

    ValueTask<int> ReceiveAsync(Memory<byte> buffer, CancellationToken cancellationToken = default);
}