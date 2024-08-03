using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway;
using Disqord.Utilities.Threading;
using Disqord.Voice;
using Microsoft.Extensions.Logging;
using Qommon;
using Qommon.Collections.ThreadSafe;

namespace Disqord.Extensions.Voice;

// TODO: keyed semaphore
public class VoiceExtension : DiscordClientExtension
{
    private readonly IVoiceConnectionFactory _connectionFactory;

    private readonly IThreadSafeDictionary<Snowflake, VoiceConnectionInfo> _connections;

    public VoiceExtension(
        ILogger<VoiceExtension> logger,
        IVoiceConnectionFactory connectionFactory)
        : base(logger)
    {
        _connectionFactory = connectionFactory;

        _connections = ThreadSafeDictionary.Monitor.Create<Snowflake, VoiceConnectionInfo>();
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
        GetConnection(e.GuildId)?.OnVoiceServerUpdate(e.Token, e.Endpoint);
        return Task.CompletedTask;
    }

    private Task VoiceStateUpdatedAsync(object? sender, VoiceStateUpdatedEventArgs e)
    {
        if (Client.CurrentUser.Id != e.NewVoiceState.MemberId)
        {
            return Task.CompletedTask;
        }

        var voiceState = e.NewVoiceState;
        GetConnection(e.GuildId)?.OnVoiceStateUpdate(voiceState.ChannelId, voiceState.SessionId);
        return Task.CompletedTask;
    }

    /// <summary>
    ///     Gets the voice connection for the guild with the given ID.
    /// </summary>
    /// <param name="guildId"> The ID of the guild. </param>
    /// <returns>
    ///     The voice connection or <see langword="null"/> if the connection does not exist.
    /// </returns>
    public IVoiceConnection? GetConnection(Snowflake guildId)
    {
        return _connections.TryGetValue(guildId, out var connectionInfo)
            ? connectionInfo.Connection
            : null;
    }

    /// <summary>
    ///     Gets all maintained voice connections.
    /// </summary>
    /// <returns>
    ///     A dictionary of all connections keyed by the IDs of the guilds.
    /// </returns>
    public IReadOnlyDictionary<Snowflake, IVoiceConnection> GetConnections()
    {
        return _connections.ToDictionary(static kvp => kvp.Key, static kvp => kvp.Value.Connection);
    }

    /// <summary>
    ///     Connects to the channel with the given ID.
    /// </summary>
    /// <remarks>
    ///     To disconnect the voice connection, use <see cref="DisconnectAsync"/>.
    /// </remarks>
    /// <param name="guildId"> The ID of the guild the channel is in. </param>
    /// <param name="channelId"> The ID of the channel. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. This is only used for the initial connection. </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}"/> with the result being the created voice connection.
    /// </returns>
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

        var connectionInfo = new VoiceConnectionInfo(connection, Cts.Linked(Client.StoppingToken));
        _connections[guildId] = connectionInfo;
        try
        {
            var readyTask = connection.WaitUntilReadyAsync(cancellationToken);
            _ = connection.RunAsync(connectionInfo.Cts.Token);

            await readyTask.ConfigureAwait(false);
        }
        catch
        {
            _connections.Remove(guildId);
            await connectionInfo.DisposeAsync();
            throw;
        }

        return connection;
    }

    /// <summary>
    ///     Disconnects the voice connection, if one exists, for the guild with the given ID.
    /// </summary>
    /// <remarks>
    ///     This will render the relevant voice connection unusable.
    ///     Use <see cref="ConnectAsync"/> to obtain a new connection afterward.
    /// </remarks>
    /// <param name="guildId"> The ID of the guild. </param>
    public ValueTask DisconnectAsync(Snowflake guildId)
    {
        if (!_connections.TryRemove(guildId, out var connectionInfo))
        {
            return default;
        }

        return connectionInfo.DisposeAsync();
    }

    private readonly struct VoiceConnectionInfo : IAsyncDisposable
    {
        public IVoiceConnection Connection { get; }

        public Cts Cts { get; }

        public VoiceConnectionInfo(IVoiceConnection connection, Cts cts)
        {
            Connection = connection;
            Cts = cts;
        }

        public async ValueTask DisposeAsync()
        {
            Cts.Cancel();
            Cts.Dispose();
            await Connection.DisposeAsync();
        }
    }
}
