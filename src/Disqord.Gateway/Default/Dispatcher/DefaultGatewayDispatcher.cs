using System;
using System.Threading.Tasks;
using Disqord.Collections.Synchronized;
using Disqord.Gateway.Api;
using Disqord.Gateway.Default.Dispatcher;
using Disqord.Models;
using Disqord.Serialization.Json.Default;
using Disqord.Utilities.Binding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disqord.Gateway.Default
{
    public partial class DefaultGatewayDispatcher : IGatewayDispatcher
    {
        public ILogger Logger { get; }

        public IGatewayClient Client => _binder.Value;

        public Handler this[string name]
        {
            get => _handlers[name];
            set
            {
                _handlers[name] = value;
                value.Bind(this);
            }
        }

        private readonly SynchronizedDictionary<string, Handler> _handlers;
        private readonly Binder<IGatewayClient> _binder;

        public DefaultGatewayDispatcher(
            IOptions<DefaultGatewayDispatcherConfiguration> options,
            ILogger<DefaultGatewayDispatcher> logger)
        {
            Logger = logger;

            _handlers = new SynchronizedDictionary<string, Handler>
            {
                ["READY"] = new ReadyHandler(),
                ["RESUMED"] = new ResumedHandler(),

                ["CHANNEL_CREATE"] = new ChannelCreateHandler(),
                ["CHANNEL_UPDATE"] = new ChannelUpdateHandler(),
                ["CHANNEL_DELETE"] = new ChannelDeleteHandler(),

                ["CHANNEL_PINS_UPDATE"] = new ChannelPinsUpdateHandler(),

                ["GUILD_CREATE"] = new GuildCreateHandler(),
                ["GUILD_DELETE"] = new GuildDeleteHandler(),

                ["GUILD_BAN_ADD"] = new GuildBanAddHandler(),
                ["GUILD_BAN_REMOVE"] = new GuildBanRemoveHandler(),

                ["MESSAGE_CREATE"] = new MessageCreateHandler(),
                ["MESSAGE_UPDATE"] = new MessageUpdateHandler(),
                ["MESSAGE_DELETE"] = new MessageDeleteHandler(),

                ["TYPING_START"] = new TypingStartHandler(),

                ["VOICE_STATE_UPDATE"] = new VoiceStateUpdateHandler(),

                ["VOICE_SERVER_UPDATE"] = new VoiceServerUpdateHandler(),
            };

            // The binding here is used so handler code knows when the handlers collection is populated.
            // E.g. GUILD_CREATE and GUILD_DELETE then notify READY so it can delay the actual event invocation.
            foreach (var handler in _handlers.Values)
                handler.Bind(this);

            _binder = new Binder<IGatewayClient>(this, allowRebinding: true);
        }

        public void Bind(IGatewayClient value)
        {
            _binder.Bind(value);
        }

        private bool _loggedUnknownWarning = false;
        private readonly SynchronizedHashSet<string> _unknownDispatches = new();

        public async Task HandleDispatchAsync(object sender, GatewayDispatchReceivedEventArgs e)
        {
            if (!_handlers.TryGetValue(e.Name, out var handler))
            {
                await UnknownDispatchAsync(e).ConfigureAwait(false);
                return;
            }

            try
            {
                var task = handler.HandleDispatchAsync(e.Data);
                if (task == null)
                    Logger.LogError("The handler {0} returned a null handle task.", handler.GetType());

                await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An exception occurred while handling dispatch {0}.", e.Name);
            }
        }

        private Task UnknownDispatchAsync(GatewayDispatchReceivedEventArgs e)
        {
            if (!_unknownDispatches.Add(e.Name))
                return Task.CompletedTask;

            Logger.LogWarning(_loggedUnknownWarning
                ? "Received an unknown dispatch {0}. Payload:\n{1}"
                : "Received an unknown dispatch {0}. This message will only appear once for each unknown dispatch. Payload:\n{1}",
                e.Name, (e.Data as DefaultJsonToken)?.ToIndentedString() ?? e.Data.ToString());

            if (!_loggedUnknownWarning)
                _loggedUnknownWarning = true;

            return Task.CompletedTask;
        }

        public async Task<IUser> GetSharedOrTransientUserAsync(UserJsonModel model)
        {
            if (Client.CacheProvider.TryGetUsers(out var cache))
            {
                if (cache.TryGetValue(model.Id, out var user))
                    return user;
            }

            return new TransientUser(Client, model);
        }
    }
}
