using Qommon.Events;

namespace Disqord.Gateway.Default;

public partial class DefaultGatewayClient
{
    /// <inheritdoc/>
    public event AsynchronousEventHandler<ReadyEventArgs> Ready
    {
        add => Dispatcher.ReadyEvent.Add(value);
        remove => Dispatcher.ReadyEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<ApplicationCommandPermissionsUpdatedEventArgs> ApplicationCommandPermissionsUpdated
    {
        add => Dispatcher.ApplicationCommandPermissionsUpdatedEvent.Add(value);
        remove => Dispatcher.ApplicationCommandPermissionsUpdatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<AutoModerationRuleCreatedEventArgs> AutoModerationRuleCreated
    {
        add => Dispatcher.AutoModerationRuleCreatedEvent.Add(value);
        remove => Dispatcher.AutoModerationRuleCreatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<AutoModerationRuleUpdatedEventArgs> AutoModerationRuleUpdated
    {
        add => Dispatcher.AutoModerationRuleUpdatedEvent.Add(value);
        remove => Dispatcher.AutoModerationRuleUpdatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<AutoModerationRuleDeletedEventArgs> AutoModerationRuleDeleted
    {
        add => Dispatcher.AutoModerationRuleDeletedEvent.Add(value);
        remove => Dispatcher.AutoModerationRuleDeletedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<AutoModerationActionExecutedEventArgs> AutoModerationActionExecuted
    {
        add => Dispatcher.AutoModerationActionExecutedEvent.Add(value);
        remove => Dispatcher.AutoModerationActionExecutedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<ChannelCreatedEventArgs> ChannelCreated
    {
        add => Dispatcher.ChannelCreatedEvent.Add(value);
        remove => Dispatcher.ChannelCreatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<ChannelUpdatedEventArgs> ChannelUpdated
    {
        add => Dispatcher.ChannelUpdatedEvent.Add(value);
        remove => Dispatcher.ChannelUpdatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<ChannelDeletedEventArgs> ChannelDeleted
    {
        add => Dispatcher.ChannelDeletedEvent.Add(value);
        remove => Dispatcher.ChannelDeletedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<ThreadCreatedEventArgs> ThreadCreated
    {
        add => Dispatcher.ThreadCreatedEvent.Add(value);
        remove => Dispatcher.ThreadCreatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<ThreadUpdatedEventArgs> ThreadUpdated
    {
        add => Dispatcher.ThreadUpdatedEvent.Add(value);
        remove => Dispatcher.ThreadUpdatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<ThreadDeletedEventArgs> ThreadDeleted
    {
        add => Dispatcher.ThreadDeletedEvent.Add(value);
        remove => Dispatcher.ThreadDeletedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<ThreadsSynchronizedEventArgs> ThreadsSynchronized
    {
        add => Dispatcher.ThreadsSynchronizedEvent.Add(value);
        remove => Dispatcher.ThreadsSynchronizedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<ThreadMembersUpdatedEventArgs> ThreadMembersUpdated
    {
        add => Dispatcher.ThreadMembersUpdatedEvent.Add(value);
        remove => Dispatcher.ThreadMembersUpdatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<ChannelPinsUpdatedEventArgs> ChannelPinsUpdated
    {
        add => Dispatcher.ChannelPinsUpdatedEvent.Add(value);
        remove => Dispatcher.ChannelPinsUpdatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<EntitlementCreatedEventArgs> EntitlementCreated
    {
        add => Dispatcher.EntitlementCreatedEvent.Add(value);
        remove => Dispatcher.EntitlementCreatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<EntitlementUpdatedEventArgs> EntitlementUpdated
    {
        add => Dispatcher.EntitlementUpdatedEvent.Add(value);
        remove => Dispatcher.EntitlementUpdatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<EntitlementDeletedEventArgs> EntitlementDeleted
    {
        add => Dispatcher.EntitlementDeletedEvent.Add(value);
        remove => Dispatcher.EntitlementDeletedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<GuildAvailableEventArgs> GuildAvailable
    {
        add => Dispatcher.GuildAvailableEvent.Add(value);
        remove => Dispatcher.GuildAvailableEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<JoinedGuildEventArgs> JoinedGuild
    {
        add => Dispatcher.JoinedGuildEvent.Add(value);
        remove => Dispatcher.JoinedGuildEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<GuildUpdatedEventArgs> GuildUpdated
    {
        add => Dispatcher.GuildUpdatedEvent.Add(value);
        remove => Dispatcher.GuildUpdatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<GuildUnavailableEventArgs> GuildUnavailable
    {
        add => Dispatcher.GuildUnavailableEvent.Add(value);
        remove => Dispatcher.GuildUnavailableEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<LeftGuildEventArgs> LeftGuild
    {
        add => Dispatcher.LeftGuildEvent.Add(value);
        remove => Dispatcher.LeftGuildEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<AuditLogCreatedEventArgs> AuditLogCreated
    {
        add => Dispatcher.AuditLogCreatedEvent.Add(value);
        remove => Dispatcher.AuditLogCreatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<BanCreatedEventArgs> BanCreated
    {
        add => Dispatcher.BanCreatedEvent.Add(value);
        remove => Dispatcher.BanCreatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<BanDeletedEventArgs> BanDeleted
    {
        add => Dispatcher.BanDeletedEvent.Add(value);
        remove => Dispatcher.BanDeletedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<EmojisUpdatedEventArgs> EmojisUpdated
    {
        add => Dispatcher.EmojisUpdatedEvent.Add(value);
        remove => Dispatcher.EmojisUpdatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<StickersUpdatedEventArgs> StickersUpdated
    {
        add => Dispatcher.StickersUpdatedEvent.Add(value);
        remove => Dispatcher.StickersUpdatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<IntegrationsUpdatedEventArgs> IntegrationsUpdated
    {
        add => Dispatcher.IntegrationsUpdatedEvent.Add(value);
        remove => Dispatcher.IntegrationsUpdatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<MemberJoinedEventArgs> MemberJoined
    {
        add => Dispatcher.MemberJoinedEvent.Add(value);
        remove => Dispatcher.MemberJoinedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<MemberUpdatedEventArgs> MemberUpdated
    {
        add => Dispatcher.MemberUpdatedEvent.Add(value);
        remove => Dispatcher.MemberUpdatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<MemberLeftEventArgs> MemberLeft
    {
        add => Dispatcher.MemberLeftEvent.Add(value);
        remove => Dispatcher.MemberLeftEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<RoleCreatedEventArgs> RoleCreated
    {
        add => Dispatcher.RoleCreatedEvent.Add(value);
        remove => Dispatcher.RoleCreatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<RoleUpdatedEventArgs> RoleUpdated
    {
        add => Dispatcher.RoleUpdatedEvent.Add(value);
        remove => Dispatcher.RoleUpdatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<RoleDeletedEventArgs> RoleDeleted
    {
        add => Dispatcher.RoleDeletedEvent.Add(value);
        remove => Dispatcher.RoleDeletedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<GuildEventCreatedEventArgs> GuildEventCreated
    {
        add => Dispatcher.GuildEventCreatedEvent.Add(value);
        remove => Dispatcher.GuildEventCreatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<GuildEventUpdatedEventArgs> GuildEventUpdated
    {
        add => Dispatcher.GuildEventUpdatedEvent.Add(value);
        remove => Dispatcher.GuildEventUpdatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<GuildEventDeletedEventArgs> GuildEventDeleted
    {
        add => Dispatcher.GuildEventDeletedEvent.Add(value);
        remove => Dispatcher.GuildEventDeletedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<GuildEventMemberAddedEventArgs> GuildEventMemberAdded
    {
        add => Dispatcher.GuildEventMemberAddedEvent.Add(value);
        remove => Dispatcher.GuildEventMemberAddedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<GuildEventMemberRemovedEventArgs> GuildEventMemberRemoved
    {
        add => Dispatcher.GuildEventMemberRemovedEvent.Add(value);
        remove => Dispatcher.GuildEventMemberRemovedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<IntegrationCreatedEventArgs> IntegrationCreated
    {
        add => Dispatcher.IntegrationCreatedEvent.Add(value);
        remove => Dispatcher.IntegrationCreatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<IntegrationUpdatedEventArgs> IntegrationUpdated
    {
        add => Dispatcher.IntegrationUpdatedEvent.Add(value);
        remove => Dispatcher.IntegrationUpdatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<IntegrationDeletedEventArgs> IntegrationDeleted
    {
        add => Dispatcher.IntegrationDeletedEvent.Add(value);
        remove => Dispatcher.IntegrationDeletedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<InteractionReceivedEventArgs> InteractionReceived
    {
        add => Dispatcher.InteractionReceivedEvent.Add(value);
        remove => Dispatcher.InteractionReceivedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<InviteCreatedEventArgs> InviteCreated
    {
        add => Dispatcher.InviteCreatedEvent.Add(value);
        remove => Dispatcher.InviteCreatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<InviteDeletedEventArgs> InviteDeleted
    {
        add => Dispatcher.InviteDeletedEvent.Add(value);
        remove => Dispatcher.InviteDeletedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<MessageReceivedEventArgs> MessageReceived
    {
        add => Dispatcher.MessageReceivedEvent.Add(value);
        remove => Dispatcher.MessageReceivedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<MessageUpdatedEventArgs> MessageUpdated
    {
        add => Dispatcher.MessageUpdatedEvent.Add(value);
        remove => Dispatcher.MessageUpdatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<MessageDeletedEventArgs> MessageDeleted
    {
        add => Dispatcher.MessageDeletedEvent.Add(value);
        remove => Dispatcher.MessageDeletedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<MessagesDeletedEventArgs> MessagesDeleted
    {
        add => Dispatcher.MessagesDeletedEvent.Add(value);
        remove => Dispatcher.MessagesDeletedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<ReactionAddedEventArgs> ReactionAdded
    {
        add => Dispatcher.ReactionAddedEvent.Add(value);
        remove => Dispatcher.ReactionAddedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<ReactionRemovedEventArgs> ReactionRemoved
    {
        add => Dispatcher.ReactionRemovedEvent.Add(value);
        remove => Dispatcher.ReactionRemovedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<ReactionsClearedEventArgs> ReactionsCleared
    {
        add => Dispatcher.ReactionsClearedEvent.Add(value);
        remove => Dispatcher.ReactionsClearedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<PresenceUpdatedEventArgs> PresenceUpdated
    {
        add => Dispatcher.PresenceUpdatedEvent.Add(value);
        remove => Dispatcher.PresenceUpdatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<StageCreatedEventArgs> StageCreated
    {
        add => Dispatcher.StageCreatedEvent.Add(value);
        remove => Dispatcher.StageCreatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<StageUpdatedEventArgs> StageUpdated
    {
        add => Dispatcher.StageUpdatedEvent.Add(value);
        remove => Dispatcher.StageUpdatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<StageDeletedEventArgs> StageDeleted
    {
        add => Dispatcher.StageDeletedEvent.Add(value);
        remove => Dispatcher.StageDeletedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<PollVoteAddedEventArgs> PollVoteAdded
    {
        add => Dispatcher.PollVoteAddedEvent.Add(value);
        remove => Dispatcher.PollVoteAddedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<PollVoteRemovedEventArgs> PollVoteRemoved
    {
        add => Dispatcher.PollVoteRemovedEvent.Add(value);
        remove => Dispatcher.PollVoteRemovedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<TypingStartedEventArgs> TypingStarted
    {
        add => Dispatcher.TypingStartedEvent.Add(value);
        remove => Dispatcher.TypingStartedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<CurrentUserUpdatedEventArgs> CurrentUserUpdated
    {
        add => Dispatcher.CurrentUserUpdatedEvent.Add(value);
        remove => Dispatcher.CurrentUserUpdatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<VoiceStateUpdatedEventArgs> VoiceStateUpdated
    {
        add => Dispatcher.VoiceStateUpdatedEvent.Add(value);
        remove => Dispatcher.VoiceStateUpdatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<VoiceServerUpdatedEventArgs> VoiceServerUpdated
    {
        add => Dispatcher.VoiceServerUpdatedEvent.Add(value);
        remove => Dispatcher.VoiceServerUpdatedEvent.Remove(value);
    }

    /// <inheritdoc/>
    public event AsynchronousEventHandler<WebhooksUpdatedEventArgs> WebhooksUpdated
    {
        add => Dispatcher.WebhooksUpdatedEvent.Add(value);
        remove => Dispatcher.WebhooksUpdatedEvent.Remove(value);
    }
}
