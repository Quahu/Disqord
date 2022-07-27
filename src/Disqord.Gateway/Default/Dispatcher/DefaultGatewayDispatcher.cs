using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Disqord.Gateway.Default.Dispatcher;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Qommon.Binding;
using Qommon.Collections.Synchronized;

namespace Disqord.Gateway.Default;

public partial class DefaultGatewayDispatcher : IGatewayDispatcher
{
    public ILogger Logger { get; }

    public IGatewayClient Client => _binder.Value;

    public ICurrentUser? CurrentUser
    {
        get
        {
            var handler = _handlers["READY"];
            if (handler is InterceptingDispatchHandler<ReadyJsonModel, ReadyEventArgs> interceptingHandler)
            {
                // Accounts for the intercepted handler in the sharder.
                handler = interceptingHandler.UnderlyingDispatchHandler;
            }

            return (handler as ReadyDispatchHandler)?.CurrentUser;
        }
    }

    public ReadyEventDelayMode ReadyEventDelayMode { get; }

    public DispatchHandler this[string name]
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

    private readonly ISynchronizedDictionary<string, DispatchHandler> _handlers;
    private readonly Binder<IGatewayClient> _binder;

    public DefaultGatewayDispatcher(
        IOptions<DefaultGatewayDispatcherConfiguration> options,
        ILogger<DefaultGatewayDispatcher> logger)
    {
        var configuration = options.Value;
        ReadyEventDelayMode = configuration.ReadyEventDelayMode;

        Logger = logger;

        _handlers = new SynchronizedDictionary<string, DispatchHandler>
        {
            [GatewayDispatchNames.Ready] = new ReadyDispatchHandler(),
            [GatewayDispatchNames.Resumed] = new ResumedDispatchHandler(),

            [GatewayDispatchNames.ApplicationCommandPermissionsUpdate] = new ApplicationCommandPermissionsUpdateDispatchHandler(),

            [GatewayDispatchNames.AutoModerationRuleCreate] = new AutoModerationRuleCreateDispatchHandler(),
            [GatewayDispatchNames.AutoModerationRuleUpdate] = new AutoModerationRuleUpdateDispatchHandler(),
            [GatewayDispatchNames.AutoModerationRuleDelete] = new AutoModerationRuleDeleteDispatchHandler(),
            [GatewayDispatchNames.AutoModerationActionExecution] = new AutoModerationActionExecutionDispatchHandler(),

            [GatewayDispatchNames.ChannelCreate] = new ChannelCreateDispatchHandler(),
            [GatewayDispatchNames.ChannelUpdate] = new ChannelUpdateDispatchHandler(),
            [GatewayDispatchNames.ChannelDelete] = new ChannelDeleteDispatchHandler(),

            [GatewayDispatchNames.ThreadCreate] = new ThreadCreateDispatchHandler(),
            [GatewayDispatchNames.ThreadUpdate] = new ThreadUpdateDispatchHandler(),
            [GatewayDispatchNames.ThreadDelete] = new ThreadDeleteDispatchHandler(),
            [GatewayDispatchNames.ThreadListSync] = new ThreadListSyncDispatchHandler(),
            [GatewayDispatchNames.ThreadMembersUpdate] = new ThreadMembersUpdateDispatchHandler(),

            [GatewayDispatchNames.ChannelPinsUpdate] = new ChannelPinsUpdateDispatchHandler(),

            [GatewayDispatchNames.GuildCreate] = new GuildCreateDispatchHandler(),
            [GatewayDispatchNames.GuildUpdate] = new GuildUpdateDispatchHandler(),
            [GatewayDispatchNames.GuildDelete] = new GuildDeleteDispatchHandler(),

            [GatewayDispatchNames.GuildBanAdd] = new GuildBanAddDispatchHandler(),
            [GatewayDispatchNames.GuildBanRemove] = new GuildBanRemoveDispatchHandler(),

            [GatewayDispatchNames.GuildEmojisUpdate] = new GuildEmojisUpdateDispatchHandler(),

            [GatewayDispatchNames.GuildStickersUpdate] = new GuildStickersUpdateDispatchHandler(),

            [GatewayDispatchNames.GuildIntegrationsUpdate] = new GuildIntegrationsUpdateDispatchHandler(),

            [GatewayDispatchNames.GuildMemberAdd] = new GuildMemberAddDispatchHandler(),
            [GatewayDispatchNames.GuildMemberUpdate] = new GuildMemberUpdateDispatchHandler(),
            [GatewayDispatchNames.GuildMemberRemove] = new GuildMemberRemoveDispatchHandler(),

            [GatewayDispatchNames.GuildMembersChunk] = new GuildMembersChunkDispatchHandler(),

            [GatewayDispatchNames.GuildRoleCreate] = new GuildRoleCreateDispatchHandler(),
            [GatewayDispatchNames.GuildRoleUpdate] = new GuildRoleUpdateDispatchHandler(),
            [GatewayDispatchNames.GuildRoleDelete] = new GuildRoleDeleteDispatchHandler(),

            [GatewayDispatchNames.GuildScheduledEventCreate] = new GuildScheduledEventCreateDispatchHandler(),
            [GatewayDispatchNames.GuildScheduledEventUpdate] = new GuildScheduledEventUpdateDispatchHandler(),
            [GatewayDispatchNames.GuildScheduledEventDelete] = new GuildScheduledEventDeleteDispatchHandler(),
            [GatewayDispatchNames.GuildScheduledEventUserAdd] = new GuildScheduledEventUserAddDispatchHandler(),
            [GatewayDispatchNames.GuildScheduledEventUserRemove] = new GuildScheduledEventUserRemoveDispatchHandler(),

            [GatewayDispatchNames.IntegrationCreate] = new IntegrationCreateDispatchHandler(),
            [GatewayDispatchNames.IntegrationUpdate] = new IntegrationUpdateDispatchHandler(),
            [GatewayDispatchNames.IntegrationDelete] = new IntegrationDeleteDispatchHandler(),

            [GatewayDispatchNames.InteractionCreate] = new InteractionCreateDispatchHandler(),

            [GatewayDispatchNames.InviteCreate] = new InviteCreateDispatchHandler(),
            [GatewayDispatchNames.InviteDelete] = new InviteDeleteDispatchHandler(),

            [GatewayDispatchNames.MessageCreate] = new MessageCreateDispatchHandler(),
            [GatewayDispatchNames.MessageUpdate] = new MessageUpdateDispatchHandler(),
            [GatewayDispatchNames.MessageDelete] = new MessageDeleteDispatchHandler(),
            [GatewayDispatchNames.MessageDeleteBulk] = new MessageDeleteBulkDispatchHandler(),

            [GatewayDispatchNames.MessageReactionAdd] = new MessageReactionAddDispatchHandler(),
            [GatewayDispatchNames.MessageReactionRemove] = new MessageReactionRemoveDispatchHandler(),
            [GatewayDispatchNames.MessageReactionRemoveAll] = new MessageReactionRemoveAllDispatchHandler(),
            [GatewayDispatchNames.MessageReactionRemoveEmoji] = new MessageReactionRemoveEmojiDispatchHandler(),

            [GatewayDispatchNames.PresenceUpdate] = new PresenceUpdateDispatchHandler(),

            [GatewayDispatchNames.StageInstanceCreate] = new StageCreateDispatchHandler(),
            [GatewayDispatchNames.StageInstanceUpdate] = new StageUpdateDispatchHandler(),
            [GatewayDispatchNames.StageInstanceDelete] = new StageDeleteDispatchHandler(),

            [GatewayDispatchNames.TypingStart] = new TypingStartDispatchHandler(),

            [GatewayDispatchNames.UserUpdate] = new UserUpdateDispatchHandler(),

            [GatewayDispatchNames.VoiceStateUpdate] = new VoiceStateUpdateDispatchHandler(),

            [GatewayDispatchNames.VoiceServerUpdate] = new VoiceServerUpdateDispatchHandler(),

            [GatewayDispatchNames.WebhooksUpdate] = new WebhooksUpdateDispatchHandler()
        };

        _binder = new Binder<IGatewayClient>(this, allowRebinding: true);
    }

    public void Bind(IGatewayClient value)
    {
        var isRebind = _binder.IsBound;
        _binder.Bind(value);

        if (!isRebind)
        {
            value.ApiClient.DispatchReceivedEvent.Hook(HandleDispatchAsync);

            // The binding here is used so handler code knows when the handlers collection is populated.
            // E.g. GUILD_CREATE and GUILD_DELETE will notify READY so that it can delay the actual event invocation.
            foreach (var handler in _handlers.Values)
                handler.Bind(this);
        }
    }

    public async ValueTask HandleDispatchAsync(object? sender, GatewayDispatchReceivedEventArgs e)
    {
        if (sender is not IShard shard)
            throw new ArgumentException("The sender is expected to be an IGateway instance.", nameof(sender));

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

    private void HandleUnknownDispatch(IShard shard, GatewayDispatchReceivedEventArgs e)
    {
        if (!_unknownDispatches.Add(e.Name))
            return;

        shard.Logger.LogWarning(_loggedUnknownWarning
                ? "Received an unknown dispatch {0}.\n{1}"
                : "Received an unknown dispatch {0}. This message will only appear once for each unknown dispatch.\n{1}",
            e.Name, e.Data.ToString());

        if (!_loggedUnknownWarning)
            _loggedUnknownWarning = true;
    }
}
