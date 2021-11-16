using System.Threading.Tasks;
using Disqord.Gateway;

namespace Disqord.Hosting
{
    public abstract partial class DiscordClientService
    {
        protected internal virtual ValueTask OnReady(ReadyEventArgs e)
            => default;

        protected internal virtual ValueTask OnChannelCreated(ChannelCreatedEventArgs e)
            => default;

        protected internal virtual ValueTask OnChannelUpdated(ChannelUpdatedEventArgs e)
            => default;

        protected internal virtual ValueTask OnChannelDeleted(ChannelDeletedEventArgs e)
            => default;

        protected internal virtual ValueTask OnThreadCreated(ThreadCreatedEventArgs e)
            => default;

        protected internal virtual ValueTask OnThreadUpdated(ThreadUpdatedEventArgs e)
            => default;

        protected internal virtual ValueTask OnThreadDeleted(ThreadDeletedEventArgs e)
            => default;

        protected internal virtual ValueTask OnThreadsSynchronized(ThreadsSynchronizedEventArgs e)
            => default;

        protected internal virtual ValueTask OnThreadMembersUpdated(ThreadMembersUpdatedEventArgs e)
            => default;

        protected internal virtual ValueTask OnChannelPinsUpdated(ChannelPinsUpdatedEventArgs e)
            => default;

        protected internal virtual ValueTask OnGuildAvailable(GuildAvailableEventArgs e)
            => default;

        protected internal virtual ValueTask OnJoinedGuild(JoinedGuildEventArgs e)
            => default;

        protected internal virtual ValueTask OnGuildUpdated(GuildUpdatedEventArgs e)
            => default;

        protected internal virtual ValueTask OnGuildUnavailable(GuildUnavailableEventArgs e)
            => default;

        protected internal virtual ValueTask OnLeftGuild(LeftGuildEventArgs e)
            => default;

        protected internal virtual ValueTask OnBanCreated(BanCreatedEventArgs e)
            => default;

        protected internal virtual ValueTask OnBanDeleted(BanDeletedEventArgs e)
            => default;

        protected internal virtual ValueTask OnEmojisUpdated(EmojisUpdatedEventArgs e)
            => default;

        protected internal virtual ValueTask OnStickersUpdated(StickersUpdatedEventArgs e)
            => default;

        protected internal virtual ValueTask OnIntegrationsUpdated(IntegrationsUpdatedEventArgs e)
            => default;

        protected internal virtual ValueTask OnMemberJoined(MemberJoinedEventArgs e)
            => default;

        protected internal virtual ValueTask OnMemberUpdated(MemberUpdatedEventArgs e)
            => default;

        protected internal virtual ValueTask OnMemberLeft(MemberLeftEventArgs e)
            => default;

        protected internal virtual ValueTask OnRoleCreated(RoleCreatedEventArgs e)
            => default;

        protected internal virtual ValueTask OnRoleUpdated(RoleUpdatedEventArgs e)
            => default;

        protected internal virtual ValueTask OnRoleDeleted(RoleDeletedEventArgs e)
            => default;

        protected internal virtual ValueTask OnGuildEventCreated(GuildEventCreatedEventArgs e)
            => default;

        protected internal virtual ValueTask OnGuildEventUpdated(GuildEventUpdatedEventArgs e)
            => default;

        protected internal virtual ValueTask OnGuildEventDeleted(GuildEventDeletedEventArgs e)
            => default;

        protected internal virtual ValueTask OnGuildEventMemberAdded(GuildEventMemberAddedEventArgs e)
            => default;

        protected internal virtual ValueTask OnGuildEventMemberRemoved(GuildEventMemberRemovedEventArgs e)
            => default;

        protected internal virtual ValueTask OnIntegrationCreated(IntegrationCreatedEventArgs e)
            => default;

        protected internal virtual ValueTask OnIntegrationUpdated(IntegrationUpdatedEventArgs e)
            => default;

        protected internal virtual ValueTask OnIntegrationDeleted(IntegrationDeletedEventArgs e)
            => default;

        protected internal virtual ValueTask OnInteractionReceived(InteractionReceivedEventArgs e)
            => default;

        protected internal virtual ValueTask OnInviteCreated(InviteCreatedEventArgs e)
            => default;

        protected internal virtual ValueTask OnInviteDeleted(InviteDeletedEventArgs e)
            => default;

        protected internal virtual ValueTask OnMessageReceived(MessageReceivedEventArgs e)
            => default;

        protected internal virtual ValueTask OnMessageUpdated(MessageUpdatedEventArgs e)
            => default;

        protected internal virtual ValueTask OnMessageDeleted(MessageDeletedEventArgs e)
            => default;

        protected internal virtual ValueTask OnMessagesDeleted(MessagesDeletedEventArgs e)
            => default;

        protected internal virtual ValueTask OnReactionAdded(ReactionAddedEventArgs e)
            => default;

        protected internal virtual ValueTask OnReactionRemoved(ReactionRemovedEventArgs e)
            => default;

        protected internal virtual ValueTask OnReactionsCleared(ReactionsClearedEventArgs e)
            => default;

        protected internal virtual ValueTask OnPresenceUpdated(PresenceUpdatedEventArgs e)
            => default;

        protected internal virtual ValueTask OnStageCreated(StageCreatedEventArgs e)
            => default;

        protected internal virtual ValueTask OnStageUpdated(StageUpdatedEventArgs e)
            => default;

        protected internal virtual ValueTask OnStageDeleted(StageDeletedEventArgs e)
            => default;

        protected internal virtual ValueTask OnTypingStarted(TypingStartedEventArgs e)
            => default;

        protected internal virtual ValueTask OnCurrentUserUpdated(CurrentUserUpdatedEventArgs e)
            => default;

        protected internal virtual ValueTask OnVoiceStateUpdated(VoiceStateUpdatedEventArgs e)
            => default;

        protected internal virtual ValueTask OnVoiceServerUpdated(VoiceServerUpdatedEventArgs e)
            => default;

        protected internal virtual ValueTask OnWebhooksUpdated(WebhooksUpdatedEventArgs e)
            => default;
    }
}
