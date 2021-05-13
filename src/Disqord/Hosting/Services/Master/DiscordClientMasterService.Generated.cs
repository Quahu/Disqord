using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Gateway;

namespace Disqord.Hosting
{
    public partial class DiscordClientMasterService
    {
        private readonly DiscordClientService[] _readyServices;
        private readonly DiscordClientService[] _channelCreatedServices;
        private readonly DiscordClientService[] _channelUpdatedServices;
        private readonly DiscordClientService[] _channelDeletedServices;
        private readonly DiscordClientService[] _channelPinsUpdatedServices;
        private readonly DiscordClientService[] _guildAvailableServices;
        private readonly DiscordClientService[] _joinedGuildServices;
        private readonly DiscordClientService[] _guildUpdatedServices;
        private readonly DiscordClientService[] _guildUnavailableServices;
        private readonly DiscordClientService[] _leftGuildServices;
        private readonly DiscordClientService[] _banCreatedServices;
        private readonly DiscordClientService[] _banDeletedServices;
        private readonly DiscordClientService[] _emojisUpdatedServices;
        private readonly DiscordClientService[] _integrationsUpdatedServices;
        private readonly DiscordClientService[] _memberJoinedServices;
        private readonly DiscordClientService[] _memberUpdatedServices;
        private readonly DiscordClientService[] _memberLeftServices;
        private readonly DiscordClientService[] _roleCreatedServices;
        private readonly DiscordClientService[] _roleUpdatedServices;
        private readonly DiscordClientService[] _roleDeletedServices;
        private readonly DiscordClientService[] _integrationCreatedServices;
        private readonly DiscordClientService[] _integrationUpdatedServices;
        private readonly DiscordClientService[] _integrationDeletedServices;
        private readonly DiscordClientService[] _inviteCreatedServices;
        private readonly DiscordClientService[] _inviteDeletedServices;
        private readonly DiscordClientService[] _messageReceivedServices;
        private readonly DiscordClientService[] _messageUpdatedServices;
        private readonly DiscordClientService[] _messageDeletedServices;
        private readonly DiscordClientService[] _messagesDeletedServices;
        private readonly DiscordClientService[] _reactionAddedServices;
        private readonly DiscordClientService[] _reactionRemovedServices;
        private readonly DiscordClientService[] _reactionsClearedServices;
        private readonly DiscordClientService[] _presenceUpdatedServices;
        private readonly DiscordClientService[] _typingStartedServices;
        private readonly DiscordClientService[] _currentUserUpdatedServices;
        private readonly DiscordClientService[] _voiceStateUpdatedServices;
        private readonly DiscordClientService[] _voiceServerUpdatedServices;
        private readonly DiscordClientService[] _webhooksUpdatedServices;

        private DiscordClientMasterService(
            DiscordClientBase client,
            IEnumerable<DiscordClientService> services)
        {
            Client = client;

            _readyServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnReady))).ToArray();
            _channelCreatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnChannelCreated))).ToArray();
            _channelUpdatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnChannelUpdated))).ToArray();
            _channelDeletedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnChannelDeleted))).ToArray();
            _channelPinsUpdatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnChannelPinsUpdated))).ToArray();
            _guildAvailableServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnGuildAvailable))).ToArray();
            _joinedGuildServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnJoinedGuild))).ToArray();
            _guildUpdatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnGuildUpdated))).ToArray();
            _guildUnavailableServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnGuildUnavailable))).ToArray();
            _leftGuildServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnLeftGuild))).ToArray();
            _banCreatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnBanCreated))).ToArray();
            _banDeletedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnBanDeleted))).ToArray();
            _emojisUpdatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnEmojisUpdated))).ToArray();
            _integrationsUpdatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnIntegrationsUpdated))).ToArray();
            _memberJoinedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnMemberJoined))).ToArray();
            _memberUpdatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnMemberUpdated))).ToArray();
            _memberLeftServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnMemberLeft))).ToArray();
            _roleCreatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnRoleCreated))).ToArray();
            _roleUpdatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnRoleUpdated))).ToArray();
            _roleDeletedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnRoleDeleted))).ToArray();
            _integrationCreatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnIntegrationCreated))).ToArray();
            _integrationUpdatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnIntegrationUpdated))).ToArray();
            _integrationDeletedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnIntegrationDeleted))).ToArray();
            _inviteCreatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnInviteCreated))).ToArray();
            _inviteDeletedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnInviteDeleted))).ToArray();
            _messageReceivedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnMessageReceived))).ToArray();
            _messageUpdatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnMessageUpdated))).ToArray();
            _messageDeletedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnMessageDeleted))).ToArray();
            _messagesDeletedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnMessagesDeleted))).ToArray();
            _reactionAddedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnReactionAdded))).ToArray();
            _reactionRemovedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnReactionRemoved))).ToArray();
            _reactionsClearedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnReactionsCleared))).ToArray();
            _presenceUpdatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnPresenceUpdated))).ToArray();
            _typingStartedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnTypingStarted))).ToArray();
            _currentUserUpdatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnCurrentUserUpdated))).ToArray();
            _voiceStateUpdatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnVoiceStateUpdated))).ToArray();
            _voiceServerUpdatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnVoiceServerUpdated))).ToArray();
            _webhooksUpdatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnWebhooksUpdated))).ToArray();

            Client.Ready += HandleReady;
            Client.ChannelCreated += HandleChannelCreated;
            Client.ChannelUpdated += HandleChannelUpdated;
            Client.ChannelDeleted += HandleChannelDeleted;
            Client.ChannelPinsUpdated += HandleChannelPinsUpdated;
            Client.GuildAvailable += HandleGuildAvailable;
            Client.JoinedGuild += HandleJoinedGuild;
            Client.GuildUpdated += HandleGuildUpdated;
            Client.GuildUnavailable += HandleGuildUnavailable;
            Client.LeftGuild += HandleLeftGuild;
            Client.BanCreated += HandleBanCreated;
            Client.BanDeleted += HandleBanDeleted;
            Client.EmojisUpdated += HandleEmojisUpdated;
            Client.IntegrationsUpdated += HandleIntegrationsUpdated;
            Client.MemberJoined += HandleMemberJoined;
            Client.MemberUpdated += HandleMemberUpdated;
            Client.MemberLeft += HandleMemberLeft;
            Client.RoleCreated += HandleRoleCreated;
            Client.RoleUpdated += HandleRoleUpdated;
            Client.RoleDeleted += HandleRoleDeleted;
            Client.IntegrationCreated += HandleIntegrationCreated;
            Client.IntegrationUpdated += HandleIntegrationUpdated;
            Client.IntegrationDeleted += HandleIntegrationDeleted;
            Client.InviteCreated += HandleInviteCreated;
            Client.InviteDeleted += HandleInviteDeleted;
            Client.MessageReceived += HandleMessageReceived;
            Client.MessageUpdated += HandleMessageUpdated;
            Client.MessageDeleted += HandleMessageDeleted;
            Client.MessagesDeleted += HandleMessagesDeleted;
            Client.ReactionAdded += HandleReactionAdded;
            Client.ReactionRemoved += HandleReactionRemoved;
            Client.ReactionsCleared += HandleReactionsCleared;
            Client.PresenceUpdated += HandlePresenceUpdated;
            Client.TypingStarted += HandleTypingStarted;
            Client.CurrentUserUpdated += HandleCurrentUserUpdated;
            Client.VoiceStateUpdated += HandleVoiceStateUpdated;
            Client.VoiceServerUpdated += HandleVoiceServerUpdated;
            Client.WebhooksUpdated += HandleWebhooksUpdated;
        }

        public async ValueTask HandleReady(object sender, ReadyEventArgs e)
        {
            foreach (var service in _readyServices)
                await ExecuteAsync((service, e) => service.OnReady(e), service, e).ConfigureAwait(false);
        }

        public ValueTask HandleChannelCreated(object sender, ChannelCreatedEventArgs e)
        {
            foreach (var service in _channelCreatedServices)
                _ = ExecuteAsync((service, e) => service.OnChannelCreated(e), service, e);

			return default;
        }

        public ValueTask HandleChannelUpdated(object sender, ChannelUpdatedEventArgs e)
        {
            foreach (var service in _channelUpdatedServices)
                _ = ExecuteAsync((service, e) => service.OnChannelUpdated(e), service, e);

			return default;
        }

        public ValueTask HandleChannelDeleted(object sender, ChannelDeletedEventArgs e)
        {
            foreach (var service in _channelDeletedServices)
                _ = ExecuteAsync((service, e) => service.OnChannelDeleted(e), service, e);

			return default;
        }

        public ValueTask HandleChannelPinsUpdated(object sender, ChannelPinsUpdatedEventArgs e)
        {
            foreach (var service in _channelPinsUpdatedServices)
                _ = ExecuteAsync((service, e) => service.OnChannelPinsUpdated(e), service, e);

			return default;
        }

        public ValueTask HandleGuildAvailable(object sender, GuildAvailableEventArgs e)
        {
            foreach (var service in _guildAvailableServices)
                _ = ExecuteAsync((service, e) => service.OnGuildAvailable(e), service, e);

			return default;
        }

        public ValueTask HandleJoinedGuild(object sender, JoinedGuildEventArgs e)
        {
            foreach (var service in _joinedGuildServices)
                _ = ExecuteAsync((service, e) => service.OnJoinedGuild(e), service, e);

			return default;
        }

        public ValueTask HandleGuildUpdated(object sender, GuildUpdatedEventArgs e)
        {
            foreach (var service in _guildUpdatedServices)
                _ = ExecuteAsync((service, e) => service.OnGuildUpdated(e), service, e);

			return default;
        }

        public ValueTask HandleGuildUnavailable(object sender, GuildUnavailableEventArgs e)
        {
            foreach (var service in _guildUnavailableServices)
                _ = ExecuteAsync((service, e) => service.OnGuildUnavailable(e), service, e);

			return default;
        }

        public ValueTask HandleLeftGuild(object sender, LeftGuildEventArgs e)
        {
            foreach (var service in _leftGuildServices)
                _ = ExecuteAsync((service, e) => service.OnLeftGuild(e), service, e);

			return default;
        }

        public ValueTask HandleBanCreated(object sender, BanCreatedEventArgs e)
        {
            foreach (var service in _banCreatedServices)
                _ = ExecuteAsync((service, e) => service.OnBanCreated(e), service, e);

			return default;
        }

        public ValueTask HandleBanDeleted(object sender, BanDeletedEventArgs e)
        {
            foreach (var service in _banDeletedServices)
                _ = ExecuteAsync((service, e) => service.OnBanDeleted(e), service, e);

			return default;
        }

        public ValueTask HandleEmojisUpdated(object sender, EmojisUpdatedEventArgs e)
        {
            foreach (var service in _emojisUpdatedServices)
                _ = ExecuteAsync((service, e) => service.OnEmojisUpdated(e), service, e);

			return default;
        }
        
        public ValueTask HandleIntegrationsUpdated(object sender, IntegrationsUpdatedEventArgs e)
        {
            foreach (var service in _integrationsUpdatedServices)
                _ = ExecuteAsync((service, e) => service.OnIntegrationsUpdated(e), service, e);

            return default;
        }

        public ValueTask HandleMemberJoined(object sender, MemberJoinedEventArgs e)
        {
            foreach (var service in _memberJoinedServices)
                _ = ExecuteAsync((service, e) => service.OnMemberJoined(e), service, e);

			return default;
        }

        public ValueTask HandleMemberUpdated(object sender, MemberUpdatedEventArgs e)
        {
            foreach (var service in _memberUpdatedServices)
                _ = ExecuteAsync((service, e) => service.OnMemberUpdated(e), service, e);

			return default;
        }

        public ValueTask HandleMemberLeft(object sender, MemberLeftEventArgs e)
        {
            foreach (var service in _memberLeftServices)
                _ = ExecuteAsync((service, e) => service.OnMemberLeft(e), service, e);

			return default;
        }

        public ValueTask HandleRoleCreated(object sender, RoleCreatedEventArgs e)
        {
            foreach (var service in _roleCreatedServices)
                _ = ExecuteAsync((service, e) => service.OnRoleCreated(e), service, e);

			return default;
        }

        public ValueTask HandleRoleUpdated(object sender, RoleUpdatedEventArgs e)
        {
            foreach (var service in _roleUpdatedServices)
                _ = ExecuteAsync((service, e) => service.OnRoleUpdated(e), service, e);

			return default;
        }

        public ValueTask HandleRoleDeleted(object sender, RoleDeletedEventArgs e)
        {
            foreach (var service in _roleDeletedServices)
                _ = ExecuteAsync((service, e) => service.OnRoleDeleted(e), service, e);

			return default;
        }

        public ValueTask HandleIntegrationCreated(object sender, IntegrationCreatedEventArgs e)
        {
            foreach (var service in _integrationCreatedServices)
                _ = ExecuteAsync((service, e) => service.OnIntegrationCreated(e), service, e);

            return default;
        }

        public ValueTask HandleIntegrationUpdated(object sender, IntegrationUpdatedEventArgs e)
        {
            foreach (var service in _integrationUpdatedServices)
                _ = ExecuteAsync((service, e) => service.OnIntegrationUpdated(e), service, e);

            return default;
        }

        public ValueTask HandleIntegrationDeleted(object sender, IntegrationDeletedEventArgs e)
        {
            foreach (var service in _integrationDeletedServices)
                _ = ExecuteAsync((service, e) => service.OnIntegrationDeleted(e), service, e);

            return default;
        }

        public ValueTask HandleInviteCreated(object sender, InviteCreatedEventArgs e)
        {
            foreach (var service in _inviteCreatedServices)
                _ = ExecuteAsync((service, e) => service.OnInviteCreated(e), service, e);

			return default;
        }

        public ValueTask HandleInviteDeleted(object sender, InviteDeletedEventArgs e)
        {
            foreach (var service in _inviteDeletedServices)
                _ = ExecuteAsync((service, e) => service.OnInviteDeleted(e), service, e);

			return default;
        }

        public ValueTask HandleMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            foreach (var service in _messageReceivedServices)
                _ = ExecuteAsync((service, e) => service.OnMessageReceived(e), service, e);

			return default;
        }

        public ValueTask HandleMessageUpdated(object sender, MessageUpdatedEventArgs e)
        {
            foreach (var service in _messageUpdatedServices)
                _ = ExecuteAsync((service, e) => service.OnMessageUpdated(e), service, e);

			return default;
        }

        public ValueTask HandleMessageDeleted(object sender, MessageDeletedEventArgs e)
        {
            foreach (var service in _messageDeletedServices)
                _ = ExecuteAsync((service, e) => service.OnMessageDeleted(e), service, e);

			return default;
        }

        public ValueTask HandleMessagesDeleted(object sender, MessagesDeletedEventArgs e)
        {
            foreach (var service in _messagesDeletedServices)
                _ = ExecuteAsync((service, e) => service.OnMessagesDeleted(e), service, e);

			return default;
        }

        public ValueTask HandleReactionAdded(object sender, ReactionAddedEventArgs e)
        {
            foreach (var service in _reactionAddedServices)
                _ = ExecuteAsync((service, e) => service.OnReactionAdded(e), service, e);

			return default;
        }

        public ValueTask HandleReactionRemoved(object sender, ReactionRemovedEventArgs e)
        {
            foreach (var service in _reactionRemovedServices)
                _ = ExecuteAsync((service, e) => service.OnReactionRemoved(e), service, e);

			return default;
        }

        public ValueTask HandleReactionsCleared(object sender, ReactionsClearedEventArgs e)
        {
            foreach (var service in _reactionsClearedServices)
                _ = ExecuteAsync((service, e) => service.OnReactionsCleared(e), service, e);

			return default;
        }

        public ValueTask HandlePresenceUpdated(object sender, PresenceUpdatedEventArgs e)
        {
            foreach (var service in _presenceUpdatedServices)
                _ = ExecuteAsync((service, e) => service.OnPresenceUpdated(e), service, e);

            return default;
        }

        public ValueTask HandleTypingStarted(object sender, TypingStartedEventArgs e)
        {
            foreach (var service in _typingStartedServices)
                _ = ExecuteAsync((service, e) => service.OnTypingStarted(e), service, e);

			return default;
        }

        public ValueTask HandleCurrentUserUpdated(object sender, CurrentUserUpdatedEventArgs e)
        {
            foreach (var service in _currentUserUpdatedServices)
                _ = ExecuteAsync((service, e) => service.OnCurrentUserUpdated(e), service, e);

			return default;
        }

        public ValueTask HandleVoiceStateUpdated(object sender, VoiceStateUpdatedEventArgs e)
        {
            foreach (var service in _voiceStateUpdatedServices)
                _ = ExecuteAsync((service, e) => service.OnVoiceStateUpdated(e), service, e);

			return default;
        }

        public ValueTask HandleVoiceServerUpdated(object sender, VoiceServerUpdatedEventArgs e)
        {
            foreach (var service in _voiceServerUpdatedServices)
                _ = ExecuteAsync((service, e) => service.OnVoiceServerUpdated(e), service, e);

			return default;
        }

        public ValueTask HandleWebhooksUpdated(object sender, WebhooksUpdatedEventArgs e)
        {
            foreach (var service in _webhooksUpdatedServices)
                _ = ExecuteAsync((service, e) => service.OnWebhooksUpdated(e), service, e);

			return default;
        }
    }
}

