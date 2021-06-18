using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Gateway;
using Microsoft.Extensions.Logging;

namespace Disqord.Hosting
{
    public partial class DiscordClientMasterService
    {
        public DiscordClientService[] ReadyServices { get; }
        public DiscordClientService[] ChannelCreatedServices { get; }
        public DiscordClientService[] ChannelUpdatedServices { get; }
        public DiscordClientService[] ChannelDeletedServices { get; }
        public DiscordClientService[] ChannelPinsUpdatedServices { get; }
        public DiscordClientService[] GuildAvailableServices { get; }
        public DiscordClientService[] JoinedGuildServices { get; }
        public DiscordClientService[] GuildUpdatedServices { get; }
        public DiscordClientService[] GuildUnavailableServices { get; }
        public DiscordClientService[] LeftGuildServices { get; }
        public DiscordClientService[] BanCreatedServices { get; }
        public DiscordClientService[] BanDeletedServices { get; }
        public DiscordClientService[] EmojisUpdatedServices { get; }
        public DiscordClientService[] IntegrationsUpdatedServices { get; }
        public DiscordClientService[] MemberJoinedServices { get; }
        public DiscordClientService[] MemberUpdatedServices { get; }
        public DiscordClientService[] MemberLeftServices { get; }
        public DiscordClientService[] RoleCreatedServices { get; }
        public DiscordClientService[] RoleUpdatedServices { get; }
        public DiscordClientService[] RoleDeletedServices { get; }
        public DiscordClientService[] IntegrationCreatedServices { get; }
        public DiscordClientService[] IntegrationUpdatedServices { get; }
        public DiscordClientService[] IntegrationDeletedServices { get; }
        public DiscordClientService[] InteractionReceivedServices { get; }
        public DiscordClientService[] InviteCreatedServices { get; }
        public DiscordClientService[] InviteDeletedServices { get; }
        public DiscordClientService[] MessageReceivedServices { get; }
        public DiscordClientService[] MessageUpdatedServices { get; }
        public DiscordClientService[] MessageDeletedServices { get; }
        public DiscordClientService[] MessagesDeletedServices { get; }
        public DiscordClientService[] ReactionAddedServices { get; }
        public DiscordClientService[] ReactionRemovedServices { get; }
        public DiscordClientService[] ReactionsClearedServices { get; }
        public DiscordClientService[] PresenceUpdatedServices { get; }
        public DiscordClientService[] TypingStartedServices { get; }
        public DiscordClientService[] CurrentUserUpdatedServices { get; }
        public DiscordClientService[] VoiceStateUpdatedServices { get; }
        public DiscordClientService[] VoiceServerUpdatedServices { get; }
        public DiscordClientService[] WebhooksUpdatedServices { get; }

        private DiscordClientMasterService(
            DiscordClientBase client,
            IEnumerable<DiscordClientService> services,
            IServiceProvider serviceProvider)
        {
            Client = client;

            foreach (var service in services)
            {
                service.Logger ??= serviceProvider.GetService(typeof(ILogger<>).MakeGenericType(service.GetType())) as ILogger;
                service.Client ??= client;
            }

            ReadyServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnReady), typeof(ReadyEventArgs))).ToArray();
            ChannelCreatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnChannelCreated), typeof(ChannelCreatedEventArgs))).ToArray();
            ChannelUpdatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnChannelUpdated), typeof(ChannelUpdatedEventArgs))).ToArray();
            ChannelDeletedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnChannelDeleted), typeof(ChannelDeletedEventArgs))).ToArray();
            ChannelPinsUpdatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnChannelPinsUpdated), typeof(ChannelPinsUpdatedEventArgs))).ToArray();
            GuildAvailableServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnGuildAvailable), typeof(GuildAvailableEventArgs))).ToArray();
            JoinedGuildServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnJoinedGuild), typeof(JoinedGuildEventArgs))).ToArray();
            GuildUpdatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnGuildUpdated), typeof(GuildUpdatedEventArgs))).ToArray();
            GuildUnavailableServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnGuildUnavailable), typeof(GuildUnavailableEventArgs))).ToArray();
            LeftGuildServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnLeftGuild), typeof(LeftGuildEventArgs))).ToArray();
            BanCreatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnBanCreated), typeof(BanCreatedEventArgs))).ToArray();
            BanDeletedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnBanDeleted), typeof(BanDeletedEventArgs))).ToArray();
            EmojisUpdatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnEmojisUpdated), typeof(EmojisUpdatedEventArgs))).ToArray();
            IntegrationsUpdatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnIntegrationsUpdated), typeof(IntegrationsUpdatedEventArgs))).ToArray();
            MemberJoinedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnMemberJoined), typeof(MemberJoinedEventArgs))).ToArray();
            MemberUpdatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnMemberUpdated), typeof(MemberUpdatedEventArgs))).ToArray();
            MemberLeftServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnMemberLeft), typeof(MemberLeftEventArgs))).ToArray();
            RoleCreatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnRoleCreated), typeof(RoleCreatedEventArgs))).ToArray();
            RoleUpdatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnRoleUpdated), typeof(RoleUpdatedEventArgs))).ToArray();
            RoleDeletedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnRoleDeleted), typeof(RoleDeletedEventArgs))).ToArray();
            IntegrationCreatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnIntegrationCreated), typeof(IntegrationCreatedEventArgs))).ToArray();
            IntegrationUpdatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnIntegrationUpdated), typeof(IntegrationUpdatedEventArgs))).ToArray();
            IntegrationDeletedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnIntegrationDeleted), typeof(IntegrationDeletedEventArgs))).ToArray();
            InteractionReceivedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnInteractionReceived), typeof(InteractionReceivedEventArgs))).ToArray();
            InviteCreatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnInviteCreated), typeof(InviteCreatedEventArgs))).ToArray();
            InviteDeletedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnInviteDeleted), typeof(InviteDeletedEventArgs))).ToArray();
            MessageReceivedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnMessageReceived), typeof(MessageReceivedEventArgs))).ToArray();
            MessageUpdatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnMessageUpdated), typeof(MessageUpdatedEventArgs))).ToArray();
            MessageDeletedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnMessageDeleted), typeof(MessageDeletedEventArgs))).ToArray();
            MessagesDeletedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnMessagesDeleted), typeof(MessagesDeletedEventArgs))).ToArray();
            ReactionAddedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnReactionAdded), typeof(ReactionAddedEventArgs))).ToArray();
            ReactionRemovedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnReactionRemoved), typeof(ReactionRemovedEventArgs))).ToArray();
            ReactionsClearedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnReactionsCleared), typeof(ReactionsClearedEventArgs))).ToArray();
            PresenceUpdatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnPresenceUpdated), typeof(PresenceUpdatedEventArgs))).ToArray();
            TypingStartedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnTypingStarted), typeof(TypingStartedEventArgs))).ToArray();
            CurrentUserUpdatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnCurrentUserUpdated), typeof(CurrentUserUpdatedEventArgs))).ToArray();
            VoiceStateUpdatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnVoiceStateUpdated), typeof(VoiceStateUpdatedEventArgs))).ToArray();
            VoiceServerUpdatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnVoiceServerUpdated), typeof(VoiceServerUpdatedEventArgs))).ToArray();
            WebhooksUpdatedServices = services.Where(x => IsOverridden(x, nameof(DiscordClientService.OnWebhooksUpdated), typeof(WebhooksUpdatedEventArgs))).ToArray();

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
            Client.InteractionReceived += HandleInteractionReceived;
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
            foreach (var service in ReadyServices)
                await ExecuteAsync((service, e) => service.OnReady(e), service, e).ConfigureAwait(false);
        }

        public ValueTask HandleChannelCreated(object sender, ChannelCreatedEventArgs e)
        {
            foreach (var service in ChannelCreatedServices)
                _ = ExecuteAsync((service, e) => service.OnChannelCreated(e), service, e);

			return default;
        }

        public ValueTask HandleChannelUpdated(object sender, ChannelUpdatedEventArgs e)
        {
            foreach (var service in ChannelUpdatedServices)
                _ = ExecuteAsync((service, e) => service.OnChannelUpdated(e), service, e);

			return default;
        }

        public ValueTask HandleChannelDeleted(object sender, ChannelDeletedEventArgs e)
        {
            foreach (var service in ChannelDeletedServices)
                _ = ExecuteAsync((service, e) => service.OnChannelDeleted(e), service, e);

			return default;
        }

        public ValueTask HandleChannelPinsUpdated(object sender, ChannelPinsUpdatedEventArgs e)
        {
            foreach (var service in ChannelPinsUpdatedServices)
                _ = ExecuteAsync((service, e) => service.OnChannelPinsUpdated(e), service, e);

			return default;
        }

        public ValueTask HandleGuildAvailable(object sender, GuildAvailableEventArgs e)
        {
            foreach (var service in GuildAvailableServices)
                _ = ExecuteAsync((service, e) => service.OnGuildAvailable(e), service, e);

			return default;
        }

        public ValueTask HandleJoinedGuild(object sender, JoinedGuildEventArgs e)
        {
            foreach (var service in JoinedGuildServices)
                _ = ExecuteAsync((service, e) => service.OnJoinedGuild(e), service, e);

			return default;
        }

        public ValueTask HandleGuildUpdated(object sender, GuildUpdatedEventArgs e)
        {
            foreach (var service in GuildUpdatedServices)
                _ = ExecuteAsync((service, e) => service.OnGuildUpdated(e), service, e);

			return default;
        }

        public ValueTask HandleGuildUnavailable(object sender, GuildUnavailableEventArgs e)
        {
            foreach (var service in GuildUnavailableServices)
                _ = ExecuteAsync((service, e) => service.OnGuildUnavailable(e), service, e);

			return default;
        }

        public ValueTask HandleLeftGuild(object sender, LeftGuildEventArgs e)
        {
            foreach (var service in LeftGuildServices)
                _ = ExecuteAsync((service, e) => service.OnLeftGuild(e), service, e);

			return default;
        }

        public ValueTask HandleBanCreated(object sender, BanCreatedEventArgs e)
        {
            foreach (var service in BanCreatedServices)
                _ = ExecuteAsync((service, e) => service.OnBanCreated(e), service, e);

			return default;
        }

        public ValueTask HandleBanDeleted(object sender, BanDeletedEventArgs e)
        {
            foreach (var service in BanDeletedServices)
                _ = ExecuteAsync((service, e) => service.OnBanDeleted(e), service, e);

			return default;
        }

        public ValueTask HandleEmojisUpdated(object sender, EmojisUpdatedEventArgs e)
        {
            foreach (var service in EmojisUpdatedServices)
                _ = ExecuteAsync((service, e) => service.OnEmojisUpdated(e), service, e);

			return default;
        }
        
        public ValueTask HandleIntegrationsUpdated(object sender, IntegrationsUpdatedEventArgs e)
        {
            foreach (var service in IntegrationsUpdatedServices)
                _ = ExecuteAsync((service, e) => service.OnIntegrationsUpdated(e), service, e);

            return default;
        }

        public ValueTask HandleMemberJoined(object sender, MemberJoinedEventArgs e)
        {
            foreach (var service in MemberJoinedServices)
                _ = ExecuteAsync((service, e) => service.OnMemberJoined(e), service, e);

			return default;
        }

        public ValueTask HandleMemberUpdated(object sender, MemberUpdatedEventArgs e)
        {
            foreach (var service in MemberUpdatedServices)
                _ = ExecuteAsync((service, e) => service.OnMemberUpdated(e), service, e);

			return default;
        }

        public ValueTask HandleMemberLeft(object sender, MemberLeftEventArgs e)
        {
            foreach (var service in MemberLeftServices)
                _ = ExecuteAsync((service, e) => service.OnMemberLeft(e), service, e);

			return default;
        }

        public ValueTask HandleRoleCreated(object sender, RoleCreatedEventArgs e)
        {
            foreach (var service in RoleCreatedServices)
                _ = ExecuteAsync((service, e) => service.OnRoleCreated(e), service, e);

			return default;
        }

        public ValueTask HandleRoleUpdated(object sender, RoleUpdatedEventArgs e)
        {
            foreach (var service in RoleUpdatedServices)
                _ = ExecuteAsync((service, e) => service.OnRoleUpdated(e), service, e);

			return default;
        }

        public ValueTask HandleRoleDeleted(object sender, RoleDeletedEventArgs e)
        {
            foreach (var service in RoleDeletedServices)
                _ = ExecuteAsync((service, e) => service.OnRoleDeleted(e), service, e);

			return default;
        }

        public ValueTask HandleIntegrationCreated(object sender, IntegrationCreatedEventArgs e)
        {
            foreach (var service in IntegrationCreatedServices)
                _ = ExecuteAsync((service, e) => service.OnIntegrationCreated(e), service, e);

            return default;
        }

        public ValueTask HandleIntegrationUpdated(object sender, IntegrationUpdatedEventArgs e)
        {
            foreach (var service in IntegrationUpdatedServices)
                _ = ExecuteAsync((service, e) => service.OnIntegrationUpdated(e), service, e);

            return default;
        }

        public ValueTask HandleIntegrationDeleted(object sender, IntegrationDeletedEventArgs e)
        {
            foreach (var service in IntegrationDeletedServices)
                _ = ExecuteAsync((service, e) => service.OnIntegrationDeleted(e), service, e);

            return default;
        }

        public ValueTask HandleInteractionReceived(object sender, InteractionReceivedEventArgs e)
        {
            foreach (var service in InteractionReceivedServices)
                _ = ExecuteAsync((service, e) => service.OnInteractionReceived(e), service, e);

            return default;
        }

        public ValueTask HandleInviteCreated(object sender, InviteCreatedEventArgs e)
        {
            foreach (var service in InviteCreatedServices)
                _ = ExecuteAsync((service, e) => service.OnInviteCreated(e), service, e);

			return default;
        }

        public ValueTask HandleInviteDeleted(object sender, InviteDeletedEventArgs e)
        {
            foreach (var service in InviteDeletedServices)
                _ = ExecuteAsync((service, e) => service.OnInviteDeleted(e), service, e);

			return default;
        }

        public ValueTask HandleMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            foreach (var service in MessageReceivedServices)
                _ = ExecuteAsync((service, e) => service.OnMessageReceived(e), service, e);

			return default;
        }

        public ValueTask HandleMessageUpdated(object sender, MessageUpdatedEventArgs e)
        {
            foreach (var service in MessageUpdatedServices)
                _ = ExecuteAsync((service, e) => service.OnMessageUpdated(e), service, e);

			return default;
        }

        public ValueTask HandleMessageDeleted(object sender, MessageDeletedEventArgs e)
        {
            foreach (var service in MessageDeletedServices)
                _ = ExecuteAsync((service, e) => service.OnMessageDeleted(e), service, e);

			return default;
        }

        public ValueTask HandleMessagesDeleted(object sender, MessagesDeletedEventArgs e)
        {
            foreach (var service in MessagesDeletedServices)
                _ = ExecuteAsync((service, e) => service.OnMessagesDeleted(e), service, e);

			return default;
        }

        public ValueTask HandleReactionAdded(object sender, ReactionAddedEventArgs e)
        {
            foreach (var service in ReactionAddedServices)
                _ = ExecuteAsync((service, e) => service.OnReactionAdded(e), service, e);

			return default;
        }

        public ValueTask HandleReactionRemoved(object sender, ReactionRemovedEventArgs e)
        {
            foreach (var service in ReactionRemovedServices)
                _ = ExecuteAsync((service, e) => service.OnReactionRemoved(e), service, e);

			return default;
        }

        public ValueTask HandleReactionsCleared(object sender, ReactionsClearedEventArgs e)
        {
            foreach (var service in ReactionsClearedServices)
                _ = ExecuteAsync((service, e) => service.OnReactionsCleared(e), service, e);

			return default;
        }

        public ValueTask HandlePresenceUpdated(object sender, PresenceUpdatedEventArgs e)
        {
            foreach (var service in PresenceUpdatedServices)
                _ = ExecuteAsync((service, e) => service.OnPresenceUpdated(e), service, e);

            return default;
        }

        public ValueTask HandleTypingStarted(object sender, TypingStartedEventArgs e)
        {
            foreach (var service in TypingStartedServices)
                _ = ExecuteAsync((service, e) => service.OnTypingStarted(e), service, e);

			return default;
        }

        public ValueTask HandleCurrentUserUpdated(object sender, CurrentUserUpdatedEventArgs e)
        {
            foreach (var service in CurrentUserUpdatedServices)
                _ = ExecuteAsync((service, e) => service.OnCurrentUserUpdated(e), service, e);

			return default;
        }

        public ValueTask HandleVoiceStateUpdated(object sender, VoiceStateUpdatedEventArgs e)
        {
            foreach (var service in VoiceStateUpdatedServices)
                _ = ExecuteAsync((service, e) => service.OnVoiceStateUpdated(e), service, e);

			return default;
        }

        public ValueTask HandleVoiceServerUpdated(object sender, VoiceServerUpdatedEventArgs e)
        {
            foreach (var service in VoiceServerUpdatedServices)
                _ = ExecuteAsync((service, e) => service.OnVoiceServerUpdated(e), service, e);

			return default;
        }

        public ValueTask HandleWebhooksUpdated(object sender, WebhooksUpdatedEventArgs e)
        {
            foreach (var service in WebhooksUpdatedServices)
                _ = ExecuteAsync((service, e) => service.OnWebhooksUpdated(e), service, e);

			return default;
        }
    }
}

