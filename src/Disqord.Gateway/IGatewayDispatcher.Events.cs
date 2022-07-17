using Qommon.Events;

namespace Disqord.Gateway;

public partial interface IGatewayDispatcher
{
    AsynchronousEvent<ReadyEventArgs> ReadyEvent { get; }

    AsynchronousEvent<ApplicationCommandPermissionsUpdatedEventArgs> ApplicationCommandPermissionsUpdatedEvent { get; }

    AsynchronousEvent<AutoModerationRuleCreatedEventArgs> AutoModerationRuleCreatedEvent { get; }

    AsynchronousEvent<AutoModerationRuleUpdatedEventArgs> AutoModerationRuleUpdatedEvent { get; }

    AsynchronousEvent<AutoModerationRuleDeletedEventArgs> AutoModerationRuleDeletedEvent { get; }

    AsynchronousEvent<AutoModerationActionExecutedEventArgs> AutoModerationActionExecutedEvent { get; }

    AsynchronousEvent<ChannelCreatedEventArgs> ChannelCreatedEvent { get; }

    AsynchronousEvent<ChannelUpdatedEventArgs> ChannelUpdatedEvent { get; }

    AsynchronousEvent<ChannelDeletedEventArgs> ChannelDeletedEvent { get; }

    AsynchronousEvent<ThreadCreatedEventArgs> ThreadCreatedEvent { get; }

    AsynchronousEvent<ThreadUpdatedEventArgs> ThreadUpdatedEvent { get; }

    AsynchronousEvent<ThreadDeletedEventArgs> ThreadDeletedEvent { get; }

    AsynchronousEvent<ThreadsSynchronizedEventArgs> ThreadsSynchronizedEvent { get; }

    AsynchronousEvent<ThreadMembersUpdatedEventArgs> ThreadMembersUpdatedEvent { get; }

    AsynchronousEvent<ChannelPinsUpdatedEventArgs> ChannelPinsUpdatedEvent { get; }

    AsynchronousEvent<GuildAvailableEventArgs> GuildAvailableEvent { get; }

    AsynchronousEvent<JoinedGuildEventArgs> JoinedGuildEvent { get; }

    AsynchronousEvent<GuildUnavailableEventArgs> GuildUnavailableEvent { get; }

    AsynchronousEvent<GuildUpdatedEventArgs> GuildUpdatedEvent { get; }

    AsynchronousEvent<LeftGuildEventArgs> LeftGuildEvent { get; }

    AsynchronousEvent<BanCreatedEventArgs> BanCreatedEvent { get; }

    AsynchronousEvent<BanDeletedEventArgs> BanDeletedEvent { get; }

    AsynchronousEvent<EmojisUpdatedEventArgs> EmojisUpdatedEvent { get; }

    AsynchronousEvent<StickersUpdatedEventArgs> StickersUpdatedEvent { get; }

    AsynchronousEvent<IntegrationsUpdatedEventArgs> IntegrationsUpdatedEvent { get; }

    AsynchronousEvent<MemberJoinedEventArgs> MemberJoinedEvent { get; }

    AsynchronousEvent<MemberUpdatedEventArgs> MemberUpdatedEvent { get; }

    AsynchronousEvent<MemberLeftEventArgs> MemberLeftEvent { get; }

    AsynchronousEvent<RoleCreatedEventArgs> RoleCreatedEvent { get; }

    AsynchronousEvent<RoleUpdatedEventArgs> RoleUpdatedEvent { get; }

    AsynchronousEvent<RoleDeletedEventArgs> RoleDeletedEvent { get; }

    AsynchronousEvent<GuildEventCreatedEventArgs> GuildEventCreatedEvent { get; }

    AsynchronousEvent<GuildEventUpdatedEventArgs> GuildEventUpdatedEvent { get; }

    AsynchronousEvent<GuildEventDeletedEventArgs> GuildEventDeletedEvent { get; }

    AsynchronousEvent<GuildEventMemberAddedEventArgs> GuildEventMemberAddedEvent { get; }

    AsynchronousEvent<GuildEventMemberRemovedEventArgs> GuildEventMemberRemovedEvent { get; }

    AsynchronousEvent<IntegrationCreatedEventArgs> IntegrationCreatedEvent { get; }

    AsynchronousEvent<IntegrationUpdatedEventArgs> IntegrationUpdatedEvent { get; }

    AsynchronousEvent<IntegrationDeletedEventArgs> IntegrationDeletedEvent { get; }

    AsynchronousEvent<InteractionReceivedEventArgs> InteractionReceivedEvent { get; }

    AsynchronousEvent<InviteCreatedEventArgs> InviteCreatedEvent { get; }

    AsynchronousEvent<InviteDeletedEventArgs> InviteDeletedEvent { get; }

    AsynchronousEvent<MessageReceivedEventArgs> MessageReceivedEvent { get; }

    AsynchronousEvent<MessageUpdatedEventArgs> MessageUpdatedEvent { get; }

    AsynchronousEvent<MessageDeletedEventArgs> MessageDeletedEvent { get; }

    AsynchronousEvent<MessagesDeletedEventArgs> MessagesDeletedEvent { get; }

    AsynchronousEvent<ReactionAddedEventArgs> ReactionAddedEvent { get; }

    AsynchronousEvent<ReactionRemovedEventArgs> ReactionRemovedEvent { get; }

    AsynchronousEvent<ReactionsClearedEventArgs> ReactionsClearedEvent { get; }

    AsynchronousEvent<PresenceUpdatedEventArgs> PresenceUpdatedEvent { get; }

    AsynchronousEvent<StageCreatedEventArgs> StageCreatedEvent { get; }

    AsynchronousEvent<StageUpdatedEventArgs> StageUpdatedEvent { get; }

    AsynchronousEvent<StageDeletedEventArgs> StageDeletedEvent { get; }

    AsynchronousEvent<TypingStartedEventArgs> TypingStartedEvent { get; }

    AsynchronousEvent<CurrentUserUpdatedEventArgs> CurrentUserUpdatedEvent { get; }

    AsynchronousEvent<VoiceStateUpdatedEventArgs> VoiceStateUpdatedEvent { get; }

    AsynchronousEvent<VoiceServerUpdatedEventArgs> VoiceServerUpdatedEvent { get; }

    AsynchronousEvent<WebhooksUpdatedEventArgs> WebhooksUpdatedEvent { get; }
}