using System;
using System.Threading.Tasks;
using Disqord.Logging;
using Disqord.Models;
using Disqord.Models.Dispatches;

namespace Disqord
{
    public partial class DiscordClient : DiscordClientBase
    {
        /// <summary>
        ///     Gets the latency between heartbeats.
        /// </summary>
        public TimeSpan? Latency => _gateway.Latency;

        private readonly DiscordClientGateway _gateway;
        private string _gatewayUrl;
        private bool _isDisposed;

        public override async Task ConnectAsync()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(DiscordClient));

            if (IsBot)
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
            await _gateway.ConnectAsync(_gatewayUrl).ConfigureAwait(false);
        }

        public override async Task DisconnectAsync()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(DiscordClient));

            await _gateway.DisconnectAsync().ConfigureAwait(false);
        }

        public override Task SetPresenceAsync(UserStatus status)
            => InternalSetPresenceAsync(status);

        public override Task SetPresenceAsync(LocalActivity activity)
            => InternalSetPresenceAsync(activity: activity);

        public override Task SetPresenceAsync(UserStatus status, LocalActivity activity)
            => InternalSetPresenceAsync(status, activity);

        private Task InternalSetPresenceAsync(UserStatus? status = default, in Optional<LocalActivity> activity = default)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(DiscordClient));

            if (!status.HasValue && !activity.HasValue)
                return Task.CompletedTask;

            if (status.HasValue)
                SetStatus(status.Value);

            if (activity.HasValue)
                SetActivity(activity.Value);

            return _gateway.SendAsync(new PayloadModel
            {
                Op = Opcode.StatusUpdate,
                D = new UpdateStatusModel
                {
                    Status = _gateway._status,
                    Game = _gateway._activity
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
                    _gateway._status = status;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(status));
            }
        }

        private void SetActivity(LocalActivity activity)
        {
            _gateway._activity = activity == null
                ? null
                : new ActivityModel
                {
                    Name = activity.Name,
                    Url = activity.Url,
                    Type = activity.Type
                };
        }

        // TODO
        public override ValueTask DisposeAsync()
        {
            if (_isDisposed)
                return default;

            _isDisposed = true;
            _gateway.Dispose();
            State.Reset();
            RestClient.Dispose();
            return default;
        }
    }
}