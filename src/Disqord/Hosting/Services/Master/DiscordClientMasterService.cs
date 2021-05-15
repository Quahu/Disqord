using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Disqord.Hosting
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public partial class DiscordClientMasterService : IHostedService
    {
        public ILogger<DiscordClientMasterService> Logger { get; }

        public DiscordClientBase Client { get; }

        public DiscordClientMasterService(
            ILogger<DiscordClientMasterService> logger,
            DiscordClientBase client,
            IEnumerable<DiscordClientService> services)
            : this(client, services)
        {
            Logger = logger;
        }

        internal static bool IsOverridden(DiscordClientService service, string name, params Type[] types)
        {
            var serviceType = service.GetType();
            var method = serviceType.GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic, null, types, null);
            return method != null && method.DeclaringType != null && method.DeclaringType != method.GetBaseDefinition().DeclaringType;
        }

        private async ValueTask ExecuteAsync<TEventArgs>(Func<DiscordClientService, TEventArgs, ValueTask> factory,
            DiscordClientService service, TEventArgs e)
        {
            try
            {
                await factory(service, e).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An exception occurred while executing {0}.{1}().", service.GetType().Name, "On" + e.GetType().Name.Replace("EventArgs", ""));
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}
