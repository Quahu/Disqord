using System.Threading.Tasks;
using Disqord.Gateway;

namespace Disqord.Hosting;

public abstract partial class DiscordClientService
{
    /// <inheritdoc cref="IGatewayClient.Ready"/>
    protected internal virtual ValueTask OnReady(ReadyEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.ApplicationCommandPermissionsUpdated"/>
    protected internal virtual ValueTask OnApplicationCommandPermissionsUpdated(ApplicationCommandPermissionsUpdatedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.AutoModerationRuleCreated"/>
    protected internal virtual ValueTask OnAutoModerationRuleCreated(AutoModerationRuleCreatedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.AutoModerationRuleUpdated"/>
    protected internal virtual ValueTask OnAutoModerationRuleUpdated(AutoModerationRuleUpdatedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.AutoModerationRuleDeleted"/>
    protected internal virtual ValueTask OnAutoModerationRuleDeleted(AutoModerationRuleDeletedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.AutoModerationActionExecuted"/>
    protected internal virtual ValueTask OnAutoModerationActionExecuted(AutoModerationActionExecutedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.ChannelCreated"/>
    protected internal virtual ValueTask OnChannelCreated(ChannelCreatedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.ChannelUpdated"/>
    protected internal virtual ValueTask OnChannelUpdated(ChannelUpdatedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.ChannelDeleted"/>
    protected internal virtual ValueTask OnChannelDeleted(ChannelDeletedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.ThreadCreated"/>
    protected internal virtual ValueTask OnThreadCreated(ThreadCreatedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.ThreadUpdated"/>
    protected internal virtual ValueTask OnThreadUpdated(ThreadUpdatedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.ThreadDeleted"/>
    protected internal virtual ValueTask OnThreadDeleted(ThreadDeletedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.ThreadsSynchronized"/>
    protected internal virtual ValueTask OnThreadsSynchronized(ThreadsSynchronizedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.ThreadMembersUpdated"/>
    protected internal virtual ValueTask OnThreadMembersUpdated(ThreadMembersUpdatedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.ChannelPinsUpdated"/>
    protected internal virtual ValueTask OnChannelPinsUpdated(ChannelPinsUpdatedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.GuildAvailable"/>
    protected internal virtual ValueTask OnGuildAvailable(GuildAvailableEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.JoinedGuild"/>
    protected internal virtual ValueTask OnJoinedGuild(JoinedGuildEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.GuildUpdated"/>
    protected internal virtual ValueTask OnGuildUpdated(GuildUpdatedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.GuildUnavailable"/>
    protected internal virtual ValueTask OnGuildUnavailable(GuildUnavailableEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.LeftGuild"/>
    protected internal virtual ValueTask OnLeftGuild(LeftGuildEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.BanCreated"/>
    protected internal virtual ValueTask OnBanCreated(BanCreatedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.BanDeleted"/>
    protected internal virtual ValueTask OnBanDeleted(BanDeletedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.EmojisUpdated"/>
    protected internal virtual ValueTask OnEmojisUpdated(EmojisUpdatedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.StickersUpdated"/>
    protected internal virtual ValueTask OnStickersUpdated(StickersUpdatedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.IntegrationsUpdated"/>
    protected internal virtual ValueTask OnIntegrationsUpdated(IntegrationsUpdatedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.MemberJoined"/>
    protected internal virtual ValueTask OnMemberJoined(MemberJoinedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.MemberUpdated"/>
    protected internal virtual ValueTask OnMemberUpdated(MemberUpdatedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.MemberLeft"/>
    protected internal virtual ValueTask OnMemberLeft(MemberLeftEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.RoleCreated"/>
    protected internal virtual ValueTask OnRoleCreated(RoleCreatedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.RoleUpdated"/>
    protected internal virtual ValueTask OnRoleUpdated(RoleUpdatedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.RoleDeleted"/>
    protected internal virtual ValueTask OnRoleDeleted(RoleDeletedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.GuildEventCreated"/>
    protected internal virtual ValueTask OnGuildEventCreated(GuildEventCreatedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.GuildEventUpdated"/>
    protected internal virtual ValueTask OnGuildEventUpdated(GuildEventUpdatedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.GuildEventDeleted"/>
    protected internal virtual ValueTask OnGuildEventDeleted(GuildEventDeletedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.GuildEventMemberAdded"/>
    protected internal virtual ValueTask OnGuildEventMemberAdded(GuildEventMemberAddedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.GuildEventMemberRemoved"/>
    protected internal virtual ValueTask OnGuildEventMemberRemoved(GuildEventMemberRemovedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.IntegrationCreated"/>
    protected internal virtual ValueTask OnIntegrationCreated(IntegrationCreatedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.IntegrationUpdated"/>
    protected internal virtual ValueTask OnIntegrationUpdated(IntegrationUpdatedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.IntegrationDeleted"/>
    protected internal virtual ValueTask OnIntegrationDeleted(IntegrationDeletedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.InteractionReceived"/>
    protected internal virtual ValueTask OnInteractionReceived(InteractionReceivedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.InviteCreated"/>
    protected internal virtual ValueTask OnInviteCreated(InviteCreatedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.InviteDeleted"/>
    protected internal virtual ValueTask OnInviteDeleted(InviteDeletedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.MessageReceived"/>
    protected internal virtual ValueTask OnMessageReceived(MessageReceivedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.MessageUpdated"/>
    protected internal virtual ValueTask OnMessageUpdated(MessageUpdatedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.MessageDeleted"/>
    protected internal virtual ValueTask OnMessageDeleted(MessageDeletedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.MessagesDeleted"/>
    protected internal virtual ValueTask OnMessagesDeleted(MessagesDeletedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.ReactionAdded"/>
    protected internal virtual ValueTask OnReactionAdded(ReactionAddedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.ReactionRemoved"/>
    protected internal virtual ValueTask OnReactionRemoved(ReactionRemovedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.ReactionsCleared"/>
    protected internal virtual ValueTask OnReactionsCleared(ReactionsClearedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.PresenceUpdated"/>
    protected internal virtual ValueTask OnPresenceUpdated(PresenceUpdatedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.StageCreated"/>
    protected internal virtual ValueTask OnStageCreated(StageCreatedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.StageUpdated"/>
    protected internal virtual ValueTask OnStageUpdated(StageUpdatedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.StageDeleted"/>
    protected internal virtual ValueTask OnStageDeleted(StageDeletedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.TypingStarted"/>
    protected internal virtual ValueTask OnTypingStarted(TypingStartedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.CurrentUserUpdated"/>
    protected internal virtual ValueTask OnCurrentUserUpdated(CurrentUserUpdatedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.VoiceStateUpdated"/>
    protected internal virtual ValueTask OnVoiceStateUpdated(VoiceStateUpdatedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.VoiceServerUpdated"/>
    protected internal virtual ValueTask OnVoiceServerUpdated(VoiceServerUpdatedEventArgs e)
        => default;

    /// <inheritdoc cref="IGatewayClient.WebhooksUpdated"/>
    protected internal virtual ValueTask OnWebhooksUpdated(WebhooksUpdatedEventArgs e)
        => default;
}