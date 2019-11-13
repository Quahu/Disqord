using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Models;
using Disqord.Models.Dispatches;
using Disqord.WebSocket;

namespace Disqord
{
    internal sealed partial class DiscordClientGateway : IDisposable
    {
        private readonly (int ShardId, int ShardCount)? _shards;
        internal UserStatus? _status;
        internal ActivityModel _activity;

        internal Task SendGuildSyncAsync(IEnumerable<ulong> guildIds)
            => SendAsync(new PayloadModel
            {
                Op = Opcode.GuildSync,
                D = guildIds
            });

        internal Task SendRequestOfflineMembersAsync(IEnumerable<ulong> guildIds)
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

        internal Task SendRequestOfflineMembersAsync(ulong guildId)
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

        internal Task SendResumeAsync()
            => SendAsync(new PayloadModel
            {
                Op = Opcode.Resume,
                D = new ResumeModel
                {
                    Token = _client.Token,
                    Seq = _lastSequenceNumber,
                    SessionId = _sessionId
                }
            });

        internal Task SendHeartbeatAsync()
            => SendAsync(new PayloadModel
            {
                Op = Opcode.Heartbeat,
                D = _lastSequenceNumber
            }, _heartbeatCts.Token);

        internal Task SendIdentifyAsync()
            => SendAsync(new PayloadModel
            {
                Op = Opcode.Identify,
                D = new IdentifyModel
                {
                    Token = _client.Token,
                    LargeThreshold = 250,
                    Presence = new UpdateStatusModel
                    {
                        Status = _status,
                        Game = _activity ?? Optional<ActivityModel>.Empty
                    },
                    GuildSubscriptions = true
                }
            });

        internal Task SendAsync(PayloadModel payload)
            => SendAsync(payload, CancellationToken.None);

        internal async Task SendAsync(PayloadModel payload, CancellationToken cancellationToken)
        {
            var json = Serializer.Serialize(payload);
            await _ws.SendAsync(new WebSocketRequest(json, cancellationToken)).ConfigureAwait(false);
        }
    }
}
