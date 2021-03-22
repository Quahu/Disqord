using Disqord.Events;

namespace Disqord.Gateway.Default
{
    public partial class DefaultGatewayDispatcher : IGatewayDispatcher
    {
        public AsynchronousEvent<ReadyEventArgs> ReadyEvent { get; } = new();

        public AsynchronousEvent<ChannelCreatedEventArgs> ChannelCreatedEvent { get; } = new();

        public AsynchronousEvent<ChannelUpdatedEventArgs> ChannelUpdatedEvent { get; } = new();

        public AsynchronousEvent<ChannelDeletedEventArgs> ChannelDeletedEvent { get; } = new();

        public AsynchronousEvent<ChannelPinsUpdatedEventArgs> ChannelPinsUpdatedEvent { get; } = new();

        public AsynchronousEvent<GuildAvailableEventArgs> GuildAvailableEvent { get; } = new();

        public AsynchronousEvent<JoinedGuildEventArgs> JoinedGuildEvent { get; } = new();

        public AsynchronousEvent<GuildUpdatedEventArgs> GuildUpdatedEvent { get; } = new();

        public AsynchronousEvent<GuildUnavailableEventArgs> GuildUnavailableEvent { get; } = new();

        public AsynchronousEvent<LeftGuildEventArgs> LeftGuildEvent { get; } = new();

        public AsynchronousEvent<BanCreatedEventArgs> BanCreatedEvent { get; } = new();

        public AsynchronousEvent<BanDeletedEventArgs> BanDeletedEvent { get; } = new();

        public AsynchronousEvent<WebhooksUpdatedEventArgs> WebhooksUpdatedEvent { get; } = new();

        public AsynchronousEvent<MemberJoinedEventArgs> MemberJoinedEvent { get; } = new();

        public AsynchronousEvent<MemberUpdatedEventArgs> MemberUpdatedEvent { get; } = new();

        public AsynchronousEvent<MemberLeftEventArgs> MemberLeftEvent { get; } = new();

        public AsynchronousEvent<RoleCreatedEventArgs> RoleCreatedEvent { get; } = new();

        public AsynchronousEvent<RoleUpdatedEventArgs> RoleUpdatedEvent { get; } = new();

        public AsynchronousEvent<RoleDeletedEventArgs> RoleDeletedEvent { get; } = new();

        public AsynchronousEvent<MessageReceivedEventArgs> MessageReceivedEvent { get; } = new();

        public AsynchronousEvent<MessageUpdatedEventArgs> MessageUpdatedEvent { get; } = new();

        public AsynchronousEvent<MessageDeletedEventArgs> MessageDeletedEvent { get; } = new();

        public AsynchronousEvent<ReactionAddedEventArgs> ReactionAddedEvent { get; } = new();

        public AsynchronousEvent<ReactionRemovedEventArgs> ReactionRemovedEvent { get; } = new();

        public AsynchronousEvent<ReactionsClearedEventArgs> ReactionsClearedEvent { get; } = new();

        public AsynchronousEvent<TypingStartedEventArgs> TypingStartedEvent { get; } = new();

        public AsynchronousEvent<CurrentUserUpdatedEventArgs> CurrentUserUpdatedEvent { get; } = new();

        public AsynchronousEvent<VoiceStateUpdatedEventArgs> VoiceStateUpdatedEvent { get; } = new();

        public AsynchronousEvent<VoiceServerUpdatedEventArgs> VoiceServerUpdatedEvent { get; } = new();
    }
}
