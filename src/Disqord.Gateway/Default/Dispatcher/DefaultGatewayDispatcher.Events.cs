using Qommon.Events;

namespace Disqord.Gateway.Default;

public partial class DefaultGatewayDispatcher
{
    public AsynchronousEvent<ReadyEventArgs> ReadyEvent { get; } = new();

    public AsynchronousEvent<ApplicationCommandPermissionsUpdatedEventArgs> ApplicationCommandPermissionsUpdatedEvent { get; } = new();

    public AsynchronousEvent<AutoModerationRuleCreatedEventArgs> AutoModerationRuleCreatedEvent { get; } = new();

    public AsynchronousEvent<AutoModerationRuleUpdatedEventArgs> AutoModerationRuleUpdatedEvent { get; } = new();

    public AsynchronousEvent<AutoModerationRuleDeletedEventArgs> AutoModerationRuleDeletedEvent { get; } = new();

    public AsynchronousEvent<AutoModerationActionExecutedEventArgs> AutoModerationActionExecutedEvent { get; } = new();

    public AsynchronousEvent<ChannelCreatedEventArgs> ChannelCreatedEvent { get; } = new();

    public AsynchronousEvent<ChannelUpdatedEventArgs> ChannelUpdatedEvent { get; } = new();

    public AsynchronousEvent<ChannelDeletedEventArgs> ChannelDeletedEvent { get; } = new();

    public AsynchronousEvent<ThreadCreatedEventArgs> ThreadCreatedEvent { get; } = new();

    public AsynchronousEvent<ThreadUpdatedEventArgs> ThreadUpdatedEvent { get; } = new();

    public AsynchronousEvent<ThreadDeletedEventArgs> ThreadDeletedEvent { get; } = new();

    public AsynchronousEvent<ThreadsSynchronizedEventArgs> ThreadsSynchronizedEvent { get; } = new();

    public AsynchronousEvent<ThreadMembersUpdatedEventArgs> ThreadMembersUpdatedEvent { get; } = new();

    public AsynchronousEvent<ChannelPinsUpdatedEventArgs> ChannelPinsUpdatedEvent { get; } = new();

    public AsynchronousEvent<GuildAvailableEventArgs> GuildAvailableEvent { get; } = new();

    public AsynchronousEvent<JoinedGuildEventArgs> JoinedGuildEvent { get; } = new();

    public AsynchronousEvent<GuildUpdatedEventArgs> GuildUpdatedEvent { get; } = new();

    public AsynchronousEvent<GuildUnavailableEventArgs> GuildUnavailableEvent { get; } = new();

    public AsynchronousEvent<LeftGuildEventArgs> LeftGuildEvent { get; } = new();

    public AsynchronousEvent<BanCreatedEventArgs> BanCreatedEvent { get; } = new();

    public AsynchronousEvent<BanDeletedEventArgs> BanDeletedEvent { get; } = new();

    public AsynchronousEvent<EmojisUpdatedEventArgs> EmojisUpdatedEvent { get; } = new();

    public AsynchronousEvent<StickersUpdatedEventArgs> StickersUpdatedEvent { get; } = new();

    public AsynchronousEvent<IntegrationsUpdatedEventArgs> IntegrationsUpdatedEvent { get; } = new();

    public AsynchronousEvent<MemberJoinedEventArgs> MemberJoinedEvent { get; } = new();

    public AsynchronousEvent<MemberUpdatedEventArgs> MemberUpdatedEvent { get; } = new();

    public AsynchronousEvent<MemberLeftEventArgs> MemberLeftEvent { get; } = new();

    public AsynchronousEvent<RoleCreatedEventArgs> RoleCreatedEvent { get; } = new();

    public AsynchronousEvent<RoleUpdatedEventArgs> RoleUpdatedEvent { get; } = new();

    public AsynchronousEvent<RoleDeletedEventArgs> RoleDeletedEvent { get; } = new();

    public AsynchronousEvent<GuildEventCreatedEventArgs> GuildEventCreatedEvent { get; } = new();

    public AsynchronousEvent<GuildEventUpdatedEventArgs> GuildEventUpdatedEvent { get; } = new();

    public AsynchronousEvent<GuildEventDeletedEventArgs> GuildEventDeletedEvent { get; } = new();

    public AsynchronousEvent<GuildEventMemberAddedEventArgs> GuildEventMemberAddedEvent { get; } = new();

    public AsynchronousEvent<GuildEventMemberRemovedEventArgs> GuildEventMemberRemovedEvent { get; } = new();

    public AsynchronousEvent<IntegrationCreatedEventArgs> IntegrationCreatedEvent { get; } = new();

    public AsynchronousEvent<IntegrationUpdatedEventArgs> IntegrationUpdatedEvent { get; } = new();

    public AsynchronousEvent<IntegrationDeletedEventArgs> IntegrationDeletedEvent { get; } = new();

    public AsynchronousEvent<InteractionReceivedEventArgs> InteractionReceivedEvent { get; } = new();

    public AsynchronousEvent<InviteCreatedEventArgs> InviteCreatedEvent { get; } = new();

    public AsynchronousEvent<InviteDeletedEventArgs> InviteDeletedEvent { get; } = new();

    public AsynchronousEvent<MessageReceivedEventArgs> MessageReceivedEvent { get; } = new();

    public AsynchronousEvent<MessageUpdatedEventArgs> MessageUpdatedEvent { get; } = new();

    public AsynchronousEvent<MessageDeletedEventArgs> MessageDeletedEvent { get; } = new();

    public AsynchronousEvent<MessagesDeletedEventArgs> MessagesDeletedEvent { get; } = new();

    public AsynchronousEvent<ReactionAddedEventArgs> ReactionAddedEvent { get; } = new();

    public AsynchronousEvent<ReactionRemovedEventArgs> ReactionRemovedEvent { get; } = new();

    public AsynchronousEvent<ReactionsClearedEventArgs> ReactionsClearedEvent { get; } = new();

    public AsynchronousEvent<PresenceUpdatedEventArgs> PresenceUpdatedEvent { get; } = new();

    public AsynchronousEvent<StageCreatedEventArgs> StageCreatedEvent { get; } = new();

    public AsynchronousEvent<StageUpdatedEventArgs> StageUpdatedEvent { get; } = new();

    public AsynchronousEvent<StageDeletedEventArgs> StageDeletedEvent { get; } = new();

    public AsynchronousEvent<TypingStartedEventArgs> TypingStartedEvent { get; } = new();

    public AsynchronousEvent<CurrentUserUpdatedEventArgs> CurrentUserUpdatedEvent { get; } = new();

    public AsynchronousEvent<VoiceStateUpdatedEventArgs> VoiceStateUpdatedEvent { get; } = new();

    public AsynchronousEvent<VoiceServerUpdatedEventArgs> VoiceServerUpdatedEvent { get; } = new();

    public AsynchronousEvent<WebhooksUpdatedEventArgs> WebhooksUpdatedEvent { get; } = new();
}
