using Disqord.Events;

namespace Disqord.Gateway.Default
{
    public partial class DefaultGatewayClient
    {
        public event AsynchronousEventHandler<ReadyEventArgs> Ready
        {
            add => Dispatcher.ReadyEvent.Hook(value);
            remove => Dispatcher.ReadyEvent.Unhook(value);
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
        public event AsynchronousEventHandler<BanDeletedEventArgs> BanDeleted
        {
            add => Dispatcher.BanDeletedEvent.Hook(value);
            remove => Dispatcher.BanDeletedEvent.Unhook(value);
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
    }
}
