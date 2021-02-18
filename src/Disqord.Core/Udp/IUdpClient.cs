using System;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Udp
{
    public interface IUdpClient : IDisposable
    {
        void Connect(string hostname, int port);

        void Close();

        Task SendAsync(ArraySegment<byte> data, CancellationToken cancellationToken = default);

        ValueTask<ReadOnlyMemory<byte>> ReceiveAsync(CancellationToken cancellationToken = default);
    }
}
