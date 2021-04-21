using System;
using System.Threading.Tasks;
using Disqord.Collections.Synchronized;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Disqord.Gateway.Default.Dispatcher;
using Disqord.Serialization.Json.Default;
using Disqord.Utilities.Binding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disqord.Gateway.Default
{
    public partial class DefaultGatewayDispatcher
    {
        public ILogger Logger { get; }

        public IGatewayClient Client => _binder.Value;

        public ICurrentUser CurrentUser
        {
            get
            {
                var handler = _handlers["READY"];
                if (handler is InterceptingHandler<ReadyJsonModel, ReadyEventArgs> interceptingHandler)
                {
                    // Accounts for the intercepted handler in the sharder.
                    handler = interceptingHandler.Handler;
                }

                return (handler as ReadyHandler)?.CurrentUser;
            }
        }

        public ReadyEventDelayMode ReadyEventDelayMode { get; }

        public Handler this[string name]
        {
            get => _handlers[name];
            set
            {
                _handlers[name] = value;
                value.Bind(this);
            }
        }

        private bool _loggedUnknownWarning;
        private readonly SynchronizedHashSet<string> _unknownDispatches = new();

        private readonly SynchronizedDictionary<string, Handler> _handlers;
        private readonly Binder<IGatewayClient> _binder;

        public DefaultGatewayDispatcher(
            IOptions<DefaultGatewayDispatcherConfiguration> options,
            ILogger<DefaultGatewayDispatcher> logger)
        {
            var configuration = options.Value;
            ReadyEventDelayMode = configuration.ReadyEventDelayMode;

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
                ["GUILD_UPDATE"] = new GuildUpdateHandler(),
                ["GUILD_DELETE"] = new GuildDeleteHandler(),

                ["GUILD_BAN_ADD"] = new GuildBanAddHandler(),
                ["GUILD_BAN_REMOVE"] = new GuildBanRemoveHandler(),

                ["GUILD_EMOJIS_UPDATE"] = new GuildEmojisUpdateHandler(),

                ["GUILD_MEMBER_ADD"] = new GuildMemberAddHandler(),
                ["GUILD_MEMBER_UPDATE"] = new GuildMemberUpdateHandler(),
                ["GUILD_MEMBER_REMOVE"] = new GuildMemberRemoveHandler(),

                ["GUILD_MEMBERS_CHUNK"] = new GuildMembersChunkHandler(),

                ["GUILD_ROLE_CREATE"] = new GuildRoleCreateHandler(),
                ["GUILD_ROLE_UPDATE"] = new GuildRoleUpdateHandler(),
                ["GUILD_ROLE_DELETE"] = new GuildRoleDeleteHandler(),

                ["INVITE_CREATE"] = new InviteCreateHandler(),
                ["INVITE_DELETE"] = new InviteDeleteHandler(),

                ["MESSAGE_CREATE"] = new MessageCreateHandler(),
                ["MESSAGE_UPDATE"] = new MessageUpdateHandler(),
                ["MESSAGE_DELETE"] = new MessageDeleteHandler(),
                ["MESSAGE_DELETE_BULK"] = new MessageDeleteBulkHandler(),

                ["MESSAGE_REACTION_ADD"] = new MessageReactionAddHandler(),
                ["MESSAGE_REACTION_REMOVE"] = new MessageReactionRemoveHandler(),
                ["MESSAGE_REACTION_REMOVE_ALL"] = new MessageReactionRemoveAllHandler(),
                ["MESSAGE_REACTION_REMOVE_EMOJI"] = new MessageReactionRemoveEmojiHandler(),

                ["TYPING_START"] = new TypingStartHandler(),

                ["USER_UPDATE"] = new UserUpdateHandler(),

                ["VOICE_STATE_UPDATE"] = new VoiceStateUpdateHandler(),

                ["VOICE_SERVER_UPDATE"] = new VoiceServerUpdateHandler(),

                ["WEBHOOKS_UPDATE"] = new WebhooksUpdateHandler()
            };

            _binder = new Binder<IGatewayClient>(this, allowRebinding: true);
        }

        public void Bind(IGatewayClient value)
        {
            var isRebind = _binder.IsBound;
            _binder.Bind(value);

            if (!isRebind)
            {
                // The binding here is used so handler code knows when the handlers collection is populated.
                // E.g. GUILD_CREATE and GUILD_DELETE then notify READY so it can delay the actual event invocation.
                foreach (var handler in _handlers.Values)
                    handler.Bind(this);
            }
        }

        public async ValueTask HandleDispatchAsync(object sender, GatewayDispatchReceivedEventArgs e)
        {
            if (sender is not IGatewayApiClient shard)
                throw new ArgumentException("The sender is expected to be an IGatewayApiClient.", nameof(sender));

            if (!_handlers.TryGetValue(e.Name, out var handler))
            {
                HandleUnknownDispatch(shard, e);
                return;
            }

            try
            {
                await handler.HandleDispatchAsync(shard, e.Data).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An exception occurred while handling dispatch {0}.", e.Name);
            }
        }

        private void HandleUnknownDispatch(IGatewayApiClient shard, GatewayDispatchReceivedEventArgs e)
        {
            if (!_unknownDispatches.Add(e.Name))
                return;

            shard.Logger.LogWarning(_loggedUnknownWarning
                    ? "Received an unknown dispatch {0}. Payload:\n{1}"
                    : "Received an unknown dispatch {0}. This message will only appear once for each unknown dispatch. Payload:\n{1}",
                e.Name, (e.Data as DefaultJsonToken)?.ToIndentedString() ?? e.Data.ToString());

            if (!_loggedUnknownWarning)
                _loggedUnknownWarning = true;
        }
    }
}
