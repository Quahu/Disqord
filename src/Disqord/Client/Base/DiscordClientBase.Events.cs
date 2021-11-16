using Qommon.Events;
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
        public event AsynchronousEventHandler<ThreadCreatedEventArgs> ThreadCreated
        {
            add => GatewayClient.ThreadCreated += value;
            remove => GatewayClient.ThreadCreated -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<ThreadUpdatedEventArgs> ThreadUpdated
        {
            add => GatewayClient.ThreadUpdated += value;
            remove => GatewayClient.ThreadUpdated -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<ThreadDeletedEventArgs> ThreadDeleted
        {
            add => GatewayClient.ThreadDeleted += value;
            remove => GatewayClient.ThreadDeleted -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<ThreadsSynchronizedEventArgs> ThreadsSynchronized
        {
            add => GatewayClient.ThreadsSynchronized += value;
            remove => GatewayClient.ThreadsSynchronized -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<ThreadMembersUpdatedEventArgs> ThreadMembersUpdated
        {
            add => GatewayClient.ThreadMembersUpdated += value;
            remove => GatewayClient.ThreadMembersUpdated -= value;
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
        public event AsynchronousEventHandler<GuildUpdatedEventArgs> GuildUpdated
        {
            add => GatewayClient.GuildUpdated += value;
            remove => GatewayClient.GuildUpdated -= value;
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
        public event AsynchronousEventHandler<EmojisUpdatedEventArgs> EmojisUpdated
        {
            add => GatewayClient.EmojisUpdated += value;
            remove => GatewayClient.EmojisUpdated -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<StickersUpdatedEventArgs> StickersUpdated
        {
            add => GatewayClient.StickersUpdated += value;
            remove => GatewayClient.StickersUpdated -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<IntegrationsUpdatedEventArgs> IntegrationsUpdated
        {
            add => GatewayClient.IntegrationsUpdated += value;
            remove => GatewayClient.IntegrationsUpdated -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<MemberJoinedEventArgs> MemberJoined
        {
            add => GatewayClient.MemberJoined += value;
            remove => GatewayClient.MemberJoined -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<MemberUpdatedEventArgs> MemberUpdated
        {
            add => GatewayClient.MemberUpdated += value;
            remove => GatewayClient.MemberUpdated -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<MemberLeftEventArgs> MemberLeft
        {
            add => GatewayClient.MemberLeft += value;
            remove => GatewayClient.MemberLeft -= value;
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
        public event AsynchronousEventHandler<GuildEventCreatedEventArgs> GuildEventCreated
        {
            add => GatewayClient.GuildEventCreated += value;
            remove => GatewayClient.GuildEventCreated -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<GuildEventUpdatedEventArgs> GuildEventUpdated
        {
            add => GatewayClient.GuildEventUpdated += value;
            remove => GatewayClient.GuildEventUpdated -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<GuildEventDeletedEventArgs> GuildEventDeleted
        {
            add => GatewayClient.GuildEventDeleted += value;
            remove => GatewayClient.GuildEventDeleted -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<GuildEventMemberAddedEventArgs> GuildEventMemberAdded
        {
            add => GatewayClient.GuildEventMemberAdded += value;
            remove => GatewayClient.GuildEventMemberAdded -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<GuildEventMemberRemovedEventArgs> GuildEventMemberRemoved
        {
            add => GatewayClient.GuildEventMemberRemoved += value;
            remove => GatewayClient.GuildEventMemberRemoved -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<IntegrationCreatedEventArgs> IntegrationCreated
        {
            add => GatewayClient.IntegrationCreated += value;
            remove => GatewayClient.IntegrationCreated -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<IntegrationUpdatedEventArgs> IntegrationUpdated
        {
            add => GatewayClient.IntegrationUpdated += value;
            remove => GatewayClient.IntegrationUpdated -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<IntegrationDeletedEventArgs> IntegrationDeleted
        {
            add => GatewayClient.IntegrationDeleted += value;
            remove => GatewayClient.IntegrationDeleted -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<InteractionReceivedEventArgs> InteractionReceived
        {
            add => GatewayClient.InteractionReceived += value;
            remove => GatewayClient.InteractionReceived -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<InviteCreatedEventArgs> InviteCreated
        {
            add => GatewayClient.InviteCreated += value;
            remove => GatewayClient.InviteCreated -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<InviteDeletedEventArgs> InviteDeleted
        {
            add => GatewayClient.InviteDeleted += value;
            remove => GatewayClient.InviteDeleted -= value;
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
        public event AsynchronousEventHandler<MessagesDeletedEventArgs> MessagesDeleted
        {
            add => GatewayClient.MessagesDeleted += value;
            remove => GatewayClient.MessagesDeleted -= value;
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
        public event AsynchronousEventHandler<PresenceUpdatedEventArgs> PresenceUpdated
        {
            add => GatewayClient.PresenceUpdated += value;
            remove => GatewayClient.PresenceUpdated -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<StageCreatedEventArgs> StageCreated
        {
            add => GatewayClient.StageCreated += value;
            remove => GatewayClient.StageCreated -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<StageUpdatedEventArgs> StageUpdated
        {
            add => GatewayClient.StageUpdated += value;
            remove => GatewayClient.StageUpdated -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<StageDeletedEventArgs> StageDeleted
        {
            add => GatewayClient.StageDeleted += value;
            remove => GatewayClient.StageDeleted -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<TypingStartedEventArgs> TypingStarted
        {
            add => GatewayClient.TypingStarted += value;
            remove => GatewayClient.TypingStarted -= value;
        }

        /// <inheritdoc/>
        public event AsynchronousEventHandler<CurrentUserUpdatedEventArgs> CurrentUserUpdated
        {
            add => GatewayClient.CurrentUserUpdated += value;
            remove => GatewayClient.CurrentUserUpdated -= value;
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

        /// <inheritdoc/>
        public event AsynchronousEventHandler<WebhooksUpdatedEventArgs> WebhooksUpdated
        {
            add => GatewayClient.WebhooksUpdated += value;
            remove => GatewayClient.WebhooksUpdated -= value;
        }
    }
}
