using Disqord.Events;

namespace Disqord.Gateway
{
    public partial interface IGatewayDispatcher
    {
        AsynchronousEvent<ReadyEventArgs> ReadyEvent { get; }

        AsynchronousEvent<ChannelCreatedEventArgs> ChannelCreatedEvent { get; }

        AsynchronousEvent<ChannelUpdatedEventArgs> ChannelUpdatedEvent { get; }

        AsynchronousEvent<ChannelDeletedEventArgs> ChannelDeletedEvent { get; }

        AsynchronousEvent<ChannelPinsUpdatedEventArgs> ChannelPinsUpdatedEvent { get; }

        AsynchronousEvent<GuildAvailableEventArgs> GuildAvailableEvent { get; }

        AsynchronousEvent<JoinedGuildEventArgs> JoinedGuildEvent { get; }

        AsynchronousEvent<GuildUnavailableEventArgs> GuildUnavailableEvent { get; }

        AsynchronousEvent<GuildUpdatedEventArgs> GuildUpdatedEvent { get; }

        AsynchronousEvent<LeftGuildEventArgs> LeftGuildEvent { get; }

        AsynchronousEvent<BanCreatedEventArgs> BanCreatedEvent { get; }

        AsynchronousEvent<BanDeletedEventArgs> BanDeletedEvent { get; }

        AsynchronousEvent<EmojisUpdatedEventArgs> EmojisUpdatedEvent { get; }

        AsynchronousEvent<IntegrationsUpdatedEventArgs> IntegrationsUpdatedEvent { get; }

        AsynchronousEvent<MemberJoinedEventArgs> MemberJoinedEvent { get; }

        AsynchronousEvent<MemberUpdatedEventArgs> MemberUpdatedEvent { get; }

        AsynchronousEvent<MemberLeftEventArgs> MemberLeftEvent { get; }

        AsynchronousEvent<RoleCreatedEventArgs> RoleCreatedEvent { get; }

        AsynchronousEvent<RoleUpdatedEventArgs> RoleUpdatedEvent { get; }

        AsynchronousEvent<RoleDeletedEventArgs> RoleDeletedEvent { get; }

        AsynchronousEvent<IntegrationCreatedEventArgs> IntegrationCreatedEvent { get; }

        AsynchronousEvent<IntegrationUpdatedEventArgs> IntegrationUpdatedEvent { get; }

        AsynchronousEvent<IntegrationDeletedEventArgs> IntegrationDeletedEvent { get; }

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

        AsynchronousEvent<TypingStartedEventArgs> TypingStartedEvent { get; }

        AsynchronousEvent<CurrentUserUpdatedEventArgs> CurrentUserUpdatedEvent { get; }

        AsynchronousEvent<VoiceStateUpdatedEventArgs> VoiceStateUpdatedEvent { get; }

        AsynchronousEvent<VoiceServerUpdatedEventArgs> VoiceServerUpdatedEvent { get; }

        AsynchronousEvent<WebhooksUpdatedEventArgs> WebhooksUpdatedEvent { get; }
    }
}
