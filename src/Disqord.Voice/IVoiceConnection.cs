using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Voice.Api;

namespace Disqord.Voice;

public delegate ValueTask SetVoiceStateDelegate(Snowflake guildId, Snowflake? channelId, CancellationToken cancellationToken);

public interface IVoiceConnection : IAsyncDisposable
{
    IVoiceGatewayClient Gateway { get; }

    IVoiceUdpClient Udp { get; }

    IVoiceSynchronizer Synchronizer { get; }

    Snowflake GuildId { get; }

    Snowflake ChannelId { get; }

    Snowflake CurrentMemberId { get; }

    void OnVoiceStateUpdate(Snowflake? channelId, string sessionId);

    void OnVoiceServerUpdate(string token, string? endpoint);

    ValueTask SetChannelIdAsync(Snowflake channelId, CancellationToken cancellationToken = default);

    ValueTask SetSpeakingFlagsAsync(SpeakingFlags flags, CancellationToken cancellationToken = default);

    ValueTask SendPacketAsync(ReadOnlyMemory<byte> opus, CancellationToken cancellationToken = default);

    Task RunAsync(CancellationToken stoppingToken);

    Task WaitUntilReadyAsync(CancellationToken cancellationToken = default);
}
