using Disqord.Events;
using Qommon.Events;

namespace Disqord
{
    public abstract partial class DiscordClientBase : IDiscordClient
    {
        public event AsynchronousEventHandler<ReadyEventArgs> Ready
        {
            add => _ready.Hook(value);
            remove => _ready.Unhook(value);
        }
        internal readonly AsynchronousEvent<ReadyEventArgs> _ready = new AsynchronousEvent<ReadyEventArgs>();

        public event AsynchronousEventHandler<ChannelCreatedEventArgs> ChannelCreated
        {
            add => _channelCreated.Hook(value);
            remove => _channelCreated.Unhook(value);
        }
        internal readonly AsynchronousEvent<ChannelCreatedEventArgs> _channelCreated = new AsynchronousEvent<ChannelCreatedEventArgs>();

        public event AsynchronousEventHandler<ChannelUpdatedEventArgs> ChannelUpdated
        {
            add => _channelUpdated.Hook(value);
            remove => _channelUpdated.Unhook(value);
        }
        internal readonly AsynchronousEvent<ChannelUpdatedEventArgs> _channelUpdated = new AsynchronousEvent<ChannelUpdatedEventArgs>();

        public event AsynchronousEventHandler<ChannelDeletedEventArgs> ChannelDeleted
        {
            add => _channelDeleted.Hook(value);
            remove => _channelDeleted.Unhook(value);
        }
        internal readonly AsynchronousEvent<ChannelDeletedEventArgs> _channelDeleted = new AsynchronousEvent<ChannelDeletedEventArgs>();

        public event AsynchronousEventHandler<ChannelPinsUpdatedEventArgs> ChannelPinsUpdated
        {
            add => _channelPinsUpdated.Hook(value);
            remove => _channelPinsUpdated.Unhook(value);
        }
        internal readonly AsynchronousEvent<ChannelPinsUpdatedEventArgs> _channelPinsUpdated = new AsynchronousEvent<ChannelPinsUpdatedEventArgs>();

        public event AsynchronousEventHandler<GuildAvailableEventArgs> GuildAvailable
        {
            add => _guildAvailable.Hook(value);
            remove => _guildAvailable.Unhook(value);
        }
        internal readonly AsynchronousEvent<GuildAvailableEventArgs> _guildAvailable = new AsynchronousEvent<GuildAvailableEventArgs>();

        public event AsynchronousEventHandler<GuildUnavailableEventArgs> GuildUnavailable
        {
            add => _guildUnavailable.Hook(value);
            remove => _guildUnavailable.Unhook(value);
        }
        internal readonly AsynchronousEvent<GuildUnavailableEventArgs> _guildUnavailable = new AsynchronousEvent<GuildUnavailableEventArgs>();

        public event AsynchronousEventHandler<JoinedGuildEventArgs> JoinedGuild
        {
            add => _joinedGuild.Hook(value);
            remove => _joinedGuild.Unhook(value);
        }
        internal readonly AsynchronousEvent<JoinedGuildEventArgs> _joinedGuild = new AsynchronousEvent<JoinedGuildEventArgs>();

        public event AsynchronousEventHandler<JoinedGuildEventArgs> LeftGuild
        {
            add => _leftGuild.Hook(value);
            remove => _leftGuild.Unhook(value);
        }
        internal readonly AsynchronousEvent<JoinedGuildEventArgs> _leftGuild = new AsynchronousEvent<JoinedGuildEventArgs>();

        public event AsynchronousEventHandler<RoleCreatedEventArgs> RoleCreated
        {
            add => _roleCreated.Hook(value);
            remove => _roleCreated.Unhook(value);
        }
        internal readonly AsynchronousEvent<RoleCreatedEventArgs> _roleCreated = new AsynchronousEvent<RoleCreatedEventArgs>();

        public event AsynchronousEventHandler<RoleUpdatedEventArgs> RoleUpdated
        {
            add => _roleUpdated.Hook(value);
            remove => _roleUpdated.Unhook(value);
        }
        internal readonly AsynchronousEvent<RoleUpdatedEventArgs> _roleUpdated = new AsynchronousEvent<RoleUpdatedEventArgs>();

        public event AsynchronousEventHandler<RoleDeletedEventArgs> RoleDeleted
        {
            add => _roleDeleted.Hook(value);
            remove => _roleDeleted.Unhook(value);
        }
        internal readonly AsynchronousEvent<RoleDeletedEventArgs> _roleDeleted = new AsynchronousEvent<RoleDeletedEventArgs>();

        public event AsynchronousEventHandler<MemberBannedEventArgs> MemberBanned
        {
            add => _memberBanned.Hook(value);
            remove => _memberBanned.Unhook(value);
        }
        internal readonly AsynchronousEvent<MemberBannedEventArgs> _memberBanned = new AsynchronousEvent<MemberBannedEventArgs>();

        public event AsynchronousEventHandler<MemberUnbannedEventArgs> MemberUnbanned
        {
            add => _memberUnbanned.Hook(value);
            remove => _memberUnbanned.Unhook(value);
        }
        internal readonly AsynchronousEvent<MemberUnbannedEventArgs> _memberUnbanned = new AsynchronousEvent<MemberUnbannedEventArgs>();

        public event AsynchronousEventHandler<MemberJoinedEventArgs> MemberJoined
        {
            add => _memberJoined.Hook(value);
            remove => _memberJoined.Unhook(value);
        }
        internal readonly AsynchronousEvent<MemberJoinedEventArgs> _memberJoined = new AsynchronousEvent<MemberJoinedEventArgs>();

        public event AsynchronousEventHandler<MemberLeftEventArgs> MemberLeft
        {
            add => _memberLeft.Hook(value);
            remove => _memberLeft.Unhook(value);
        }
        internal readonly AsynchronousEvent<MemberLeftEventArgs> _memberLeft = new AsynchronousEvent<MemberLeftEventArgs>();

        public event AsynchronousEventHandler<MessageAcknowledgedEventArgs> MessageAcknowledged
        {
            add => _messageAcknowledged.Hook(value);
            remove => _messageAcknowledged.Unhook(value);
        }
        internal readonly AsynchronousEvent<MessageAcknowledgedEventArgs> _messageAcknowledged = new AsynchronousEvent<MessageAcknowledgedEventArgs>();

        public event AsynchronousEventHandler<MessageReceivedEventArgs> MessageReceived
        {
            add => _messageReceived.Hook(value);
            remove => _messageReceived.Unhook(value);
        }
        internal readonly AsynchronousEvent<MessageReceivedEventArgs> _messageReceived = new AsynchronousEvent<MessageReceivedEventArgs>();

        public event AsynchronousEventHandler<MessageDeletedEventArgs> MessageDeleted
        {
            add => _messageDeleted.Hook(value);
            remove => _messageDeleted.Unhook(value);
        }
        internal readonly AsynchronousEvent<MessageDeletedEventArgs> _messageDeleted = new AsynchronousEvent<MessageDeletedEventArgs>();

        public event AsynchronousEventHandler<MessageUpdatedEventArgs> MessageUpdated
        {
            add => _messageUpdated.Hook(value);
            remove => _messageUpdated.Unhook(value);
        }
        internal readonly AsynchronousEvent<MessageUpdatedEventArgs> _messageUpdated = new AsynchronousEvent<MessageUpdatedEventArgs>();

        public event AsynchronousEventHandler<ReactionAddedEventArgs> ReactionAdded
        {
            add => _reactionAdded.Hook(value);
            remove => _reactionAdded.Unhook(value);
        }
        internal readonly AsynchronousEvent<ReactionAddedEventArgs> _reactionAdded = new AsynchronousEvent<ReactionAddedEventArgs>();

        public event AsynchronousEventHandler<ReactionRemovedEventArgs> ReactionRemoved
        {
            add => _reactionRemoved.Hook(value);
            remove => _reactionRemoved.Unhook(value);
        }
        internal readonly AsynchronousEvent<ReactionRemovedEventArgs> _reactionRemoved = new AsynchronousEvent<ReactionRemovedEventArgs>();

        public event AsynchronousEventHandler<TypingStartedEventArgs> TypingStarted
        {
            add => _typingStarted.Hook(value);
            remove => _typingStarted.Unhook(value);
        }
        internal readonly AsynchronousEvent<TypingStartedEventArgs> _typingStarted = new AsynchronousEvent<TypingStartedEventArgs>();

        public event AsynchronousEventHandler<VoiceStateUpdatedEventArgs> VoiceStateUpdated
        {
            add => _voiceStateUpdatedEvent.Hook(value);
            remove => _voiceStateUpdatedEvent.Unhook(value);
        }
        internal readonly AsynchronousEvent<VoiceStateUpdatedEventArgs> _voiceStateUpdatedEvent = new AsynchronousEvent<VoiceStateUpdatedEventArgs>();

        public event AsynchronousEventHandler<VoiceServerUpdatedEventArgs> VoiceServerUpdated
        {
            add => _voiceServerUpdatedEvent.Hook(value);
            remove => _voiceServerUpdatedEvent.Unhook(value);
        }
        internal readonly AsynchronousEvent<VoiceServerUpdatedEventArgs> _voiceServerUpdatedEvent = new AsynchronousEvent<VoiceServerUpdatedEventArgs>();
    }
}
