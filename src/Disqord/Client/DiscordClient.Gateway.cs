using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Logging;

namespace Disqord
{
    public partial class DiscordClient : DiscordClientBase
    {
        /// <summary>
        ///     Gets the latency between heartbeats.
        /// </summary>
        public override TimeSpan? Latency => _gateway.Latency;

        private readonly DiscordClientGateway _gateway;
        private string _gatewayUrl;

        public override Task RunAsync(CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            return _gateway.RunAsync(cancellationToken);
        }

        public override async Task StopAsync()
        {
            ThrowIfDisposed();
            await _gateway.StopAsync().ConfigureAwait(false);
        }

        public sealed override Task SetPresenceAsync(UserStatus status)
            => InternalSetPresenceAsync(status);

        public sealed override Task SetPresenceAsync(LocalActivity activity)
            => InternalSetPresenceAsync(activity: activity);

        public sealed override Task SetPresenceAsync(UserStatus status, LocalActivity activity)
            => InternalSetPresenceAsync(status, activity);

        internal override async Task<string> GetGatewayAsync(bool isNewSession)
        {
            ThrowIfDisposed();

            if (IsBot && isNewSession)
            {
                var botGatewayResponse = await GetGatewayBotUrlAsync().ConfigureAwait(false);
                if (botGatewayResponse.RemainingSessionAmount == 0)
                    throw new SessionLimitException(botGatewayResponse.ResetAfter);

                Log(LogMessageSeverity.Information,
                    $"Sessions used: {botGatewayResponse.MaxSessionAmount - botGatewayResponse.RemainingSessionAmount}/{botGatewayResponse.MaxSessionAmount}. " +
                    $"Resets in {botGatewayResponse.ResetAfter}.");
                return _gatewayUrl = botGatewayResponse.Url;
            }
            else if (_gatewayUrl == null)
            {
                return _gatewayUrl = await GetGatewayUrlAsync().ConfigureAwait(false);
            }
            else
            {
                return _gatewayUrl;
            }
        }

        private Task InternalSetPresenceAsync(UserStatus? status = default, Optional<LocalActivity> activity = default)
        {
            ThrowIfDisposed();

            if (!status.HasValue && !activity.HasValue)
                return Task.CompletedTask;

            if (status.HasValue)
                _gateway.SetStatus(status.Value);

            if (activity.HasValue)
                _gateway.SetActivity(activity.Value);

            return _gateway.SendPresenceAsync();
        }
    }
}