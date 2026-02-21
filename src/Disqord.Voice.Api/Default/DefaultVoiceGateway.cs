using System;
using System.Buffers.Binary;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Serialization.Json;
using Disqord.Voice.Api.Models;
using Disqord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Qommon.Binding;

namespace Disqord.Voice.Api.Default;

public class DefaultVoiceGateway : IVoiceGateway
{
    public int Version { get; }

    public bool LogsPayloads { get; }

    public IVoiceGatewayClient Client => _binder.Value;

    public ILogger Logger => Client.Logger;

    public IJsonSerializer Serializer { get; }

    public IWebSocketClientFactory WebSocketClientFactory { get; }

    private VoiceWebSocket _ws = null!;
    private readonly Binder<IVoiceGatewayClient> _binder;

    public DefaultVoiceGateway(
        IOptions<DefaultVoiceGatewayConfiguration> options,
        IJsonSerializer serializer,
        IWebSocketClientFactory webSocketClientFactory)
    {
        var configuration = options.Value;
        Version = configuration.Version;
        LogsPayloads = configuration.LogsPayloads;
        Serializer = serializer;
        WebSocketClientFactory = webSocketClientFactory;

        _binder = new(this);
    }

    public void Bind(IVoiceGatewayClient value)
    {
        _binder.Bind(value);

        _ws = new VoiceWebSocket(Logger, WebSocketClientFactory);
    }

    public ValueTask ConnectAsync(Uri uri, CancellationToken cancellationToken = default)
    {
        uri = new Uri($"wss://{uri}?v={Version}");
        Logger.LogDebug("Voice connecting to {0}", uri);
        return _ws.ConnectAsync(uri, cancellationToken);
    }

    public ValueTask CloseAsync(int closeStatus, string? closeMessage = null, CancellationToken cancellationToken = default)
    {
        Logger.LogDebug("Voice closing with status {0}: {1}", closeStatus, closeMessage);
        return _ws.CloseAsync(closeStatus, closeMessage, cancellationToken);
    }

    public ValueTask SendAsync(VoiceGatewayPayloadJsonModel payload, CancellationToken cancellationToken = default)
    {
        var json = Serializer.Serialize(payload);
        if (LogsPayloads)
            Logger.LogTrace("Voice sending payload: {0}", Encoding.UTF8.GetString(json.Span));

        if (json.Length > 4096)
            throw new ArgumentException("Voice cannot send payloads longer than 4096 bytes.", nameof(payload));

        return _ws.SendAsync(json, WebSocketMessageType.Text, cancellationToken);
    }

    public ValueTask SendBinaryAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default)
    {
        if (LogsPayloads && data.Length > 0)
            Logger.LogTrace("Voice sending binary payload: op {0}, {1} bytes", data.Span[0], data.Length);

        return _ws.SendAsync(data, WebSocketMessageType.Binary, cancellationToken);
    }

    public async ValueTask<VoiceGatewayMessage> ReceiveAsync(CancellationToken cancellationToken = default)
    {
        var (stream, isBinary) = await _ws.ReceiveAsync(cancellationToken).ConfigureAwait(false);
        if (isBinary)
        {
            return ParseBinaryMessage(stream);
        }

        if (LogsPayloads)
        {
            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream, cancellationToken).ConfigureAwait(false);
            Logger.LogTrace("Voice received payload: {0}", Encoding.UTF8.GetString(memoryStream.GetBuffer()));
            memoryStream.Position = 0;
            stream = memoryStream;
        }

        var payload = Serializer.Deserialize<VoiceGatewayPayloadJsonModel>(stream)!;
        return VoiceGatewayMessage.FromJson(payload);
    }

    private static VoiceGatewayMessage ParseBinaryMessage(MemoryStream stream)
    {
        // Binary format (server → client): [2-byte big-endian seq] [1-byte opcode] [payload]
        stream.TryGetBuffer(out var buffer);
        var span = buffer.AsSpan();

        if (span.Length < 3)
        {
            throw new InvalidDataException("Binary voice gateway message is too short.");
        }

        var sequenceNumber = (int) BinaryPrimitives.ReadUInt16BigEndian(span);
        var op = (VoiceGatewayPayloadOperation) span[2];
        var payload = buffer.AsMemory(3);

        return VoiceGatewayMessage.FromBinary(op, sequenceNumber, payload);
    }

    public ValueTask DisposeAsync()
    {
        return _ws.DisposeAsync();
    }
}
