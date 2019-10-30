using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Events;
using Disqord.Logging;
using Disqord.Models;
using Disqord.Models.Dispatches;
using Disqord.Rest;
using Disqord.Serialization;
using Disqord.WebSocket;
using Qommon.Collections;

namespace Disqord
{
    public partial class DiscordClient : DiscordClientBase
    {
        internal readonly ConcurrentDictionary<Snowflake, CircularBuffer<CachedUserMessage>> CachedMessages = Extensions.CreateConcurrentDictionary<Snowflake, CircularBuffer<CachedUserMessage>>(0);

        /// <summary>
        ///     Gets the latency between heartbeats.
        /// </summary>
        public TimeSpan? Latency => _lastHeartbeatAck - _lastHeartbeatSent;

        internal UserStatus Status;
        internal ActivityModel Activity;

        private readonly bool _guildSubscriptions;
        internal readonly WebSocketClient _ws;
        private string _gatewayUrl;

        private int? _lastSequenceNumber;
        private string _sessionId;
        private string[] _trace;
        private volatile bool _resuming;
        private volatile bool _reconnecting;
        private readonly object _reconnectionLock = new object();
        private readonly SemaphoreSlim _resumeSemaphore = new SemaphoreSlim(1, 1);
        private TaskCompletionSource<bool> _readyTaskCompletionSource;
        private readonly ConcurrentQueue<(PayloadModel, GatewayDispatch)> _readyPayloadQueue = new ConcurrentQueue<(PayloadModel, GatewayDispatch)>();

        private DateTimeOffset? _lastHeartbeatAck;
        private DateTimeOffset? _lastHeartbeatSent;
        private DateTimeOffset? _lastHeartbeatSend;
        private CancellationTokenSource _heartbeatCts;
        private CancellationTokenSource _connectionCts;
        private int _heartbeatInterval;
        private DateTimeOffset? _lastGuildCreate;

        private bool _disposed;
        private bool _manualDisconnection;
        private readonly Random _random = new Random();

        public async Task ConnectAsync()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(DiscordClient));

            if (TokenType == TokenType.Bot)
            {
                var botGatewayResponse = await GetGatewayBotUrlAsync().ConfigureAwait(false);
                if (botGatewayResponse.RemainingSessionAmount == 0)
                    throw new SessionLimitException(botGatewayResponse.ResetAfter);

                Log(LogMessageSeverity.Information,
                    $"Using gateway session {botGatewayResponse.MaxSessionAmount - botGatewayResponse.RemainingSessionAmount}/{botGatewayResponse.MaxSessionAmount}. Limit resets in {botGatewayResponse.ResetAfter}.");
                _gatewayUrl = botGatewayResponse.Url;
            }
            else if (_gatewayUrl == null)
            {
                _gatewayUrl = await RestClient.GetGatewayUrlAsync().ConfigureAwait(false);
            }

            Log(LogMessageSeverity.Information, $"Fetched the gateway url: {_gatewayUrl}.");

            await _resumeSemaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                var connectionCts = _connectionCts ?? (_connectionCts = new CancellationTokenSource());
                using (var cts = new CancellationTokenSource(10000))
                using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, connectionCts.Token))
                {
                    await _ws.ConnectAsync(new Uri(_gatewayUrl + RestRequest.BuildQueryString(new Dictionary<string, object>
                    {
                        ["compress"] = "zlib-stream"
                    })), linkedCts.Token).ConfigureAwait(false);
                }

                if (_sessionId != null)
                {
                    _resuming = true;
                    await SendResumeAsync().ConfigureAwait(false);
                }
            }
            finally
            {
                _resumeSemaphore.Release();
            }
        }

        public async Task DisconnectAsync()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(DiscordClient));

            lock (_reconnectionLock)
            {
                _manualDisconnection = true;
                _sessionId = null;
                try
                {
                    _heartbeatCts?.Cancel();
                }
                catch { }
                _heartbeatCts?.Dispose();
                try
                {
                    _connectionCts?.Cancel();
                }
                catch { }
                _connectionCts?.Dispose();
            }
            await _ws.CloseAsync().ConfigureAwait(false);
        }

        public Task SetPresenceAsync(UserStatus status)
            => InternalSetPresenceAsync(status);

        public Task SetPresenceAsync(LocalActivity activity)
            => InternalSetPresenceAsync(activity: activity);

        public Task SetPresenceAsync(UserStatus status, LocalActivity activity)
            => InternalSetPresenceAsync(status, activity);

        private Task InternalSetPresenceAsync(UserStatus? status = default, in Optional<LocalActivity> activity = default)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(DiscordClient));

            if (!status.HasValue && !activity.HasValue)
                return Task.CompletedTask;

            if (status.HasValue)
                SetStatus(status.Value);

            if (activity.HasValue)
                SetActivity(activity.Value);

            return SendAsync(new PayloadModel
            {
                Op = Opcode.StatusUpdate,
                D = new UpdateStatusModel
                {
                    Status = Status,
                    Game = Activity
                }
            });
        }

        private void SetStatus(UserStatus status)
        {
            switch (status)
            {
                case UserStatus.Invisible:
                case UserStatus.Idle:
                case UserStatus.DoNotDisturb:
                case UserStatus.Online:
                    Status = status;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(status));
            }
        }

        private void SetActivity(LocalActivity activity)
        {
            Activity = activity == null
                ? null
                : new ActivityModel
                {
                    Name = activity.Name,
                    Url = activity.Url,
                    Type = activity.Type
                };
        }

        private async Task WebSocketMessageReceivedAsync(WebSocketMessageReceivedEventArgs e)
        {
            var payload = Serializer.Deserialize<PayloadModel>(e.Stream);
            if (IsBot)
            {
                Log(LogMessageSeverity.Debug, $"Received opcode {payload.Op}.");
            }
            try
            {
                await HandleOpcodeAsync(payload).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log(LogMessageSeverity.Error, $"An exception occurred while handling opcode {payload.Op} ({(byte) payload.Op}).", ex);
                //Console.WriteLine($"Payload data:\n{payload.D}");
            }
        }

        private async Task WebSocketClosedAsync(WebSocketClosedEventArgs e)
        {
            bool manualDisconnection;
            bool reconnecting;

            lock (_reconnectionLock)
            {
                manualDisconnection = _manualDisconnection;
                reconnecting = _reconnecting;
            }

            if (manualDisconnection)
                return;

            Log(LogMessageSeverity.Warning, $"Close: {e.Status} {e.Description}", e.Exception);
            if (reconnecting)
                return;

            GatewayCloseCode gatewayCloseCode = default;
            bool shouldRetry;
            if (e.Status != null)
            {
                gatewayCloseCode = (GatewayCloseCode) e.Status.Value;
                switch (gatewayCloseCode)
                {
                    case GatewayCloseCode.AuthenticationFailed:
                    case GatewayCloseCode.RateLimited:
                    case GatewayCloseCode.InvalidShard:
                    case GatewayCloseCode.ShardingRequired:
                        shouldRetry = false;
                        break;

                    default:
                        shouldRetry = true;
                        break;
                }
            }
            else
            {
                shouldRetry = true;
            }

            if (!shouldRetry && e.Status != null)
            {
                Log(LogMessageSeverity.Error, $"Close {gatewayCloseCode} ({(int) gatewayCloseCode}) is unrecoverable, not retrying.");
                _reconnecting = false;
                await DisconnectAsync().ConfigureAwait(false);
                return;
            }

            try
            {
                _heartbeatCts?.Cancel();
            }
            catch { }
            _heartbeatCts?.Dispose();
            _reconnecting = true;
            while (!_disposed)
            {
                try
                {
                    await ConnectAsync().ConfigureAwait(false);
                    _reconnecting = false;
                    return;
                }
                catch (SessionLimitException ex)
                {
                    Log(LogMessageSeverity.Critical, $"No available sessions. Resets after {ex.ResetsAfter}.", ex);
                    return;
                }
                catch (DiscordHttpException ex) when ((int) ex.HttpStatusCode == 403)
                {
                    Log(LogMessageSeverity.Critical, "");
                    return;
                }
                catch (Exception ex)
                {
                    Log(LogMessageSeverity.Error, $"Failed to reconnect after closure. Retrying in 10 seconds.", ex);
                    await Task.Delay(10000).ConfigureAwait(false);
                }
            }
        }

        private async Task HandleOpcodeAsync(PayloadModel payload)
        {
            switch (payload.Op)
            {
                case Opcode.Dispatch:
                {
                    await HandleDispatchAsync(payload).ConfigureAwait(false);
                    break;
                }

                case Opcode.Heartbeat:
                {
                    Log(LogMessageSeverity.Debug, "Heartbeat requested. Heartbeating...");
                    await SendHeartbeatAsync().ConfigureAwait(false);
                    break;
                }

                case Opcode.Reconnect:
                {
                    Log(LogMessageSeverity.Information, "Reconnect requested, closing...");
                    try
                    {
                        _heartbeatCts?.Cancel();
                    }
                    catch { }
                    _heartbeatCts?.Dispose();
                    await _ws.CloseAsync().ConfigureAwait(false);
                    break;
                }

                case Opcode.InvalidSession:
                {
                    Log(LogMessageSeverity.Warning, "Received invalid session...");
                    if (_resuming)
                    {
                        Log(LogMessageSeverity.Information, "Currently resuming, starting a new session...");
                        await Task.Delay(_random.Next(1000, 5001)).ConfigureAwait(false);
                        await SendIdentifyAsync().ConfigureAwait(false);
                    }
                    else
                    {
                        if ((bool) payload.D)
                        {
                            Log(LogMessageSeverity.Information, "Session is resumable, resuming...");
                            await SendResumeAsync().ConfigureAwait(false);
                            _resuming = true;
                        }
                        else
                        {
                            Log(LogMessageSeverity.Information, "Session is not resumable, identifying...");
                            await SendIdentifyAsync().ConfigureAwait(false);
                        }
                    }
                    break;
                }

                case Opcode.Hello:
                {
                    var data = Serializer.ToObject<HelloModel>(payload.D);
                    _heartbeatInterval = data.HeartbeatInterval;
                    _ = RunHeartbeatAsync();
                    try
                    {
                        await _resumeSemaphore.WaitAsync().ConfigureAwait(false);
                        if (_resuming)
                        {
                            Log(LogMessageSeverity.Information, "Received Hello after requesting a resume, not identifying.");
                            return;
                        }
                    }
                    finally
                    {
                        _resumeSemaphore.Release();
                    }

                    Log(LogMessageSeverity.Information, "Received Hello, identifying...");
                    await SendIdentifyAsync().ConfigureAwait(false);
                    break;
                }

                case Opcode.HeartbeatAck:
                {
                    _lastHeartbeatSent = _lastHeartbeatSend;
                    _lastHeartbeatAck = DateTimeOffset.UtcNow;
                    Log(LogMessageSeverity.Debug, "Acknowledged Heartbeat.");
                    break;
                }
            }
        }

        private async Task HandleDispatchAsync(PayloadModel payload, GatewayDispatch? gatewayEvent = null)
        {
            if (gatewayEvent == null) // if not queued
            {
                if (_lastSequenceNumber == payload.S)
                    Log(LogMessageSeverity.Warning, $"S is the same as the previous one: {payload.S}.");

                try
                {
                    gatewayEvent = Serializer.ToObject<GatewayDispatch>(payload.T);
                }
                catch (SerializationException)
                { }

                if (gatewayEvent == null)
                {
                    Log(LogMessageSeverity.Warning, $"Unknown dispatch: {payload.T}\n{payload.D}.");
                    return;
                }

                if (gatewayEvent != GatewayDispatch.GuildMembersChunk
                    && gatewayEvent != GatewayDispatch.GuildCreate
                    && gatewayEvent != GatewayDispatch.GuildSync
                    && gatewayEvent != GatewayDispatch.Ready)
                {
                    var tcs = _readyTaskCompletionSource;
                    if (tcs != null && !tcs.Task.IsCompleted)
                    {
                        Log(LogMessageSeverity.Debug, $"Queueing up {gatewayEvent.Value}.");
                        _readyPayloadQueue.Enqueue((payload, gatewayEvent.Value));
                        return;
                    }
                }
            }

            _lastSequenceNumber = payload.S;
            //if (IsBot)
            Log(LogMessageSeverity.Trace, $"Dispatch: {gatewayEvent.Value}.");
            switch (gatewayEvent)
            {
                case GatewayDispatch.Ready:
                {
                    var model = Serializer.ToObject<ReadyModel>(payload.D);
                    _sessionId = model.SessionId;
                    _trace = model.Trace;
                    RestClient.CurrentUser.SetValue(new RestCurrentUser(RestClient, model.User));
                    var sharedUser = new CachedSharedUser(this, model.User);
                    CurrentUser = new CachedCurrentUser(sharedUser, model.User, model.Relationships?.Length ?? 0, model.Notes?.Count ?? 0);
                    sharedUser.References++;
                    _users.TryAdd(model.User.Id, CurrentUser.SharedUser);

                    try
                    {
                        switch (TokenType)
                        {
                            case TokenType.Bearer:
                            case TokenType.User:
                            {
                                foreach (var guildModel in model.Guilds)
                                    _guilds.TryAdd(guildModel.Id, new CachedGuild(this, guildModel));

                                foreach (var note in model.Notes)
                                    CurrentUser.AddOrUpdateNote(note.Key, note.Value, (_, __) => note.Value);

                                for (var i = 0; i < model.Relationships.Length; i++)
                                {
                                    var relationshipModel = model.Relationships[i];
                                    var relationship = new CachedRelationship(this, relationshipModel);
                                    CurrentUser.TryAddRelationship(relationship);
                                }

                                for (var i = 0; i < model.PrivateChannels.Length; i++)
                                {
                                    var channelModel = model.PrivateChannels[i];
                                    var channel = CachedPrivateChannel.Create(this, channelModel);
                                    _privateChannels.TryAdd(channel.Id, channel);
                                }

                                await SendGuildSyncAsync(_guilds.Keys.Select(x => x.RawValue)).ConfigureAwait(false);
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log(LogMessageSeverity.Error, "An exception occurred while handling ready.", ex);
                    }

                    _readyTaskCompletionSource = new TaskCompletionSource<bool>();
                    _ = DelayedInvokeReadyAsync();
                    break;
                }

                case GatewayDispatch.Resumed:
                    _resuming = false;
                    Log(LogMessageSeverity.Information, "Resumed.");
                    break;

                case GatewayDispatch.ChannelCreate:
                {
                    var model = Serializer.ToObject<ChannelModel>(payload.D);
                    CachedChannel channel;
                    if (model.GuildId != null)
                    {
                        var guild = GetGuild(model.GuildId.Value);
                        channel = CachedGuildChannel.Create(this, model, guild);
                        guild._channels.TryAdd(channel.Id, (CachedGuildChannel) channel);
                    }
                    else
                    {
                        channel = _privateChannels.GetOrAdd(model.Id, _ => CachedPrivateChannel.Create(this, model));
                    }

                    await _channelCreated.InvokeAsync(new ChannelCreatedEventArgs(channel)).ConfigureAwait(false);
                    return;
                }

                case GatewayDispatch.ChannelUpdate:
                {
                    var model = Serializer.ToObject<ChannelModel>(payload.D);
                    var channel = GetChannel(model.Id);
                    if (channel == null)
                    {
                        Log(LogMessageSeverity.Warning, $"Unknown channel in ChannelUpdate. Id: {model.Id}.");
                        return;
                    }
                    var before = channel.Clone();
                    channel.Update(model);
                    await _channelUpdated.InvokeAsync(new ChannelUpdatedEventArgs(before, channel)).ConfigureAwait(false);
                    return;
                }

                case GatewayDispatch.ChannelDelete:
                {
                    var model = Serializer.ToObject<ChannelModel>(payload.D);
                    CachedChannel channel;
                    if (model.GuildId != null)
                    {
                        var guild = GetGuild(model.GuildId.Value);
                        guild._channels.TryRemove(model.Id, out var guildChannel);
                        channel = guildChannel;
                    }
                    else
                    {
                        _privateChannels.TryRemove(model.Id, out var privateChannel);
                        channel = privateChannel;
                    }

                    if (channel == null)
                    {
                        Log(LogMessageSeverity.Warning, $"Unknown channel in ChannelDelete. Id: {model.Id}.");
                        return;
                    }

                    await _channelDeleted.InvokeAsync(new ChannelDeletedEventArgs(channel)).ConfigureAwait(false);
                    return;
                }

                case GatewayDispatch.ChannelPinsUpdate:
                {
                    // TODO:
                    await _channelPinsUpdated.InvokeAsync(new ChannelPinsUpdatedEventArgs(this)).ConfigureAwait(false);
                    return;
                }

                case GatewayDispatch.GuildCreate:
                {
                    _lastGuildCreate = DateTimeOffset.UtcNow;
                    var model = Serializer.ToObject<WebSocketGuildModel>(payload.D);
                    var guild = _guilds.AddOrUpdate(model.Id, new CachedGuild(this, model), (_, oldValue) =>
                    {
                        oldValue.Update(model);
                        return oldValue;
                    });

                    if (model.Unavailable.HasValue)
                    {
                        Log(LogMessageSeverity.Information, $"Guild '{guild}' ({guild.Id}) became available.");
                        await _guildAvailable.InvokeAsync(new GuildAvailableEventArgs(guild)).ConfigureAwait(false);
                    }
                    else
                    {
                        if (guild.IsLarge)
                            _ = SendRequestOfflineMembersAsync(guild.Id);

                        Log(LogMessageSeverity.Information, $"Joined guild '{guild}' ({guild.Id}).");
                        await _joinedGuild.InvokeAsync(new JoinedGuildEventArgs(guild)).ConfigureAwait(false);
                    }
                    return;
                }

                case GatewayDispatch.GuildUpdate:
                {
                    var model = Serializer.ToObject<GuildModel>(payload.D);
                    var guild = GetGuild(model.Id);
                    var oldGuild = guild.Clone();
                    guild.Update(model);

                    await _guildUpdated.InvokeAsync(new GuildUpdatedEventArgs(oldGuild, guild)).ConfigureAwait(false);
                    return;
                }

                case GatewayDispatch.GuildDelete:
                {
                    var model = Serializer.ToObject<WebSocketGuildModel>(payload.D);
                    if (model.Unavailable.HasValue)
                    {
                        _guilds.TryGetValue(model.Id, out var guild);
                        // TODO set unavailable or something
                        Log(LogMessageSeverity.Information, $"Guild '{guild}' ({guild.Id}) became unavailable.");
                        await _guildUnavailable.InvokeAsync(new GuildUnavailableEventArgs(guild)).ConfigureAwait(false);
                        return;
                    }
                    else
                    {
                        _guilds.TryRemove(model.Id, out var guild);
                        foreach (var member in guild.Members.Values)
                            member.SharedUser.References--;

                        Log(LogMessageSeverity.Information, $"Left guild '{guild}' ({guild.Id}).");
                        await _leftGuild.InvokeAsync(new LeftGuildEventArgs(guild)).ConfigureAwait(false);
                        return;
                    }
                }

                case GatewayDispatch.GuildBanAdd:
                {
                    var model = Serializer.ToObject<GuildBanAddModel>(payload.D);
                    var guild = GetGuild(model.GuildId);
                    var user = guild._members.TryGetValue(model.User.Id, out var member)
                        ? member
                        : GetSharedOrUnknownUser(model.User);

                    await _memberBanned.InvokeAsync(new MemberBannedEventArgs(guild, user));
                    return;
                }

                case GatewayDispatch.GuildBanRemove:
                {
                    var model = Serializer.ToObject<GuildBanRemoveModel>(payload.D);
                    var guild = GetGuild(model.GuildId);
                    var user = GetSharedOrUnknownUser(model.User);

                    await _memberBanned.InvokeAsync(new MemberBannedEventArgs(guild, user));
                    return;
                }

                case GatewayDispatch.GuildEmojisUpdate:
                {
                    return;
                }

                case GatewayDispatch.GuildIntegrationsUpdate:
                {
                    return;
                }

                case GatewayDispatch.GuildMemberAdd:
                {
                    var model = Serializer.ToObject<GuildMemberAddModel>(payload.D);
                    var guild = GetGuild(model.GuildId);
                    var member = CreateMember(guild, model, model.User);

                    await _memberJoined.InvokeAsync(new MemberJoinedEventArgs(member)).ConfigureAwait(false);
                    return;
                }

                case GatewayDispatch.GuildMemberRemove:
                {
                    var model = Serializer.ToObject<GuildMemberRemoveModel>(payload.D);
                    var guild = GetGuild(model.GuildId);
                    var user = guild.TryRemoveMember(model.User.Id, out var member)
                        ? (CachedUser) member
                        : new CachedUnknownUser(this, model.User);

                    await _memberLeft.InvokeAsync(new MemberLeftEventArgs(guild, user));
                    return;
                }

                case GatewayDispatch.GuildMemberUpdate:
                {
                    var model = Serializer.ToObject<GuildMemberUpdateModel>(payload.D);
                    var guild = GetGuild(model.GuildId);
                    var member = guild.GetMember(model.User.Id);
                    CachedMember oldMember;
                    if (member != null)
                    {
                        oldMember = member.Clone();
                        member.Update(model);
                    }
                    else
                    {
                        oldMember = null;
                        member = CreateMember(guild, new MemberModel
                        {
                            Nick = model.Nick,
                            Roles = model.Roles
                        }, model.User, true);
                    }

                    await _memberUpdated.InvokeAsync(new MemberUpdatedEventArgs(oldMember, member)).ConfigureAwait(false);
                    return;
                }

                case GatewayDispatch.GuildMembersChunk:
                {
                    var model = Serializer.ToObject<GuildMembersChunkModel>(payload.D);
                    var guild = GetGuild(model.GuildId);
                    if (--guild.ChunksExpected == 0)
                    {
                        guild.ChunkTcs.SetResult(true);
                        GC.Collect();
                    }

                    guild.Update(model);
                    return;
                }

                case GatewayDispatch.GuildSync:
                {
                    var model = Serializer.ToObject<GuildSyncModel>(payload.D);
                    var guild = GetGuild(model.Id);
                    guild?.Update(model);
                    guild.SyncTcs.SetResult(true);
                    GC.Collect();
                    return;
                }

                case GatewayDispatch.GuildRoleCreate:
                {
                    var model = Serializer.ToObject<GuildRoleCreateModel>(payload.D);
                    var guild = GetGuild(model.GuildId);
                    var role = guild._roles.AddOrUpdate(model.Role.Id, _ => new CachedRole(this, model.Role, guild), (_, old) =>
                    {
                        old.Update(model.Role);
                        return old;
                    });

                    await _roleCreated.InvokeAsync(new RoleCreatedEventArgs(role)).ConfigureAwait(false);
                    return;
                }

                case GatewayDispatch.GuildRoleUpdate:
                {
                    var model = Serializer.ToObject<GuildRoleCreateModel>(payload.D);
                    var guild = GetGuild(model.GuildId);
                    CachedRole before = null;
                    var after = guild._roles.AddOrUpdate(model.Role.Id,
                        _ => new CachedRole(this, model.Role, guild),
                        (_, old) =>
                        {
                            before = old.Clone();
                            old.Update(model.Role);
                            return old;
                        });

                    await _roleUpdated.InvokeAsync(new RoleUpdatedEventArgs(before, after)).ConfigureAwait(false);
                    return;
                }

                case GatewayDispatch.GuildRoleDelete:
                {
                    var model = Serializer.ToObject<GuildRoleCreateModel>(payload.D);
                    var guild = GetGuild(model.GuildId);
                    guild._roles.TryRemove(model.Role.Id, out var role);

                    await _roleDeleted.InvokeAsync(new RoleDeletedEventArgs(role)).ConfigureAwait(false);
                    return;
                }

                case GatewayDispatch.MessageAck:
                {
                    var model = Serializer.ToObject<MessageAckModel>(payload.D);
                    if (!(GetChannel(model.ChannelId) is ICachedMessageChannel channel))
                    {
                        return;
                    }

                    var message = channel.GetMessage(model.MessageId);
                    await _messageAcknowledged.InvokeAsync(new MessageAcknowledgedEventArgs(channel,
                        new OptionalSnowflakeEntity<CachedMessage>(message, model.MessageId))).ConfigureAwait(false);
                    return;
                }

                case GatewayDispatch.MessageCreate:
                {
                    var model = Serializer.ToObject<MessageModel>(payload.D);
                    ICachedMessageChannel channel;
                    CachedUser author = null;
                    CachedGuild guild = null;
                    var isWebhook = model.WebhookId.HasValue && model.WebhookId.Value != null;
                    if (model.GuildId != null)
                    {
                        guild = GetGuild(model.GuildId.Value);
                        channel = guild.GetTextChannel(model.ChannelId);

                        if (!isWebhook)
                            author = model.Author.HasValue && model.Member.HasValue
                                ? GetOrCreateMember(guild, model.Member.Value, model.Author.Value)
                                : guild.GetMember(model.Author.Value.Id);
                    }
                    else
                    {
                        channel = GetPrivateChannel(model.ChannelId);

                        if (!isWebhook)
                            author = GetUser(model.Author.Value.Id);
                    }

                    if (author == null && !isWebhook)
                    {
                        Log(LogMessageSeverity.Warning, $"Uncached author and/or guild == null in MESSAGE_CREATE.\n{payload.D}");
                        return;
                    }

                    var message = CachedMessage.Create(this, model, channel, author);
                    if (message is CachedUserMessage userMessage)
                    {
                        if (MessageCacheSize > 0)
                            CachedMessages.AddOrUpdate(channel.Id, _ => new CircularBuffer<CachedUserMessage>(MessageCacheSize)
                            {
                                userMessage
                            }, (_, old) =>
                            {
                                old.Add(userMessage);
                                return old;
                            });
                        if (guild != null)
                            ((CachedTextChannel) channel).LastMessageId = message.Id;
                        else
                            ((CachedPrivateChannel) channel).LastMessageId = message.Id;
                    }

                    await _messageReceived.InvokeAsync(new MessageReceivedEventArgs(message)).ConfigureAwait(false);
                    return;
                }

                case GatewayDispatch.MessageUpdate:
                {
                    var model = Serializer.ToObject<MessageModel>(payload.D);
                    if (!model.EditedTimestamp.HasValue)
                        return;

                    ICachedMessageChannel channel;
                    CachedGuild guild = null;
                    if (model.GuildId != null)
                    {
                        guild = GetGuild(model.GuildId.Value);
                        channel = guild.GetTextChannel(model.ChannelId);
                    }
                    else
                    {
                        channel = GetPrivateChannel(model.ChannelId);
                    }

                    if (channel == null)
                    {
                        Log(LogMessageSeverity.Warning, $"Uncached channel in MessageUpdated. Id: {model.ChannelId}");
                        return;
                    }

                    var message = channel.GetMessage(model.Id);
                    var before = message?.Clone();
                    var isWebhook = model.WebhookId.HasValue && model.WebhookId.Value != null;
                    if (message == null)
                    {
                        CachedUser author = null;
                        if (!model.Author.HasValue && !isWebhook)
                        {
                            Log(LogMessageSeverity.Warning, "Unknown message and author has no value in MessageUpdated.");
                            return;
                        }
                        else if (!isWebhook)
                        {
                            if (guild != null)
                            {
                                if (guild.Members.TryGetValue(model.Author.Value.Id, out var member))
                                    author = member;

                                else if (model.Member.HasValue)
                                    author = GetOrCreateMember(guild, model.Member.Value, model.Author.Value);
                            }
                            else
                            {
                                author = GetUser(model.Author.Value.Id);
                            }
                        }
                        else
                        {
                            // TODO?
                            return;
                        }

                        if (author == null)
                        {
                            // TODO
                            Log(LogMessageSeverity.Error, "Author is still null in MessageUpdate.");
                            return;
                        }

                        message = new CachedUserMessage(this, model, channel, author);
                    }
                    else
                    {
                        message.Update(model);
                    }
                    await _messageUpdated.InvokeAsync(new MessageUpdatedEventArgs(channel,
                        new OptionalSnowflakeEntity<CachedUserMessage>(before, model.Id),
                        message)).ConfigureAwait(false);
                    return;
                }

                case GatewayDispatch.MessageDelete:
                {
                    var model = Serializer.ToObject<MessageDeleteModel>(payload.D);
                    var channel = model.GuildId != null
                        ? GetGuildChannel(model.ChannelId) as ICachedMessageChannel
                        : GetPrivateChannel(model.ChannelId);

                    if (channel == null)
                    {
                        Log(LogMessageSeverity.Warning, $"Uncached channel in MessageDeleted. Id: {model.ChannelId}");
                        return;
                    }

                    var message = channel.GetMessage(model.Id);
                    // TODO remove the deleted message from the cache
                    await _messageDeleted.InvokeAsync(new MessageDeletedEventArgs(channel,
                        new OptionalSnowflakeEntity<CachedUserMessage>(message, model.Id))).ConfigureAwait(false);
                    return;
                }

                case GatewayDispatch.MessageDeleteBulk:
                {
                    var model = Serializer.ToObject<MessageDeleteBulkModel>(payload.D);
                    if (model.GuildId == null)
                    {
                        Log(LogMessageSeverity.Error, $"MessageDeleteBulk contains a null guild_id. Channel id: {model.ChannelId}.");
                        return;
                    }

                    var guild = GetGuild(model.GuildId.Value);
                    var channel = guild.GetTextChannel(model.ChannelId);
                    var messages = new OptionalSnowflakeEntity<CachedUserMessage>[model.Ids.Length];
                    for (var i = 0; i < model.Ids.Length; i++)
                    {
                        var id = model.Ids[i];
                        messages[i] = new OptionalSnowflakeEntity<CachedUserMessage>(channel.GetMessage(id), id);
                    }

                    // TODO remove deleted messages from the cache
                    await _messagesBulkDeleted.InvokeAsync(new MessagesBulkDeletedEventArgs(channel,
                        new ReadOnlyList<OptionalSnowflakeEntity<CachedUserMessage>>(messages))).ConfigureAwait(false);
                    return;
                }

                case GatewayDispatch.MessageReactionAdd:
                {
                    var model = Serializer.ToObject<MessageReactionAddModel>(payload.D);
                    var channel = model.GuildId != null
                        ? GetGuildChannel(model.ChannelId) as ICachedMessageChannel
                        : GetPrivateChannel(model.ChannelId);

                    if (channel == null)
                    {
                        Log(LogMessageSeverity.Warning, $"Uncached channel in MessageReactionAdd. Id: {model.ChannelId}");
                        return;
                    }

                    var message = channel.GetMessage(model.MessageId);
                    IEmoji emoji;
                    ReactionData reaction = null;
                    if (message != null)
                    {
                        if (message._reactions.TryGetValue(model.Emoji.ToEmoji(), out reaction))
                        {
                            reaction.Count++;
                            if (model.UserId == CurrentUser.Id)
                                reaction.HasCurrentUserReacted = true;
                        }
                        else
                        {
                            reaction = new ReactionData(new ReactionModel
                            {
                                Count = 1,
                                Me = model.UserId == CurrentUser.Id,
                                Emoji = model.Emoji
                            });
                            message._reactions.TryAdd(reaction.Emoji, reaction);
                        }
                        emoji = reaction.Emoji;
                    }
                    else
                    {
                        emoji = model.Emoji.ToEmoji();
                    }
                    await _reactionAdded.InvokeAsync(
                        new ReactionAddedEventArgs(
                            channel,
                            new DownloadableOptionalSnowflakeEntity<CachedMessage, RestMessage>(
                                message, model.MessageId, options => RestClient.GetMessageAsync(channel.Id, model.MessageId, options)),
                            new DownloadableOptionalSnowflakeEntity<CachedUser, RestUser>(message?.Author, model.UserId,
                            async options => model.GuildId != null
                                ? await RestClient.GetMemberAsync(model.GuildId.Value, model.UserId, options).ConfigureAwait(false)
                                : await RestClient.GetUserAsync(model.UserId, options).ConfigureAwait(false)),
                            reaction ?? Optional<ReactionData>.Empty,
                            emoji)).ConfigureAwait(false);
                    return;
                }

                case GatewayDispatch.MessageReactionRemove:
                {
                    var model = Serializer.ToObject<MessageReactionRemoveModel>(payload.D);
                    var channel = model.GuildId != null
                        ? GetGuildChannel(model.ChannelId) as ICachedMessageChannel
                        : GetPrivateChannel(model.ChannelId);

                    if (channel == null)
                    {
                        Log(LogMessageSeverity.Warning, $"Uncached channel in MessageReactionRemove. Id: {model.ChannelId}");
                        return;
                    }

                    var message = channel.GetMessage(model.MessageId);
                    ReactionData reaction = null;
                    if (message != null)
                    {
                        message._reactions.TryGetValue(model.Emoji.ToEmoji(), out reaction);
                        if (reaction != null)
                        {
                            var count = reaction.Count - 1;
                            if (count == 0)
                            {
                                message._reactions.TryRemove(reaction.Emoji, out _);
                            }
                            else
                            {
                                reaction.Count--;
                                if (model.UserId == CurrentUser.Id)
                                    reaction.HasCurrentUserReacted = false;
                            }
                        }
                    }
                    await _reactionRemoved.InvokeAsync(
                        new ReactionRemovedEventArgs(
                            channel,
                            new DownloadableOptionalSnowflakeEntity<CachedMessage, RestMessage>(message, model.MessageId,
                            options => RestClient.GetMessageAsync(channel.Id, model.MessageId, options)),
                            new DownloadableOptionalSnowflakeEntity<CachedUser, RestUser>(message?.Author, model.UserId,
                            async options => model.GuildId != null
                                ? await RestClient.GetMemberAsync(model.GuildId.Value, model.UserId, options).ConfigureAwait(false)
                                : await RestClient.GetUserAsync(model.UserId, options).ConfigureAwait(false)),
                            reaction ?? Optional<ReactionData>.Empty,
                            model.Emoji.ToEmoji())).ConfigureAwait(false);
                    return;
                }

                case GatewayDispatch.MessageReactionRemoveAll:
                {
                    var model = Serializer.ToObject<MessageReactionRemoveAllModel>(payload.D);
                    var channel = model.GuildId != null
                        ? GetGuildChannel(model.ChannelId) as ICachedMessageChannel
                        : GetPrivateChannel(model.ChannelId);

                    if (channel == null)
                    {
                        Log(LogMessageSeverity.Warning, $"Uncached channel in MessageReactionRemoveAll. Id: {model.ChannelId}");
                        return;
                    }

                    var message = channel.GetMessage(model.MessageId);
                    message?._reactions.Clear();
                    await _allReactionsRemoved.InvokeAsync(new AllReactionsRemovedEventArgs(
                        channel,
                        new DownloadableOptionalSnowflakeEntity<CachedMessage, RestMessage>(message, model.MessageId,
                            options => RestClient.GetMessageAsync(channel.Id, model.MessageId, options)))).ConfigureAwait(false);
                    return;
                }

                case GatewayDispatch.PresenceUpdate:
                {
                    var model = Serializer.ToObject<PresenceUpdateModel>(payload.D);
                    if (model.GuildId == null)
                    {
                        if (!_users.TryGetValue(model.User.Id, out var user))
                        {
                            if (!model.User.Username.HasValue)
                                return;

                            user = GetOrAddSharedUser(model.User);
                        }

                        user.Update(model);
                    }
                    else
                    {
                        var guild = GetGuild(model.GuildId.Value);
                        var member = guild.GetMember(model.User.Id);
                        if (member == null)
                        {
                            if (!model.User.Username.HasValue)
                                return;

                            member = CreateMember(guild, new MemberModel
                            {
                                Nick = model.Nick,
                                Roles = model.Roles
                            }, model.User);
                        }

                        member.Update(model);
                    }

                    return;
                }

                case GatewayDispatch.RelationshipAdd:
                {
                    var model = Serializer.ToObject<RelationshipModel>(payload.D);
                    if (CurrentUser.Relationships.TryGetValue(model.Id, out var relationship))
                    {
                        relationship.Update(model);
                    }
                    else
                    {
                        relationship = new CachedRelationship(this, model);
                        CurrentUser.TryAddRelationship(relationship);
                    }

                    await _relationshipCreated.InvokeAsync(new RelationshipCreatedEventArgs(relationship)).ConfigureAwait(false);
                    return;
                }

                case GatewayDispatch.RelationshipRemove:
                {
                    var model = Serializer.ToObject<RelationshipModel>(payload.D);
                    CurrentUser.TryRemoveRelationship(model.Id, out var relationship);

                    await _relationshipDeleted.InvokeAsync(new RelationshipDeletedEventArgs(relationship));
                    return;
                }

                case GatewayDispatch.TypingStart:
                {
                    var model = Serializer.ToObject<TypingStartModel>(payload.D);
                    CachedGuild guild = null;
                    ICachedMessageChannel channel;
                    CachedUser user;
                    if (model.GuildId != null)
                    {
                        guild = GetGuild(model.GuildId.Value);
                        channel = guild.GetTextChannel(model.ChannelId);
                        user = guild.GetMember(model.UserId);
                    }
                    else
                    {
                        channel = GetPrivateChannel(model.ChannelId);
                        user = GetUser(model.UserId);
                    }
                    await _typingStarted.InvokeAsync(new TypingStartedEventArgs(this,
                        new OptionalSnowflakeEntity<ICachedMessageChannel>(channel, model.ChannelId),
                        new DownloadableOptionalSnowflakeEntity<CachedUser, RestUser>(user, model.UserId,
                        async options => guild != null
                            ? await guild.GetMemberAsync(model.UserId, options).ConfigureAwait(false)
                            : await RestClient.GetUserAsync(model.UserId, options).ConfigureAwait(false)),
                        DateTimeOffset.FromUnixTimeSeconds(model.Timestamp))).ConfigureAwait(false);

                    return;
                }

                case GatewayDispatch.UserNoteUpdate:
                {
                    var model = Serializer.ToObject<UserNoteUpdateModel>(payload.D);
                    string oldNote = null;
                    CurrentUser.AddOrUpdateNote(model.Id, model.Note, (_, old) =>
                    {
                        oldNote = old;
                        return model.Note;
                    });

                    await _userNoteUpdated.InvokeAsync(new UserNoteUpdatedEventArgs(this, model.Id, oldNote, model.Note)).ConfigureAwait(false);
                    return;
                }

                case GatewayDispatch.UserUpdate:
                {
                    var model = Serializer.ToObject<UserModel>(payload.D);
                    var user = GetUser(model.Id);
                    var userBefore = user.Clone();
                    user.Update(model);

                    await _userUpdated.InvokeAsync(new UserUpdatedEventArgs(userBefore, user)).ConfigureAwait(false);
                    return;
                }

                case GatewayDispatch.VoiceStateUpdate:
                {
                    var model = Serializer.ToObject<VoiceStateModel>(payload.D);
                    if (model.GuildId == null)
                        return;

                    var guild = GetGuild(model.GuildId.Value);
                    var member = GetOrCreateMember(guild, model.Member, model.Member.User);
                    var oldMember = member.Clone();
                    member.Update(model);
                    // TODO split voice states from members
                    await _voiceStateUpdatedEvent.InvokeAsync(new VoiceStateUpdatedEventArgs(oldMember, member)).ConfigureAwait(false);
                    return;
                }

                case GatewayDispatch.VoiceServerUpdate:
                {
                    var model = Serializer.ToObject<VoiceServerUpdateModel>(payload.D);
                    if (model.GuildId == null)
                        return;

                    var guild = GetGuild(model.GuildId.Value);
                    await _voiceServerUpdatedEvent.InvokeAsync(new VoiceServerUpdatedEventArgs(guild, model.Token, model.Endpoint));
                    return;
                }

                case GatewayDispatch.WebhooksUpdate:
                    return;

                default:
                    Log(LogMessageSeverity.Warning, $"Unknown dispatch: {payload.T}\n{payload.D}");
                    return;
            }
        }

        private async Task RunHeartbeatAsync()
        {
            try
            {
                _heartbeatCts?.Cancel();
            }
            catch { }
            _heartbeatCts?.Dispose();
            _heartbeatCts = new CancellationTokenSource();
            while (!_heartbeatCts.IsCancellationRequested)
            {
                Log(LogMessageSeverity.Debug, $"Heartbeat: delaying for {_heartbeatInterval}ms.");
                try
                {
                    await Task.Delay(_heartbeatInterval, _heartbeatCts.Token).ConfigureAwait(false);
                }
                catch (TaskCanceledException)
                {
                    Log(LogMessageSeverity.Warning, "Heartbeat: delay cancelled, returning.");
                    return;
                }

                Log(LogMessageSeverity.Debug, "Heartbeat: Sending...");
                var success = false;
                while (!success && !_heartbeatCts.IsCancellationRequested)
                {
                    try
                    {
                        await SendHeartbeatAsync().ConfigureAwait(false);
                        success = true;
                    }
                    catch (WebSocketException ex) when (ex.WebSocketErrorCode == WebSocketError.InvalidState)
                    {
                        Log(LogMessageSeverity.Error, "Heartbeat: send errored - websocket in an invalid state. Returning.");
                        return;
                    }
                    catch (WebSocketException ex)
                    {
                        Log(LogMessageSeverity.Error, $"Heartbeat: send errored - {ex.WebSocketErrorCode}. Returning.");
                        return;
                    }
                    catch (TaskCanceledException)
                    {
                        Log(LogMessageSeverity.Error, "Heartbeat: send cancelled. Returning.");
                        return;
                    }
                    catch (Exception ex)
                    {
                        Log(LogMessageSeverity.Error, $"Heartbeat: send failed. Retrying in 5 seconds.", ex);
                        await Task.Delay(5000).ConfigureAwait(false);
                    }
                }
                _lastHeartbeatSend = DateTimeOffset.UtcNow;
            }
        }

        private Task SendGuildSyncAsync(IEnumerable<ulong> guildIds)
            => SendAsync(new PayloadModel
            {
                Op = Opcode.GuildSync,
                D = guildIds
            });

        private Task SendRequestOfflineMembersAsync(ulong[] guildIds)
            => SendAsync(new PayloadModel
            {
                Op = Opcode.RequestGuildMembers,
                D = new RequestOfflineMembersModel
                {
                    GuildId = guildIds,
                    Query = "",
                    Limit = 0,
                    Presences = true
                }
            });

        private Task SendRequestOfflineMembersAsync(ulong guildId)
             => SendAsync(new PayloadModel
             {
                 Op = Opcode.RequestGuildMembers,
                 D = new RequestOfflineMembersModel
                 {
                     GuildId = guildId,
                     Query = "",
                     Limit = 0,
                     Presences = true
                 }
             });

        private Task SendResumeAsync()
            => SendAsync(new PayloadModel
            {
                Op = Opcode.Resume,
                D = new ResumeModel
                {
                    Token = Token,
                    Seq = _lastSequenceNumber,
                    SessionId = _sessionId
                }
            });

        private Task SendHeartbeatAsync()
            => SendAsync(new PayloadModel
            {
                Op = Opcode.Heartbeat,
                D = _lastSequenceNumber
            }, _heartbeatCts.Token);

        private Task SendIdentifyAsync()
            => SendAsync(new PayloadModel
            {
                Op = Opcode.Identify,
                D = new IdentifyModel
                {
                    Token = Token,
                    LargeThreshold = 250,
                    Presence = new UpdateStatusModel
                    {
                        Status = Status,
                        Game = Activity ?? Optional<ActivityModel>.Empty
                    },
                    GuildSubscriptions = _guildSubscriptions
                }
            });

        private Task SendAsync(PayloadModel payload)
            => SendAsync(payload, CancellationToken.None);

        private async Task SendAsync(PayloadModel payload, CancellationToken cancellationToken)
        {
            var json = Serializer.Serialize(payload);
            await _ws.SendAsync(new WebSocketRequest(json, cancellationToken)).ConfigureAwait(false);
        }

        private async Task DelayedInvokeReadyAsync()
        {
            if (IsBot)
            {
                var last = _lastGuildCreate;
                while (last == null || (DateTimeOffset.UtcNow - last).Value.TotalSeconds < 2)
                {
                    await Task.Delay(last == null ? 2000 : 1000).ConfigureAwait(false);
                    last = _lastGuildCreate;
                }

                var batches = _guilds.Values.Where(x => !x.IsChunked).Batch(75).Select(x => x.ToArray()).ToArray();
                var tasks = new Task[batches.Length];
                for (var i = 0; i < batches.Length; i++)
                {
                    var batch = batches[i];
                    for (var j = 0; j < batch.Length; j++)
                    {
                        var guild = batch[j];
                        Log(LogMessageSeverity.Information, $"Requesting offline members for {guild.Name}. Expecting {guild.ChunksExpected} {(guild.ChunksExpected == 1 ? "chunk" : "chunks")}.");
                    }

                    await SendRequestOfflineMembersAsync(batch.Select(x => x.Id.RawValue).ToArray()).ConfigureAwait(false);
                    tasks[i] = Task.WhenAll(batch.Select(x => x.ChunkTcs.Task));
                }

                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
            else
            {
                var guilds = _guilds.Values;
                Log(LogMessageSeverity.Information, $"Awaiting sync for {guilds.Count} guilds.");
                await Task.WhenAll(guilds.Select(x => x.SyncTcs.Task)).ConfigureAwait(false);
            }

            await _ready.InvokeAsync(new ReadyEventArgs(this, _sessionId, _trace)).ConfigureAwait(false);
            while (_readyPayloadQueue.TryDequeue(out var queuedPayload))
            {
                Log(LogMessageSeverity.Debug, $"Firing queued up payload: {queuedPayload.Item2} with S: {queuedPayload.Item1.S}.");
                await HandleDispatchAsync(queuedPayload.Item1, queuedPayload.Item2).ConfigureAwait(false);
            }

            _readyTaskCompletionSource.SetResult(true);
            _readyTaskCompletionSource = null;
        }

        public override void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            CurrentUser = null;
            lock (_reconnectionLock)
            {
                _manualDisconnection = true;
                try
                {
                    _heartbeatCts?.Cancel();
                }
                catch { }
                _heartbeatCts?.Dispose();
                _heartbeatCts = null;
                _ws.Dispose();
            }
            RestClient.Dispose();
        }
    }
}