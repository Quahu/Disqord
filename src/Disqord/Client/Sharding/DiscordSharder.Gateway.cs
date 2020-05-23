using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Logging;
using Disqord.Rest;

namespace Disqord.Sharding
{
    public partial class DiscordSharder : DiscordClientBase, IDiscordSharder
    {
        public override TimeSpan? Latency
        {
            get
            {
                var average = _gateways.Average(x => x.Latency?.TotalMilliseconds);
                return average != null
                    ? TimeSpan.FromMilliseconds(average.Value)
                    : (TimeSpan?) null;
            }
        }

        private DiscordClientGateway[] _gateways;
        private RestGatewayBotResponse _gatewayBotResponse;

        public override async Task RunAsync(CancellationToken cancellationToken = default)
        {
            _gatewayBotResponse = await GetGatewayBotUrlAsync().ConfigureAwait(false);
            var shardCount = _shardCount != 0
                ? _shardCount
                : _gatewayBotResponse.ShardCount;
            Log(LogMessageSeverity.Information, $"Starting sharder with {shardCount} shards. There's {_gatewayBotResponse.RemainingSessionAmount} sessions left.");
            _gateways = new DiscordClientGateway[shardCount];
            var shards = new Shard[_gateways.Length];
            Shards = shards.ReadOnly();
            var tasks = new Task[_gateways.Length];
            for (var i = 0; i < _gateways.Length; i++)
            {
                var gateway = new DiscordClientGateway(State, (i, _gateways.Length));
                _gateways[i] = gateway;
                shards[i] = new Shard(this, gateway);
                tasks[i] = gateway.RunAsync(cancellationToken);

                await gateway.WaitForIdentifyAsync().ConfigureAwait(false);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        public override Task StopAsync()
            => throw new NotImplementedException();

        public sealed override Task SetPresenceAsync(UserStatus status)
            => InternalSetPresencesAsync(status);

        public sealed override Task SetPresenceAsync(LocalActivity activity)
            => InternalSetPresencesAsync(activity: activity);

        public sealed override Task SetPresenceAsync(UserStatus status, LocalActivity activity)
            => InternalSetPresencesAsync(status, activity);

        private async Task InternalSetPresencesAsync(UserStatus? status = default, Optional<LocalActivity> activity = default)
        {
            ThrowIfDisposed();

            if (!status.HasValue && !activity.HasValue)
                return;

            for (var i = 0; i < _gateways.Length; i++)
            {
                var gateway = _gateways[i];
                if (status.HasValue)
                    gateway.SetStatus(status.Value);

                if (activity.HasValue)
                    gateway.SetActivity(activity.Value);

                await gateway.SendPresenceAsync().ConfigureAwait(false);
            }
        }

        internal Task InternalSetPresenceAsync(DiscordClientGateway gateway, UserStatus? status = default, Optional<LocalActivity> activity = default)
        {
            ThrowIfDisposed();

            if (!status.HasValue && !activity.HasValue)
                return Task.CompletedTask;

            if (status.HasValue)
                gateway.SetStatus(status.Value);

            if (activity.HasValue)
                gateway.SetActivity(activity.Value);

            return gateway.SendPresenceAsync();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void ValidateShardId(int shardId)
        {
            if (shardId < 0 || shardId >= _gateways.Length)
                throw new ArgumentOutOfRangeException(nameof(shardId));
        }

        public int GetShardId(Snowflake guildId)
            => (int) ((guildId >> 22) % (ulong) _gateways.Length);

        internal override Task<string> GetGatewayAsync(bool isNewSession)
        {
            ThrowIfDisposed();
            return Task.FromResult(_gatewayBotResponse.Url);
        }
    }
}
