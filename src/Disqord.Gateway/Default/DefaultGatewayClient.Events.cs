using Qommon.Events;

namespace Disqord.Gateway.Default;

public partial class DefaultGatewayClient
{
    /// <inheritdoc/>
    public event AsynchronousEventHandler<ReadyEventArgs> Ready
    {
        add => Dispatcher.ReadyEvent.Hook(value);
        remove => Dispatcher.ReadyEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<ApplicationCommandPermissionsUpdatedEventArgs> ApplicationCommandPermissionsUpdated
    {
        add => Dispatcher.ApplicationCommandPermissionsUpdatedEvent.Hook(value);
        remove => Dispatcher.ApplicationCommandPermissionsUpdatedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<AutoModerationRuleCreatedEventArgs> AutoModerationRuleCreated
    {
        add => Dispatcher.AutoModerationRuleCreatedEvent.Hook(value);
        remove => Dispatcher.AutoModerationRuleCreatedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<AutoModerationRuleUpdatedEventArgs> AutoModerationRuleUpdated
    {
        add => Dispatcher.AutoModerationRuleUpdatedEvent.Hook(value);
        remove => Dispatcher.AutoModerationRuleUpdatedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<AutoModerationRuleDeletedEventArgs> AutoModerationRuleDeleted
    {
        add => Dispatcher.AutoModerationRuleDeletedEvent.Hook(value);
        remove => Dispatcher.AutoModerationRuleDeletedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<AutoModerationActionExecutedEventArgs> AutoModerationActionExecuted
    {
        add => Dispatcher.AutoModerationActionExecutedEvent.Hook(value);
        remove => Dispatcher.AutoModerationActionExecutedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<ChannelCreatedEventArgs> ChannelCreated
    {
        add => Dispatcher.ChannelCreatedEvent.Hook(value);
        remove => Dispatcher.ChannelCreatedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<ChannelUpdatedEventArgs> ChannelUpdated
    {
        add => Dispatcher.ChannelUpdatedEvent.Hook(value);
        remove => Dispatcher.ChannelUpdatedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<ChannelDeletedEventArgs> ChannelDeleted
    {
        add => Dispatcher.ChannelDeletedEvent.Hook(value);
        remove => Dispatcher.ChannelDeletedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<ThreadCreatedEventArgs> ThreadCreated
    {
        add => Dispatcher.ThreadCreatedEvent.Hook(value);
        remove => Dispatcher.ThreadCreatedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<ThreadUpdatedEventArgs> ThreadUpdated
    {
        add => Dispatcher.ThreadUpdatedEvent.Hook(value);
        remove => Dispatcher.ThreadUpdatedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<ThreadDeletedEventArgs> ThreadDeleted
    {
        add => Dispatcher.ThreadDeletedEvent.Hook(value);
        remove => Dispatcher.ThreadDeletedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<ThreadsSynchronizedEventArgs> ThreadsSynchronized
    {
        add => Dispatcher.ThreadsSynchronizedEvent.Hook(value);
        remove => Dispatcher.ThreadsSynchronizedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<ThreadMembersUpdatedEventArgs> ThreadMembersUpdated
    {
        add => Dispatcher.ThreadMembersUpdatedEvent.Hook(value);
        remove => Dispatcher.ThreadMembersUpdatedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<ChannelPinsUpdatedEventArgs> ChannelPinsUpdated
    {
        add => Dispatcher.ChannelPinsUpdatedEvent.Hook(value);
        remove => Dispatcher.ChannelPinsUpdatedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<GuildAvailableEventArgs> GuildAvailable
    {
        add => Dispatcher.GuildAvailableEvent.Hook(value);
        remove => Dispatcher.GuildAvailableEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<JoinedGuildEventArgs> JoinedGuild
    {
        add => Dispatcher.JoinedGuildEvent.Hook(value);
        remove => Dispatcher.JoinedGuildEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<GuildUpdatedEventArgs> GuildUpdated
    {
        add => Dispatcher.GuildUpdatedEvent.Hook(value);
        remove => Dispatcher.GuildUpdatedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<GuildUnavailableEventArgs> GuildUnavailable
    {
        add => Dispatcher.GuildUnavailableEvent.Hook(value);
        remove => Dispatcher.GuildUnavailableEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<LeftGuildEventArgs> LeftGuild
    {
        add => Dispatcher.LeftGuildEvent.Hook(value);
        remove => Dispatcher.LeftGuildEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<BanCreatedEventArgs> BanCreated
    {
        add => Dispatcher.BanCreatedEvent.Hook(value);
        remove => Dispatcher.BanCreatedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<BanDeletedEventArgs> BanDeleted
    {
        add => Dispatcher.BanDeletedEvent.Hook(value);
        remove => Dispatcher.BanDeletedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<EmojisUpdatedEventArgs> EmojisUpdated
    {
        add => Dispatcher.EmojisUpdatedEvent.Hook(value);
        remove => Dispatcher.EmojisUpdatedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<StickersUpdatedEventArgs> StickersUpdated
    {
        add => Dispatcher.StickersUpdatedEvent.Hook(value);
        remove => Dispatcher.StickersUpdatedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<IntegrationsUpdatedEventArgs> IntegrationsUpdated
    {
        add => Dispatcher.IntegrationsUpdatedEvent.Hook(value);
        remove => Dispatcher.IntegrationsUpdatedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<MemberJoinedEventArgs> MemberJoined
    {
        add => Dispatcher.MemberJoinedEvent.Hook(value);
        remove => Dispatcher.MemberJoinedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<MemberUpdatedEventArgs> MemberUpdated
    {
        add => Dispatcher.MemberUpdatedEvent.Hook(value);
        remove => Dispatcher.MemberUpdatedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<MemberLeftEventArgs> MemberLeft
    {
        add => Dispatcher.MemberLeftEvent.Hook(value);
        remove => Dispatcher.MemberLeftEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<RoleCreatedEventArgs> RoleCreated
    {
        add => Dispatcher.RoleCreatedEvent.Hook(value);
        remove => Dispatcher.RoleCreatedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<RoleUpdatedEventArgs> RoleUpdated
    {
        add => Dispatcher.RoleUpdatedEvent.Hook(value);
        remove => Dispatcher.RoleUpdatedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<RoleDeletedEventArgs> RoleDeleted
    {
        add => Dispatcher.RoleDeletedEvent.Hook(value);
        remove => Dispatcher.RoleDeletedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<GuildEventCreatedEventArgs> GuildEventCreated
    {
        add => Dispatcher.GuildEventCreatedEvent.Hook(value);
        remove => Dispatcher.GuildEventCreatedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<GuildEventUpdatedEventArgs> GuildEventUpdated
    {
        add => Dispatcher.GuildEventUpdatedEvent.Hook(value);
        remove => Dispatcher.GuildEventUpdatedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<GuildEventDeletedEventArgs> GuildEventDeleted
    {
        add => Dispatcher.GuildEventDeletedEvent.Hook(value);
        remove => Dispatcher.GuildEventDeletedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<GuildEventMemberAddedEventArgs> GuildEventMemberAdded
    {
        add => Dispatcher.GuildEventMemberAddedEvent.Hook(value);
        remove => Dispatcher.GuildEventMemberAddedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<GuildEventMemberRemovedEventArgs> GuildEventMemberRemoved
    {
        add => Dispatcher.GuildEventMemberRemovedEvent.Hook(value);
        remove => Dispatcher.GuildEventMemberRemovedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<IntegrationCreatedEventArgs> IntegrationCreated
    {
        add => Dispatcher.IntegrationCreatedEvent.Hook(value);
        remove => Dispatcher.IntegrationCreatedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<IntegrationUpdatedEventArgs> IntegrationUpdated
    {
        add => Dispatcher.IntegrationUpdatedEvent.Hook(value);
        remove => Dispatcher.IntegrationUpdatedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<IntegrationDeletedEventArgs> IntegrationDeleted
    {
        add => Dispatcher.IntegrationDeletedEvent.Hook(value);
        remove => Dispatcher.IntegrationDeletedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<InteractionReceivedEventArgs> InteractionReceived
    {
        add => Dispatcher.InteractionReceivedEvent.Hook(value);
        remove => Dispatcher.InteractionReceivedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<InviteCreatedEventArgs> InviteCreated
    {
        add => Dispatcher.InviteCreatedEvent.Hook(value);
        remove => Dispatcher.InviteCreatedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<InviteDeletedEventArgs> InviteDeleted
    {
        add => Dispatcher.InviteDeletedEvent.Hook(value);
        remove => Dispatcher.InviteDeletedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<MessageReceivedEventArgs> MessageReceived
    {
        add => Dispatcher.MessageReceivedEvent.Hook(value);
        remove => Dispatcher.MessageReceivedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<MessageUpdatedEventArgs> MessageUpdated
    {
        add => Dispatcher.MessageUpdatedEvent.Hook(value);
        remove => Dispatcher.MessageUpdatedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<MessageDeletedEventArgs> MessageDeleted
    {
        add => Dispatcher.MessageDeletedEvent.Hook(value);
        remove => Dispatcher.MessageDeletedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<MessagesDeletedEventArgs> MessagesDeleted
    {
        add => Dispatcher.MessagesDeletedEvent.Hook(value);
        remove => Dispatcher.MessagesDeletedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<ReactionAddedEventArgs> ReactionAdded
    {
        add => Dispatcher.ReactionAddedEvent.Hook(value);
        remove => Dispatcher.ReactionAddedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<ReactionRemovedEventArgs> ReactionRemoved
    {
        add => Dispatcher.ReactionRemovedEvent.Hook(value);
        remove => Dispatcher.ReactionRemovedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<ReactionsClearedEventArgs> ReactionsCleared
    {
        add => Dispatcher.ReactionsClearedEvent.Hook(value);
        remove => Dispatcher.ReactionsClearedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<PresenceUpdatedEventArgs> PresenceUpdated
    {
        add => Dispatcher.PresenceUpdatedEvent.Hook(value);
        remove => Dispatcher.PresenceUpdatedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<StageCreatedEventArgs> StageCreated
    {
        add => Dispatcher.StageCreatedEvent.Hook(value);
        remove => Dispatcher.StageCreatedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<StageUpdatedEventArgs> StageUpdated
    {
        add => Dispatcher.StageUpdatedEvent.Hook(value);
        remove => Dispatcher.StageUpdatedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<StageDeletedEventArgs> StageDeleted
    {
        add => Dispatcher.StageDeletedEvent.Hook(value);
        remove => Dispatcher.StageDeletedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<TypingStartedEventArgs> TypingStarted
    {
        add => Dispatcher.TypingStartedEvent.Hook(value);
        remove => Dispatcher.TypingStartedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<CurrentUserUpdatedEventArgs> CurrentUserUpdated
    {
        add => Dispatcher.CurrentUserUpdatedEvent.Hook(value);
        remove => Dispatcher.CurrentUserUpdatedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<VoiceStateUpdatedEventArgs> VoiceStateUpdated
    {
        add => Dispatcher.VoiceStateUpdatedEvent.Hook(value);
        remove => Dispatcher.VoiceStateUpdatedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<VoiceServerUpdatedEventArgs> VoiceServerUpdated
    {
        add => Dispatcher.VoiceServerUpdatedEvent.Hook(value);
        remove => Dispatcher.VoiceServerUpdatedEvent.Unhook(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<WebhooksUpdatedEventArgs> WebhooksUpdated
    {
        add => Dispatcher.WebhooksUpdatedEvent.Hook(value);
        remove => Dispatcher.WebhooksUpdatedEvent.Unhook(value);
    }
}