using Disqord.Events;
using Disqord.Gateway;

namespace Disqord
{
    public abstract partial class DiscordClientBase
    {
        public event AsynchronousEventHandler<ReadyEventArgs> Ready
        {
            add => GatewayClient.Ready += value;
            remove => GatewayClient.Ready -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<ChannelCreatedEventArgs> ChannelCreated
        {
            add => GatewayClient.ChannelCreated += value;
            remove => GatewayClient.ChannelCreated -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<ChannelUpdatedEventArgs> ChannelUpdated
        {
            add => GatewayClient.ChannelUpdated += value;
            remove => GatewayClient.ChannelUpdated -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<ChannelDeletedEventArgs> ChannelDeleted
        {
            add => GatewayClient.ChannelDeleted += value;
            remove => GatewayClient.ChannelDeleted -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<ChannelPinsUpdatedEventArgs> ChannelPinsUpdated
        {
            add => GatewayClient.ChannelPinsUpdated += value;
            remove => GatewayClient.ChannelPinsUpdated -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<GuildAvailableEventArgs> GuildAvailable
        {
            add => GatewayClient.GuildAvailable += value;
            remove => GatewayClient.GuildAvailable -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<JoinedGuildEventArgs> JoinedGuild
        {
            add => GatewayClient.JoinedGuild += value;
            remove => GatewayClient.JoinedGuild -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<GuildUnavailableEventArgs> GuildUnavailable
        {
            add => GatewayClient.GuildUnavailable += value;
            remove => GatewayClient.GuildUnavailable -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<LeftGuildEventArgs> LeftGuild
        {
            add => GatewayClient.LeftGuild += value;
            remove => GatewayClient.LeftGuild -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<BanCreatedEventArgs> BanCreated
        {
            add => GatewayClient.BanCreated += value;
            remove => GatewayClient.BanCreated -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<BanDeletedEventArgs> BanDeleted
        {
            add => GatewayClient.BanDeleted += value;
            remove => GatewayClient.BanDeleted -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<RoleCreatedEventArgs> RoleCreated
        {
            add => GatewayClient.RoleCreated += value;
            remove => GatewayClient.RoleCreated -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<RoleUpdatedEventArgs> RoleUpdated
        {
            add => GatewayClient.RoleUpdated += value;
            remove => GatewayClient.RoleUpdated -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<RoleDeletedEventArgs> RoleDeleted
        {
            add => GatewayClient.RoleDeleted += value;
            remove => GatewayClient.RoleDeleted -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<MessageReceivedEventArgs> MessageReceived
        {
            add => GatewayClient.MessageReceived += value;
            remove => GatewayClient.MessageReceived -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<MessageUpdatedEventArgs> MessageUpdated
        {
            add => GatewayClient.MessageUpdated += value;
            remove => GatewayClient.MessageUpdated -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<MessageDeletedEventArgs> MessageDeleted
        {
            add => GatewayClient.MessageDeleted += value;
            remove => GatewayClient.MessageDeleted -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<ReactionAddedEventArgs> ReactionAdded
        {
            add => GatewayClient.ReactionAdded += value;
            remove => GatewayClient.ReactionAdded -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<ReactionRemovedEventArgs> ReactionRemoved
        {
            add => GatewayClient.ReactionRemoved += value;
            remove => GatewayClient.ReactionRemoved -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<ReactionsClearedEventArgs> ReactionsCleared
        {
            add => GatewayClient.ReactionsCleared += value;
            remove => GatewayClient.ReactionsCleared -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<TypingStartedEventArgs> TypingStarted
        {
            add => GatewayClient.TypingStarted += value;
            remove => GatewayClient.TypingStarted -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<VoiceStateUpdatedEventArgs> VoiceStateUpdated
        {
            add => GatewayClient.VoiceStateUpdated += value;
            remove => GatewayClient.VoiceStateUpdated -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<VoiceServerUpdatedEventArgs> VoiceServerUpdated
        {
            add => GatewayClient.VoiceServerUpdated += value;
            remove => GatewayClient.VoiceServerUpdated -= value;
        }
    }
}
