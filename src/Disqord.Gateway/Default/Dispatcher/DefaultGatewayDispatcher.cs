using System;
using System.Threading.Tasks;
using Qommon.Collections.Synchronized;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Disqord.Gateway.Default.Dispatcher;
using Qommon.Binding;
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

        private readonly ISynchronizedDictionary<string, Handler> _handlers;
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
                [GatewayDispatchNames.Ready] = new ReadyHandler(),
                [GatewayDispatchNames.Resumed] = new ResumedHandler(),

                [GatewayDispatchNames.ChannelCreate] = new ChannelCreateHandler(),
                [GatewayDispatchNames.ChannelUpdate] = new ChannelUpdateHandler(),
                [GatewayDispatchNames.ChannelDelete] = new ChannelDeleteHandler(),

                [GatewayDispatchNames.ThreadCreate] = new ThreadCreateHandler(),
                [GatewayDispatchNames.ThreadUpdate] = new ThreadUpdateHandler(),
                [GatewayDispatchNames.ThreadDelete] = new ThreadDeleteHandler(),
                [GatewayDispatchNames.ThreadListSync] = new ThreadListSyncHandler(),
                [GatewayDispatchNames.ThreadMembersUpdate] = new ThreadMembersUpdateHandler(),

                [GatewayDispatchNames.ChannelPinsUpdate] = new ChannelPinsUpdateHandler(),

                [GatewayDispatchNames.GuildCreate] = new GuildCreateHandler(),
                [GatewayDispatchNames.GuildUpdate] = new GuildUpdateHandler(),
                [GatewayDispatchNames.GuildDelete] = new GuildDeleteHandler(),

                [GatewayDispatchNames.GuildBanAdd] = new GuildBanAddHandler(),
                [GatewayDispatchNames.GuildBanRemove] = new GuildBanRemoveHandler(),

                [GatewayDispatchNames.GuildEmojisUpdate] = new GuildEmojisUpdateHandler(),

                [GatewayDispatchNames.GuildStickersUpdate] = new GuildStickersUpdateHandler(),

                [GatewayDispatchNames.GuildIntegrationsUpdate] = new GuildIntegrationsUpdateHandler(),

                [GatewayDispatchNames.GuildMemberAdd] = new GuildMemberAddHandler(),
                [GatewayDispatchNames.GuildMemberUpdate] = new GuildMemberUpdateHandler(),
                [GatewayDispatchNames.GuildMemberRemove] = new GuildMemberRemoveHandler(),

                [GatewayDispatchNames.GuildMembersChunk] = new GuildMembersChunkHandler(),

                [GatewayDispatchNames.GuildRoleCreate] = new GuildRoleCreateHandler(),
                [GatewayDispatchNames.GuildRoleUpdate] = new GuildRoleUpdateHandler(),
                [GatewayDispatchNames.GuildRoleDelete] = new GuildRoleDeleteHandler(),

                [GatewayDispatchNames.GuildScheduledEventCreate] = new GuildScheduledEventCreateHandler(),
                [GatewayDispatchNames.GuildScheduledEventUpdate] = new GuildScheduledEventUpdateHandler(),
                [GatewayDispatchNames.GuildScheduledEventDelete] = new GuildScheduledEventDeleteHandler(),
                [GatewayDispatchNames.GuildScheduledEventUserAdd] = new GuildScheduledEventUserAddHandler(),
                [GatewayDispatchNames.GuildScheduledEventUserRemove] = new GuildScheduledEventUserRemoveHandler(),

                [GatewayDispatchNames.IntegrationCreate] = new IntegrationCreateHandler(),
                [GatewayDispatchNames.IntegrationUpdate] = new IntegrationUpdateHandler(),
                [GatewayDispatchNames.IntegrationDelete] = new IntegrationDeleteHandler(),

                [GatewayDispatchNames.InteractionCreate] = new InteractionCreateHandler(),

                [GatewayDispatchNames.InviteCreate] = new InviteCreateHandler(),
                [GatewayDispatchNames.InviteDelete] = new InviteDeleteHandler(),

                [GatewayDispatchNames.MessageCreate] = new MessageCreateHandler(),
                [GatewayDispatchNames.MessageUpdate] = new MessageUpdateHandler(),
                [GatewayDispatchNames.MessageDelete] = new MessageDeleteHandler(),
                [GatewayDispatchNames.MessageDeleteBulk] = new MessageDeleteBulkHandler(),

                [GatewayDispatchNames.MessageReactionAdd] = new MessageReactionAddHandler(),
                [GatewayDispatchNames.MessageReactionRemove] = new MessageReactionRemoveHandler(),
                [GatewayDispatchNames.MessageReactionRemoveAll] = new MessageReactionRemoveAllHandler(),
                [GatewayDispatchNames.MessageReactionRemoveEmoji] = new MessageReactionRemoveEmojiHandler(),

                [GatewayDispatchNames.PresenceUpdate] = new PresenceUpdateHandler(),

                [GatewayDispatchNames.StageInstanceCreate] = new StageCreateHandler(),
                [GatewayDispatchNames.StageInstanceUpdate] = new StageUpdateHandler(),
                [GatewayDispatchNames.StageInstanceDelete] = new StageDeleteHandler(),

                [GatewayDispatchNames.TypingStart] = new TypingStartHandler(),

                [GatewayDispatchNames.UserUpdate] = new UserUpdateHandler(),

                [GatewayDispatchNames.VoiceStateUpdate] = new VoiceStateUpdateHandler(),

                [GatewayDispatchNames.VoiceServerUpdate] = new VoiceServerUpdateHandler(),

                [GatewayDispatchNames.WebhooksUpdate] = new WebhooksUpdateHandler()
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
                // E.g. GUILD_CREATE and GUILD_DELETE will notify READY so that it can delay the actual event invocation.
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
                Logger.LogError(ex, "An exception occurred while handling dispatch {0}.\n{1}", e.Name, e.Data?.ToString());
            }
        }

        private void HandleUnknownDispatch(IGatewayApiClient shard, GatewayDispatchReceivedEventArgs e)
        {
            if (!_unknownDispatches.Add(e.Name))
                return;

            shard.Logger.LogWarning(_loggedUnknownWarning
                    ? "Received an unknown dispatch {0}.\n{1}"
                    : "Received an unknown dispatch {0}. This message will only appear once for each unknown dispatch.\n{1}",
                e.Name, e.Data?.ToString());

            if (!_loggedUnknownWarning)
                _loggedUnknownWarning = true;
        }
    }
}
