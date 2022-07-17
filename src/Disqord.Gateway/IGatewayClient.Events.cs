using Qommon.Events;

namespace Disqord.Gateway;

public partial interface IGatewayClient
{
    /// <summary>
    ///     Fires when the client is ready, i.e. identified and ready to process gateway dispatches.
    /// </summary>
    /// <remarks>
    ///     The behavior of this event can be controlled via <see cref="ReadyEventDelayMode"/>.
    /// </remarks>
    event AsynchronousEventHandler<ReadyEventArgs> Ready;

    /// <summary>
    ///     Fires when an application command's guild permissions are updated.
    /// </summary>
    event AsynchronousEventHandler<ApplicationCommandPermissionsUpdatedEventArgs> ApplicationCommandPermissionsUpdated;

    /// <summary>
    ///     Fires when an auto-moderation rule is created.
    /// </summary>
    event AsynchronousEventHandler<AutoModerationRuleCreatedEventArgs> AutoModerationRuleCreated;

    /// <summary>
    ///     Fires when an auto-moderation rule is updated.
    /// </summary>
    event AsynchronousEventHandler<AutoModerationRuleUpdatedEventArgs> AutoModerationRuleUpdated;

    /// <summary>
    ///     Fires when an auto-moderation rule is deleted.
    /// </summary>
    event AsynchronousEventHandler<AutoModerationRuleDeletedEventArgs> AutoModerationRuleDeleted;

    /// <summary>
    ///     Fires when an auto-moderation action is executed.
    /// </summary>
    event AsynchronousEventHandler<AutoModerationActionExecutedEventArgs> AutoModerationActionExecuted;

    /// <summary>
    ///     Fires when a guild channel is created.
    /// </summary>
    event AsynchronousEventHandler<ChannelCreatedEventArgs> ChannelCreated;

    /// <summary>
    ///     Fires when a guild channel is updated.
    /// </summary>
    event AsynchronousEventHandler<ChannelUpdatedEventArgs> ChannelUpdated;

    /// <summary>
    ///     Fires when a guild channel is deleted.
    /// </summary>
    event AsynchronousEventHandler<ChannelDeletedEventArgs> ChannelDeleted;

    /// <summary>
    ///     Fires when a thread is created or when the bot gains access to a new thread.
    /// </summary>
    /// <remarks>
    ///     This only triggers for threads the bot can view.
    /// </remarks>
    event AsynchronousEventHandler<ThreadCreatedEventArgs> ThreadCreated;

    /// <summary>
    ///     Fires when a thread is updated.
    /// </summary>
    /// <remarks>
    ///     This only triggers for threads the bot can view.
    /// </remarks>
    event AsynchronousEventHandler<ThreadUpdatedEventArgs> ThreadUpdated;

    /// <summary>
    ///     Fires when a thread is deleted.
    /// </summary>
    /// <remarks>
    ///     This only triggers for threads the bot can view.
    /// </remarks>
    event AsynchronousEventHandler<ThreadDeletedEventArgs> ThreadDeleted;

    /// <summary>
    ///     Fires when the bot gains access to a new channel.
    /// </summary>
    event AsynchronousEventHandler<ThreadsSynchronizedEventArgs> ThreadsSynchronized;

    /// <summary>
    ///     Fires when members are added and/or removed from a thread.
    /// </summary>
    /// <remarks>
    ///     This only triggers for threads the bot can view.
    /// </remarks>
    event AsynchronousEventHandler<ThreadMembersUpdatedEventArgs> ThreadMembersUpdated;

    /// <summary>
    ///     Fires when the pinned messages in a channel are updated.
    /// </summary>
    event AsynchronousEventHandler<ChannelPinsUpdatedEventArgs> ChannelPinsUpdated;

    /// <summary>
    ///     Fires when a guild becomes available.
    /// </summary>
    event AsynchronousEventHandler<GuildAvailableEventArgs> GuildAvailable;

    /// <summary>
    ///     Fires when the bot joins a guild.
    /// </summary>
    event AsynchronousEventHandler<JoinedGuildEventArgs> JoinedGuild;

    /// <summary>
    ///     Fires when a guild is updated.
    /// </summary>
    event AsynchronousEventHandler<GuildUpdatedEventArgs> GuildUpdated;

    /// <summary>
    ///     Fires when a guild becomes unavailable.
    /// </summary>
    event AsynchronousEventHandler<GuildUnavailableEventArgs> GuildUnavailable;

    /// <summary>
    ///     Fires when the bot leaves a guild.
    ///     This also fires when the bot is kicked from a guild.
    /// </summary>
    event AsynchronousEventHandler<LeftGuildEventArgs> LeftGuild;

    /// <summary>
    ///     Fires when a guild ban is created.
    /// </summary>
    event AsynchronousEventHandler<BanCreatedEventArgs> BanCreated;

    /// <summary>
    ///     Fires when a guild ban is deleted.
    /// </summary>
    event AsynchronousEventHandler<BanDeletedEventArgs> BanDeleted;

    /// <summary>
    ///     Fires when the emojis in a guild are updated.
    /// </summary>
    event AsynchronousEventHandler<EmojisUpdatedEventArgs> EmojisUpdated;

    /// <summary>
    ///     Fires when the stickers in a guild are updated.
    /// </summary>
    event AsynchronousEventHandler<StickersUpdatedEventArgs> StickersUpdated;

    /// <summary>
    ///     Fires when the integrations in a guild are updated.
    /// </summary>
    event AsynchronousEventHandler<IntegrationsUpdatedEventArgs> IntegrationsUpdated;

    /// <summary>
    ///     Fires when a member joins a guild.
    /// </summary>
    event AsynchronousEventHandler<MemberJoinedEventArgs> MemberJoined;

    /// <summary>
    ///     Fires when a member is updated.
    /// </summary>
    event AsynchronousEventHandler<MemberUpdatedEventArgs> MemberUpdated;

    /// <summary>
    ///     Fires when a member leaves a guild.
    /// </summary>
    event AsynchronousEventHandler<MemberLeftEventArgs> MemberLeft;

    /// <summary>
    ///     Fires when a guild role is created.
    /// </summary>
    event AsynchronousEventHandler<RoleCreatedEventArgs> RoleCreated;

    /// <summary>
    ///     Fires when a guild role is updated.
    /// </summary>
    event AsynchronousEventHandler<RoleUpdatedEventArgs> RoleUpdated;

    /// <summary>
    ///     Fires when a guild role is deleted.
    /// </summary>
    event AsynchronousEventHandler<RoleDeletedEventArgs> RoleDeleted;

    /// <summary>
    ///     Fires when a guild event is created.
    /// </summary>
    event AsynchronousEventHandler<GuildEventCreatedEventArgs> GuildEventCreated;

    /// <summary>
    ///     Fires when a guild event is updated.
    /// </summary>
    event AsynchronousEventHandler<GuildEventUpdatedEventArgs> GuildEventUpdated;

    /// <summary>
    ///     Fires when a guild event is deleted.
    /// </summary>
    event AsynchronousEventHandler<GuildEventDeletedEventArgs> GuildEventDeleted;

    /// <summary>
    ///     Fires when a user subscribes to a guild event.
    /// </summary>
    event AsynchronousEventHandler<GuildEventMemberAddedEventArgs> GuildEventMemberAdded;

    /// <summary>
    ///     Fires when a user unsubscribes from a guild event.
    /// </summary>
    event AsynchronousEventHandler<GuildEventMemberRemovedEventArgs> GuildEventMemberRemoved;

    /// <summary>
    ///     Fires when a guild integration is created.
    /// </summary>
    event AsynchronousEventHandler<IntegrationCreatedEventArgs> IntegrationCreated;

    /// <summary>
    ///     Fires when a guild integration is updated.
    /// </summary>
    event AsynchronousEventHandler<IntegrationUpdatedEventArgs> IntegrationUpdated;

    /// <summary>
    ///     Fires when a guild integration is deleted.
    /// </summary>
    event AsynchronousEventHandler<IntegrationDeletedEventArgs> IntegrationDeleted;

    /// <summary>
    ///     Fires when an interaction is triggered.
    /// </summary>
    event AsynchronousEventHandler<InteractionReceivedEventArgs> InteractionReceived;

    /// <summary>
    ///     Fires when an invite is created.
    /// </summary>
    event AsynchronousEventHandler<InviteCreatedEventArgs> InviteCreated;

    /// <summary>
    ///     Fires when an invite is deleted.
    /// </summary>
    event AsynchronousEventHandler<InviteDeletedEventArgs> InviteDeleted;

    /// <summary>
    ///     Fires when a message is received.
    /// </summary>
    event AsynchronousEventHandler<MessageReceivedEventArgs> MessageReceived;

    /// <summary>
    ///     Fires when a message is updated.
    /// </summary>
    event AsynchronousEventHandler<MessageUpdatedEventArgs> MessageUpdated;

    /// <summary>
    ///     Fires when a message is deleted.
    /// </summary>
    event AsynchronousEventHandler<MessageDeletedEventArgs> MessageDeleted;

    /// <summary>
    ///     Fires when messages are bulk deleted in a text channel.
    /// </summary>
    event AsynchronousEventHandler<MessagesDeletedEventArgs> MessagesDeleted;

    /// <summary>
    ///     Fires when a reaction is added to a message.
    /// </summary>
    event AsynchronousEventHandler<ReactionAddedEventArgs> ReactionAdded;

    /// <summary>
    ///     Fires when a reaction is removed from a message.
    /// </summary>
    event AsynchronousEventHandler<ReactionRemovedEventArgs> ReactionRemoved;

    /// <summary>
    ///     Fires when reactions are cleared from a message.
    /// </summary>
    event AsynchronousEventHandler<ReactionsClearedEventArgs> ReactionsCleared;

    /// <summary>
    ///     Fires when a member's presence is updated.
    /// </summary>
    event AsynchronousEventHandler<PresenceUpdatedEventArgs> PresenceUpdated;

    /// <summary>
    ///     Fires when a stage is created.
    /// </summary>
    event AsynchronousEventHandler<StageCreatedEventArgs> StageCreated;

    /// <summary>
    ///     Fires when a stage is updated.
    /// </summary>
    event AsynchronousEventHandler<StageUpdatedEventArgs> StageUpdated;

    /// <summary>
    ///     Fires when a stage is deleted.
    /// </summary>
    event AsynchronousEventHandler<StageDeletedEventArgs> StageDeleted;

    /// <summary>
    ///     Fires when a user starts typing in a channel.
    /// </summary>
    event AsynchronousEventHandler<TypingStartedEventArgs> TypingStarted;

    /// <summary>
    ///     Fires when the current user is updated.
    /// </summary>
    event AsynchronousEventHandler<CurrentUserUpdatedEventArgs> CurrentUserUpdated;

    /// <summary>
    ///     Fires when a member's voice state is updated.
    /// </summary>
    event AsynchronousEventHandler<VoiceStateUpdatedEventArgs> VoiceStateUpdated;

    /// <summary>
    ///     Fires when the voice server of the voice channel the bot is in is updated.
    /// </summary>
    event AsynchronousEventHandler<VoiceServerUpdatedEventArgs> VoiceServerUpdated;

    /// <summary>
    ///     Fires when the webhooks in a guild channel are updated.
    /// </summary>
    event AsynchronousEventHandler<WebhooksUpdatedEventArgs> WebhooksUpdated;
}
