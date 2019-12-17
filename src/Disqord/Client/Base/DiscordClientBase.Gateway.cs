using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Logging;
using Disqord.Rest;

namespace Disqord
{
    public abstract partial class DiscordClientBase : IRestDiscordClient, IAsyncDisposable
    {
        private string _gatewayUrl;

        public abstract Task RunAsync(CancellationToken cancellationToken = default);

        public abstract Task StopAsync();

        public abstract Task SetPresenceAsync(UserStatus status);

        public abstract Task SetPresenceAsync(LocalActivity activity);

        public abstract Task SetPresenceAsync(UserStatus status, LocalActivity activity);

        internal async Task<string> GetGatewayAsync(bool isNewSession)
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
    }
}
