using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway;
using Disqord.Voice;
using Microsoft.Extensions.Logging;
using Qommon;
using Qommon.Collections.ReadOnly;
using Qommon.Collections.ThreadSafe;

namespace Disqord.Extensions.Voice;

public class VoiceExtension : DiscordClientExtension
{
    public IReadOnlyDictionary<Snowflake, IVoiceConnection> Connections => _connections.ReadOnly();

    private readonly IVoiceConnectionFactory _connectionFactory;

    private readonly IThreadSafeDictionary<Snowflake, IVoiceConnection> _connections;

    public VoiceExtension(
        ILogger<VoiceExtension> logger,
        IVoiceConnectionFactory connectionFactory)
        : base(logger)
    {
        _connectionFactory = connectionFactory;

        _connections = ThreadSafeDictionary.Monitor.Create<Snowflake, IVoiceConnection>();
    }

    /// <inheritdoc/>
    protected override ValueTask InitializeAsync(CancellationToken cancellationToken)
    {
        Client.VoiceStateUpdated += VoiceStateUpdatedAsync;
        Client.VoiceServerUpdated += VoiceServerUpdatedAsync;

        return default;
    }

    private Task VoiceServerUpdatedAsync(object? sender, VoiceServerUpdatedEventArgs e)
    {
        if (_connections.TryGetValue(e.GuildId, out var connection))
        {
            connection.OnVoiceServerUpdate(e.Token, e.Endpoint);
        }

        return Task.CompletedTask;
    }

    private Task VoiceStateUpdatedAsync(object? sender, VoiceStateUpdatedEventArgs e)
    {
        if (Client.CurrentUser.Id != e.NewVoiceState.MemberId)
            return Task.CompletedTask;

        if (_connections.TryGetValue(e.GuildId, out var connection))
        {
            var voiceState = e.NewVoiceState;
            connection.OnVoiceStateUpdate(voiceState.ChannelId, voiceState.SessionId);
        }

        return Task.CompletedTask;
    }

    public async ValueTask<IVoiceConnection> ConnectAsync(Snowflake guildId, Snowflake channelId, CancellationToken cancellationToken = default)
    {
        var connection = _connectionFactory.Create(guildId, channelId, Client.CurrentUser.Id,
            (guildId, channelId, cancellationToken) =>
            {
                var shard = Client.ApiClient.GetShard(guildId);
                if (shard == null)
                    Throw.InvalidOperationException("The guild ID is not handled by any of the shards of the client");

                Logger.LogDebug("Setting voice state for guild ID: {GuildId} to channel ID: {ChannelId}", guildId, channelId);
                return new(shard.SetVoiceStateAsync(guildId, channelId, false, true, cancellationToken));
            });

        _connections[guildId] = connection;

        var readyTask = connection.WaitUntilReadyAsync(cancellationToken);
        _ = connection.RunAsync(Client.StoppingToken);

        await readyTask.ConfigureAwait(false);
        return connection;
    }
}
