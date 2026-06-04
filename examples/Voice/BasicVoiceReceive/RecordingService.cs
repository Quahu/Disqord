using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disqord;
using Disqord.Bot.Hosting;
using Disqord.Extensions.Voice;
using Disqord.Voice;
using Microsoft.Extensions.Logging;

namespace BasicVoiceReceive;

public class RecordingService : DiscordBotService
{
    private readonly Dictionary<Snowflake, RecordingSession> _sessions = [];
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);

        await _semaphore.WaitAsync(cancellationToken);
        RecordingSession[] sessions;
        try
        {
            sessions = _sessions.Values.ToArray();
            _sessions.Clear();
        }
        finally
        {
            _semaphore.Release();
        }

        var voiceExtension = Bot.GetRequiredExtension<VoiceExtension>();
        foreach (var session in sessions)
        {
            try
            {
                await session.DisposeAsync();
                await voiceExtension.DisconnectAsync(session.GuildId);
            }
            catch (Exception ex)
            {
                Bot.Logger.LogError(ex, "An exception occurred while stopping an active recording session.");
            }
        }
    }

    public async Task<bool> StartRecordingAsync(Snowflake guildId, Snowflake voiceChannelId, Snowflake? targetUserId)
    {
        await _semaphore.WaitAsync();
        try
        {
            if (_sessions.ContainsKey(guildId))
            {
                return false;
            }

            var voiceExtension = Bot.GetRequiredExtension<VoiceExtension>();
            RecordingSession? session = null;
            try
            {
                var connection = await voiceExtension.ConnectAsync(guildId, voiceChannelId, new VoiceConnectOptions
                {
                    SelfDeafen = false
                });

                session = new RecordingSession(guildId, connection, targetUserId);
                _sessions[guildId] = session;
                session.Start();

                return true;
            }
            catch
            {
                _sessions.Remove(guildId);

                if (session != null)
                {
                    await session.DisposeAsync();
                }

                await voiceExtension.DisconnectAsync(guildId);
                throw;
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<IReadOnlyList<(Snowflake UserId, Stream Stream)>?> StopRecordingAsync(Snowflake guildId)
    {
        RecordingSession? session;
        await _semaphore.WaitAsync();
        try
        {
            if (!_sessions.Remove(guildId, out session))
            {
                return null;
            }
        }
        finally
        {
            _semaphore.Release();
        }

        try
        {
            return await session.StopAndCollectAsync();
        }
        finally
        {
            await session.DisposeAsync();

            var voiceExtension = Bot.GetRequiredExtension<VoiceExtension>();
            await voiceExtension.DisconnectAsync(guildId);
        }
    }

    private sealed class RecordingSession(Snowflake guildId, IVoiceConnection connection, Snowflake? targetUserId) : IAsyncDisposable
    {
        public Snowflake GuildId { get; } = guildId;

        private static readonly AudioReceiveSubscriptionOptions RecordingOptions = new()
        {
            EndBehaviorType = AudioReceiveEndBehaviorType.Manual,
            MaxBufferedDuration = TimeSpan.Zero
        };

        private readonly AudioReceiver _receiver = new(connection);
        private readonly CancellationTokenSource _cts = new();
        private readonly Dictionary<Snowflake, UserRecording> _recordings = [];
        private readonly Dictionary<Snowflake, Task> _activeTasks = [];
        private Task? _listenTask;

        public void Start()
        {
            _listenTask = ListenLoopAsync(_cts.Token);
        }

        private async Task ListenLoopAsync(CancellationToken cancellationToken)
        {
            try
            {
                await foreach (var subscription in _receiver.ListenAsync(
                    userId => targetUserId == null || userId == targetUserId ? RecordingOptions : null,
                    _cts.Token))
                {
                    var userId = subscription.UserId;

                    // Await any previous write task for this user (handles voice reconnect).
                    if (_activeTasks.TryGetValue(userId, out var previousTask))
                    {
                        await previousTask;
                    }

                    // Reuse existing recording for this user so reconnects produce a single file.
                    if (!_recordings.TryGetValue(userId, out var recording))
                    {
                        recording = new UserRecording(userId);
                        _recordings[userId] = recording;
                    }

                    _activeTasks[userId] = Task.Run(() => recording.RunAsync(subscription), cancellationToken);
                }
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            { }
        }

        public async Task<IReadOnlyList<(Snowflake UserId, Stream Stream)>> StopAndCollectAsync()
        {
            _cts.Cancel();

            if (_listenTask != null)
            {
                await _listenTask;
            }

            await _receiver.DisposeAsync();
            await Task.WhenAll(_activeTasks.Values);

            var results = new List<(Snowflake UserId, Stream Stream)>();
            foreach (var recording in _recordings.Values)
            {
                var stream = recording.Collect();
                if (stream != null)
                {
                    results.Add((recording.UserId, stream));
                }
            }

            return results;
        }

        public async ValueTask DisposeAsync()
        {
            _cts.Cancel();

            if (_listenTask != null)
            {
                try
                {
                    await _listenTask;
                }
                catch (OperationCanceledException)
                { }
            }

            await _receiver.DisposeAsync();
            await Task.WhenAll(_activeTasks.Values);

            foreach (var recording in _recordings.Values)
            {
                recording.Dispose();
            }

            _recordings.Clear();
            _activeTasks.Clear();
            _cts.Dispose();
        }
    }

    private sealed class UserRecording : IDisposable
    {
        public Snowflake UserId { get; }

        private readonly MemoryStream _stream = new();
        private readonly OggOpusWriter _writer;
        private long _disconnectedAt;
        private bool _hasDisconnectedAt;

        public UserRecording(Snowflake userId)
        {
            UserId = userId;
            _writer = new OggOpusWriter(_stream);
        }

        public async Task RunAsync(AudioReceiverSubscription subscription)
        {
            if (_hasDisconnectedAt)
            {
                var reconnectGap = Stopwatch.GetElapsedTime(_disconnectedAt, Stopwatch.GetTimestamp());
                _hasDisconnectedAt = false;
                _writer.WriteSilence(reconnectGap);
            }

            _writer.ResetRtpTimestampBase();

            try
            {
                await foreach (var packet in subscription)
                {
                    try
                    {
                        _writer.WritePacket(packet.Opus.Span, packet.Timestamp);
                    }
                    finally
                    {
                        packet.Dispose();
                    }
                }
            }
            finally
            {
                _disconnectedAt = Stopwatch.GetTimestamp();
                _hasDisconnectedAt = true;
            }
        }

        public Stream? Collect()
        {
            _writer.Complete();
            _stream.Position = 0;
            return _writer.PacketCount > 0 ? _stream : null;
        }

        public void Dispose()
        {
            _writer.Dispose();
        }
    }
}
