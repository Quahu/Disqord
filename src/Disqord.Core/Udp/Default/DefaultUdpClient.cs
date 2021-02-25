using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Disqord.Utilities.Threading;

namespace Disqord.Udp.Default
{
// Unused - revisit if required
#nullable disable
    public sealed class DefaultUdpClient : IUdpClient
    {
        private Cts _cts;
        private Channel<ReadOnlyMemory<byte>> _channel;
        private readonly UdpClient _udp;

        public DefaultUdpClient()
        {
            _udp = new UdpClient();
        }

        public void Connect(string hostname, int port)
        {
            _udp.Connect(hostname, port);
            _cts = new Cts();
            _channel = Channel.CreateUnbounded<ReadOnlyMemory<byte>>();
            _ = Task.Run(RunReceiveAsync);
        }

        public void Close()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _channel?.Writer.Complete();
            try
            {
                _udp.Close();
            }
            catch { }
        }

        public Task SendAsync(ArraySegment<byte> bytes, CancellationToken cancellationToken = default)
            => _udp.SendAsync(bytes.Array, bytes.Count);

        public ValueTask<ReadOnlyMemory<byte>> ReceiveAsync(CancellationToken cancellationToken = default)
            => _channel.Reader.ReadAsync(cancellationToken);

        private async Task RunReceiveAsync()
        {
            var token = _cts.Token;
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var result = await _udp.ReceiveAsync().ConfigureAwait(false);
                    await _channel.Writer.WriteAsync(result.Buffer, token).ConfigureAwait(false);
                }
                catch (TaskCanceledException)
                { }
                catch (Exception ex)
                {
                    _channel.Writer.TryComplete(ex);
                }
            }
        }

        public void Dispose()
        {
            _udp.Dispose();
        }
    }
}
