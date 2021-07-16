﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Collections;
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
        public DiscordClientService[] StageCreatedServices { get; }
        public DiscordClientService[] StageUpdatedServices { get; }
        public DiscordClientService[] StageDeletedServices { get; }
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

            var servicesArray = services.GetArray();
            ReadyServices = GetServices<ReadyEventArgs>(servicesArray, nameof(DiscordClientService.OnReady));
            ChannelCreatedServices = GetServices<ChannelCreatedEventArgs>(servicesArray, nameof(DiscordClientService.OnChannelCreated));
            ChannelUpdatedServices = GetServices<ChannelUpdatedEventArgs>(servicesArray, nameof(DiscordClientService.OnChannelUpdated));
            ChannelDeletedServices = GetServices<ChannelDeletedEventArgs>(servicesArray, nameof(DiscordClientService.OnChannelDeleted));
            ChannelPinsUpdatedServices = GetServices<ChannelPinsUpdatedEventArgs>(servicesArray, nameof(DiscordClientService.OnChannelPinsUpdated));
            GuildAvailableServices = GetServices<GuildAvailableEventArgs>(servicesArray, nameof(DiscordClientService.OnGuildAvailable));
            JoinedGuildServices = GetServices<JoinedGuildEventArgs>(servicesArray, nameof(DiscordClientService.OnJoinedGuild));
            GuildUpdatedServices = GetServices<GuildUpdatedEventArgs>(servicesArray, nameof(DiscordClientService.OnGuildUpdated));
            GuildUnavailableServices = GetServices<GuildUnavailableEventArgs>(servicesArray, nameof(DiscordClientService.OnGuildUnavailable));
            LeftGuildServices = GetServices<LeftGuildEventArgs>(servicesArray, nameof(DiscordClientService.OnLeftGuild));
            BanCreatedServices = GetServices<BanCreatedEventArgs>(servicesArray, nameof(DiscordClientService.OnBanCreated));
            BanDeletedServices = GetServices<BanDeletedEventArgs>(servicesArray, nameof(DiscordClientService.OnBanDeleted));
            EmojisUpdatedServices = GetServices<EmojisUpdatedEventArgs>(servicesArray, nameof(DiscordClientService.OnEmojisUpdated));
            IntegrationsUpdatedServices = GetServices<IntegrationsUpdatedEventArgs>(servicesArray, nameof(DiscordClientService.OnIntegrationsUpdated));
            MemberJoinedServices = GetServices<MemberJoinedEventArgs>(servicesArray, nameof(DiscordClientService.OnMemberJoined));
            MemberUpdatedServices = GetServices<MemberUpdatedEventArgs>(servicesArray, nameof(DiscordClientService.OnMemberUpdated));
            MemberLeftServices = GetServices<MemberLeftEventArgs>(servicesArray, nameof(DiscordClientService.OnMemberLeft));
            RoleCreatedServices = GetServices<RoleCreatedEventArgs>(servicesArray, nameof(DiscordClientService.OnRoleCreated));
            RoleUpdatedServices = GetServices<RoleUpdatedEventArgs>(servicesArray, nameof(DiscordClientService.OnRoleUpdated));
            RoleDeletedServices = GetServices<RoleDeletedEventArgs>(servicesArray, nameof(DiscordClientService.OnRoleDeleted));
            IntegrationCreatedServices = GetServices<IntegrationCreatedEventArgs>(servicesArray, nameof(DiscordClientService.OnIntegrationCreated));
            IntegrationUpdatedServices = GetServices<IntegrationUpdatedEventArgs>(servicesArray, nameof(DiscordClientService.OnIntegrationUpdated));
            IntegrationDeletedServices = GetServices<IntegrationDeletedEventArgs>(servicesArray, nameof(DiscordClientService.OnIntegrationDeleted));
            InteractionReceivedServices = GetServices<InteractionReceivedEventArgs>(servicesArray, nameof(DiscordClientService.OnInteractionReceived));
            InviteCreatedServices = GetServices<InviteCreatedEventArgs>(servicesArray, nameof(DiscordClientService.OnInviteCreated));
            InviteDeletedServices = GetServices<InviteDeletedEventArgs>(servicesArray, nameof(DiscordClientService.OnInviteDeleted));
            MessageReceivedServices = GetServices<MessageReceivedEventArgs>(servicesArray, nameof(DiscordClientService.OnMessageReceived));
            MessageUpdatedServices = GetServices<MessageUpdatedEventArgs>(servicesArray, nameof(DiscordClientService.OnMessageUpdated));
            MessageDeletedServices = GetServices<MessageDeletedEventArgs>(servicesArray, nameof(DiscordClientService.OnMessageDeleted));
            MessagesDeletedServices = GetServices<MessagesDeletedEventArgs>(servicesArray, nameof(DiscordClientService.OnMessagesDeleted));
            ReactionAddedServices = GetServices<ReactionAddedEventArgs>(servicesArray, nameof(DiscordClientService.OnReactionAdded));
            ReactionRemovedServices = GetServices<ReactionRemovedEventArgs>(servicesArray, nameof(DiscordClientService.OnReactionRemoved));
            ReactionsClearedServices = GetServices<ReactionsClearedEventArgs>(servicesArray, nameof(DiscordClientService.OnReactionsCleared));
            StageCreatedServices = GetServices<StageCreatedEventArgs>(servicesArray, nameof(DiscordClientService.OnStageCreated));
            StageUpdatedServices = GetServices<StageUpdatedEventArgs>(servicesArray, nameof(DiscordClientService.OnStageUpdated));
            StageDeletedServices = GetServices<StageDeletedEventArgs>(servicesArray, nameof(DiscordClientService.OnStageDeleted));
            PresenceUpdatedServices = GetServices<PresenceUpdatedEventArgs>(servicesArray, nameof(DiscordClientService.OnPresenceUpdated));
            TypingStartedServices = GetServices<TypingStartedEventArgs>(servicesArray, nameof(DiscordClientService.OnTypingStarted));
            CurrentUserUpdatedServices = GetServices<CurrentUserUpdatedEventArgs>(servicesArray, nameof(DiscordClientService.OnCurrentUserUpdated));
            VoiceStateUpdatedServices = GetServices<VoiceStateUpdatedEventArgs>(servicesArray, nameof(DiscordClientService.OnVoiceStateUpdated));
            VoiceServerUpdatedServices = GetServices<VoiceServerUpdatedEventArgs>(servicesArray, nameof(DiscordClientService.OnVoiceServerUpdated));
            WebhooksUpdatedServices = GetServices<WebhooksUpdatedEventArgs>(servicesArray, nameof(DiscordClientService.OnWebhooksUpdated));

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
            Client.StageCreated += HandleStageCreated;
            Client.StageUpdated += HandleStageUpdated;
            Client.StageDeleted += HandleStageDeleted;
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

        public async ValueTask HandleChannelCreated(object sender, ChannelCreatedEventArgs e)
        {
            foreach (var service in ChannelCreatedServices)
                await ExecuteAsync((service, e) => service.OnChannelCreated(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleChannelUpdated(object sender, ChannelUpdatedEventArgs e)
        {
            foreach (var service in ChannelUpdatedServices)
                await ExecuteAsync((service, e) => service.OnChannelUpdated(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleChannelDeleted(object sender, ChannelDeletedEventArgs e)
        {
            foreach (var service in ChannelDeletedServices)
                await ExecuteAsync((service, e) => service.OnChannelDeleted(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleChannelPinsUpdated(object sender, ChannelPinsUpdatedEventArgs e)
        {
            foreach (var service in ChannelPinsUpdatedServices)
                await ExecuteAsync((service, e) => service.OnChannelPinsUpdated(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleGuildAvailable(object sender, GuildAvailableEventArgs e)
        {
            foreach (var service in GuildAvailableServices)
                await ExecuteAsync((service, e) => service.OnGuildAvailable(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleJoinedGuild(object sender, JoinedGuildEventArgs e)
        {
            foreach (var service in JoinedGuildServices)
                await ExecuteAsync((service, e) => service.OnJoinedGuild(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleGuildUpdated(object sender, GuildUpdatedEventArgs e)
        {
            foreach (var service in GuildUpdatedServices)
                await ExecuteAsync((service, e) => service.OnGuildUpdated(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleGuildUnavailable(object sender, GuildUnavailableEventArgs e)
        {
            foreach (var service in GuildUnavailableServices)
                await ExecuteAsync((service, e) => service.OnGuildUnavailable(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleLeftGuild(object sender, LeftGuildEventArgs e)
        {
            foreach (var service in LeftGuildServices)
                await ExecuteAsync((service, e) => service.OnLeftGuild(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleBanCreated(object sender, BanCreatedEventArgs e)
        {
            foreach (var service in BanCreatedServices)
                await ExecuteAsync((service, e) => service.OnBanCreated(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleBanDeleted(object sender, BanDeletedEventArgs e)
        {
            foreach (var service in BanDeletedServices)
                await ExecuteAsync((service, e) => service.OnBanDeleted(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleEmojisUpdated(object sender, EmojisUpdatedEventArgs e)
        {
            foreach (var service in EmojisUpdatedServices)
                await ExecuteAsync((service, e) => service.OnEmojisUpdated(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleIntegrationsUpdated(object sender, IntegrationsUpdatedEventArgs e)
        {
            foreach (var service in IntegrationsUpdatedServices)
                await ExecuteAsync((service, e) => service.OnIntegrationsUpdated(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleMemberJoined(object sender, MemberJoinedEventArgs e)
        {
            foreach (var service in MemberJoinedServices)
                await ExecuteAsync((service, e) => service.OnMemberJoined(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleMemberUpdated(object sender, MemberUpdatedEventArgs e)
        {
            foreach (var service in MemberUpdatedServices)
                await ExecuteAsync((service, e) => service.OnMemberUpdated(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleMemberLeft(object sender, MemberLeftEventArgs e)
        {
            foreach (var service in MemberLeftServices)
                await ExecuteAsync((service, e) => service.OnMemberLeft(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleRoleCreated(object sender, RoleCreatedEventArgs e)
        {
            foreach (var service in RoleCreatedServices)
                await ExecuteAsync((service, e) => service.OnRoleCreated(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleRoleUpdated(object sender, RoleUpdatedEventArgs e)
        {
            foreach (var service in RoleUpdatedServices)
                await ExecuteAsync((service, e) => service.OnRoleUpdated(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleRoleDeleted(object sender, RoleDeletedEventArgs e)
        {
            foreach (var service in RoleDeletedServices)
                await ExecuteAsync((service, e) => service.OnRoleDeleted(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleIntegrationCreated(object sender, IntegrationCreatedEventArgs e)
        {
            foreach (var service in IntegrationCreatedServices)
                await ExecuteAsync((service, e) => service.OnIntegrationCreated(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleIntegrationUpdated(object sender, IntegrationUpdatedEventArgs e)
        {
            foreach (var service in IntegrationUpdatedServices)
                await ExecuteAsync((service, e) => service.OnIntegrationUpdated(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleIntegrationDeleted(object sender, IntegrationDeletedEventArgs e)
        {
            foreach (var service in IntegrationDeletedServices)
                await ExecuteAsync((service, e) => service.OnIntegrationDeleted(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleInteractionReceived(object sender, InteractionReceivedEventArgs e)
        {
            foreach (var service in InteractionReceivedServices)
                await ExecuteAsync((service, e) => service.OnInteractionReceived(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleInviteCreated(object sender, InviteCreatedEventArgs e)
        {
            foreach (var service in InviteCreatedServices)
                await ExecuteAsync((service, e) => service.OnInviteCreated(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleInviteDeleted(object sender, InviteDeletedEventArgs e)
        {
            foreach (var service in InviteDeletedServices)
                await ExecuteAsync((service, e) => service.OnInviteDeleted(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            foreach (var service in MessageReceivedServices)
                await ExecuteAsync((service, e) => service.OnMessageReceived(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleMessageUpdated(object sender, MessageUpdatedEventArgs e)
        {
            foreach (var service in MessageUpdatedServices)
                await ExecuteAsync((service, e) => service.OnMessageUpdated(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleMessageDeleted(object sender, MessageDeletedEventArgs e)
        {
            foreach (var service in MessageDeletedServices)
                await ExecuteAsync((service, e) => service.OnMessageDeleted(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleMessagesDeleted(object sender, MessagesDeletedEventArgs e)
        {
            foreach (var service in MessagesDeletedServices)
                await ExecuteAsync((service, e) => service.OnMessagesDeleted(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleReactionAdded(object sender, ReactionAddedEventArgs e)
        {
            foreach (var service in ReactionAddedServices)
                await ExecuteAsync((service, e) => service.OnReactionAdded(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleReactionRemoved(object sender, ReactionRemovedEventArgs e)
        {
            foreach (var service in ReactionRemovedServices)
                await ExecuteAsync((service, e) => service.OnReactionRemoved(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleReactionsCleared(object sender, ReactionsClearedEventArgs e)
        {
            foreach (var service in ReactionsClearedServices)
                await ExecuteAsync((service, e) => service.OnReactionsCleared(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandlePresenceUpdated(object sender, PresenceUpdatedEventArgs e)
        {
            foreach (var service in PresenceUpdatedServices)
                await ExecuteAsync((service, e) => service.OnPresenceUpdated(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleStageCreated(object sender, StageCreatedEventArgs e)
        {
            foreach (var service in StageCreatedServices)
                await ExecuteAsync((service, e) => service.OnStageCreated(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleStageUpdated(object sender, StageUpdatedEventArgs e)
        {
            foreach (var service in StageUpdatedServices)
                await ExecuteAsync((service, e) => service.OnStageUpdated(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleStageDeleted(object sender, StageDeletedEventArgs e)
        {
            foreach (var service in StageDeletedServices)
                await ExecuteAsync((service, e) => service.OnStageDeleted(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleTypingStarted(object sender, TypingStartedEventArgs e)
        {
            foreach (var service in TypingStartedServices)
                await ExecuteAsync((service, e) => service.OnTypingStarted(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleCurrentUserUpdated(object sender, CurrentUserUpdatedEventArgs e)
        {
            foreach (var service in CurrentUserUpdatedServices)
                await ExecuteAsync((service, e) => service.OnCurrentUserUpdated(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleVoiceStateUpdated(object sender, VoiceStateUpdatedEventArgs e)
        {
            foreach (var service in VoiceStateUpdatedServices)
                await ExecuteAsync((service, e) => service.OnVoiceStateUpdated(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleVoiceServerUpdated(object sender, VoiceServerUpdatedEventArgs e)
        {
            foreach (var service in VoiceServerUpdatedServices)
                await ExecuteAsync((service, e) => service.OnVoiceServerUpdated(e), service, e).ConfigureAwait(false);
        }

        public async ValueTask HandleWebhooksUpdated(object sender, WebhooksUpdatedEventArgs e)
        {
            foreach (var service in WebhooksUpdatedServices)
                await ExecuteAsync((service, e) => service.OnWebhooksUpdated(e), service, e).ConfigureAwait(false);
        }
    }
}

