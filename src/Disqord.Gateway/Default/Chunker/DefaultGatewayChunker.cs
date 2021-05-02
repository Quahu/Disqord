using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Collections.Synchronized;
using Disqord.Gateway.Api.Models;
using Disqord.Utilities.Binding;
using Disqord.Utilities.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disqord.Gateway.Default
{
    public class DefaultGatewayChunker : IGatewayChunker
    {
        public ILogger Logger { get; }

        public IGatewayClient Client => _binder.Value;

        private readonly ISynchronizedDictionary<string, ChunkOperation> _operations;

        private readonly Binder<IGatewayClient> _binder;

        public DefaultGatewayChunker(
            IOptions<DefaultGatewayChunkerConfiguration> options,
            ILogger<DefaultGatewayChunker> logger)
        {
            Logger = logger;

            _operations = new SynchronizedDictionary<string, ChunkOperation>();

            _binder = new Binder<IGatewayClient>(this);
        }

        public void Bind(IGatewayClient value)
        {
            _binder.Bind(value);
        }

        public ValueTask HandleChunkAsync(GuildMembersChunkJsonModel model)
        {
            var members = new List<IMember>();
            if (model.Members.Length != 0)
            {
                if (Client.CacheProvider.TryGetUsers(out var userCache) && Client.CacheProvider.TryGetMembers(model.GuildId, out var memberCache))
                {
                    foreach (var memberModel in model.Members)
                    {
                        var member = Client.Dispatcher.GetOrAddMember(userCache, memberCache, model.GuildId, memberModel);
                        members.Add(member);
                    }
                }
                else
                {
                    foreach (var memberModel in model.Members)
                    {
                        var member = new TransientMember(Client, model.GuildId, memberModel);
                        members.Add(member);
                    }
                }
            }

            if (!model.Nonce.HasValue)
                return default;

            var isLast = model.ChunkIndex == model.ChunkCount - 1;
            if (!(isLast
                ? _operations.TryRemove(model.Nonce.Value, out var operation)
                : _operations.TryGetValue(model.Nonce.Value, out operation)))
                return default;

            operation.AddMembers(members);
            if (isLast)
            {
                operation.Complete();
                operation.Dispose();
            }

            return default;
        }

        public async ValueTask<bool> ChunkAsync(IGatewayGuild guild, CancellationToken cancellationToken = default)
        {
            if (guild == null)
                throw new ArgumentNullException(nameof(guild));

            if (!Client.CacheProvider.TryGetUsers(out var userCache) || !Client.CacheProvider.TryGetMembers(guild.Id, out var memberCache))
                return false;

            if (memberCache.Count == guild.MemberCount)
                return false;

            var model = new RequestMembersJsonModel
            {
                GuildId = guild.Id,
                Query = "",
                Limit = 0
            };

            await InternalOperationAsync(model, false, cancellationToken).ConfigureAwait(false);
            return true;
        }

        public ValueTask<IReadOnlyDictionary<Snowflake, IMember>> QueryAsync(Snowflake guildId, string query, int limit = 100, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentNullException(nameof(query));

            if (limit == 0 || limit > 100)
                throw new ArgumentOutOfRangeException(nameof(limit));

            var model = new RequestMembersJsonModel
            {
                GuildId = guildId,
                Query = query,
                Limit = limit
            };
            return InternalOperationAsync(model, true, cancellationToken);
        }

        public ValueTask<IReadOnlyDictionary<Snowflake, IMember>> QueryAsync(Snowflake guildId, IEnumerable<Snowflake> memberIds, CancellationToken cancellationToken = default)
        {
            var ids = memberIds.ToArray();
            if (ids.Length == 0 || ids.Length > 100)
                throw new ArgumentOutOfRangeException(nameof(memberIds));

            var model = new RequestMembersJsonModel
            {
                GuildId = guildId,
                // Since the gateway is clowning around and not sending back the nonce for small limits
                // or whatever, let's just ask for the max limit always.
                Limit = 100,
                UserIds = ids
            };
            return InternalOperationAsync(model, true, cancellationToken);
        }

        private async ValueTask<IReadOnlyDictionary<Snowflake, IMember>> InternalOperationAsync(RequestMembersJsonModel model, bool isQuery, CancellationToken cancellationToken)
        {
            var operation = new ChunkOperation(isQuery, cancellationToken);
            _operations.Add(operation.Nonce, operation);
            model.Nonce = operation.Nonce;
            model.Presences = true; // According to the docs should default to false without the presences intent.
            var shard = Client.GetShard(model.GuildId);
            await shard.SendAsync(new GatewayPayloadJsonModel
            {
                Op = GatewayPayloadOperation.RequestMembers,
                D = model
            }, cancellationToken).ConfigureAwait(false);
            return await operation.WaitAsync().ConfigureAwait(false);
        }

        private sealed class ChunkOperation : IDisposable
        {
            public string Nonce { get; }

            private readonly SynchronizedDictionary<Snowflake, IMember> _members;
            private readonly Tcs<IReadOnlyDictionary<Snowflake, IMember>> _tcs;
            private readonly CancellationTokenRegistration _reg;

            public ChunkOperation(bool isQuery, CancellationToken cancellationToken)
            {
                Nonce = Guid.NewGuid().ToString("N");

                _members = isQuery
                    ? new SynchronizedDictionary<Snowflake, IMember>()
                    : null;
                _tcs = new Tcs<IReadOnlyDictionary<Snowflake, IMember>>();

                static void CancellationCallback(object tuple)
                {
                    var (tcs, token) = (ValueTuple<Tcs<IReadOnlyList<IMember>>, CancellationToken>) tuple;
                    tcs.Cancel(token);
                }

                _reg = cancellationToken.UnsafeRegister(CancellationCallback, (_tcs, cancellationToken));
            }

            public Task<IReadOnlyDictionary<Snowflake, IMember>> WaitAsync()
                => _tcs.Task;

            public void AddMembers(IReadOnlyList<IMember> members)
            {
                if (_members != null)
                {
                    for (var i = 0; i < members.Count; i++)
                    {
                        var member = members[i];
                        _members.Add(member.Id, member);
                    }
                }
            }

            public void Complete()
            {
                _tcs.Complete(_members);
            }

            public void Dispose()
            {
                _reg.Dispose();
            }
        }
    }
}
