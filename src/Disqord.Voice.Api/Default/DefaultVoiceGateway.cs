using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Serialization.Json;
using Disqord.Voice.Api.Models;
using Disqord.WebSocket;
using Disqord.WebSocket.Default.Discord;
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

    private DiscordWebSocket _ws = null!;
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

        _ws = new DiscordWebSocket(Logger, WebSocketClientFactory, false);
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

        return _ws.SendAsync(json, cancellationToken);
    }

    public async ValueTask<VoiceGatewayPayloadJsonModel> ReceiveAsync(CancellationToken cancellationToken = default)
    {
        var jsonStream = await _ws.ReceiveAsync(cancellationToken).ConfigureAwait(false);
        if (LogsPayloads)
        {
            var stream = new MemoryStream();
            await jsonStream.CopyToAsync(stream, cancellationToken).ConfigureAwait(false);
            Logger.LogTrace("Voice received payload: {0}", Encoding.UTF8.GetString(stream.GetBuffer()));
            stream.Position = 0;
            jsonStream = stream;
        }

        return Serializer.Deserialize<VoiceGatewayPayloadJsonModel>(jsonStream)!;
    }

    public ValueTask DisposeAsync()
    {
        return _ws.DisposeAsync();
    }
}
