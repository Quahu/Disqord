using System;
using Disqord.Events;
using Disqord.Rest;
using Qommon.Events;

namespace Disqord
{
    public abstract partial class DiscordClientBase : IRestDiscordClient, IAsyncDisposable
    {
        public event AsynchronousEventHandler<ReadyEventArgs> Ready
        {
            add => _ready.Hook(value);
            remove => _ready.Unhook(value);
        }
        internal readonly AsynchronousEvent<ReadyEventArgs> _ready;

        public event AsynchronousEventHandler<ChannelCreatedEventArgs> ChannelCreated
        {
            add => _channelCreated.Hook(value);
            remove => _channelCreated.Unhook(value);
        }
        internal readonly AsynchronousEvent<ChannelCreatedEventArgs> _channelCreated;

        public event AsynchronousEventHandler<ChannelUpdatedEventArgs> ChannelUpdated
        {
            add => _channelUpdated.Hook(value);
            remove => _channelUpdated.Unhook(value);
        }
        internal readonly AsynchronousEvent<ChannelUpdatedEventArgs> _channelUpdated;

        public event AsynchronousEventHandler<ChannelDeletedEventArgs> ChannelDeleted
        {
            add => _channelDeleted.Hook(value);
            remove => _channelDeleted.Unhook(value);
        }
        internal readonly AsynchronousEvent<ChannelDeletedEventArgs> _channelDeleted;

        public event AsynchronousEventHandler<ChannelPinsUpdatedEventArgs> ChannelPinsUpdated
        {
            add => _channelPinsUpdated.Hook(value);
            remove => _channelPinsUpdated.Unhook(value);
        }
        internal readonly AsynchronousEvent<ChannelPinsUpdatedEventArgs> _channelPinsUpdated;

        public event AsynchronousEventHandler<GuildAvailableEventArgs> GuildAvailable
        {
            add => _guildAvailable.Hook(value);
            remove => _guildAvailable.Unhook(value);
        }
        internal readonly AsynchronousEvent<GuildAvailableEventArgs> _guildAvailable;

        public event AsynchronousEventHandler<GuildUnavailableEventArgs> GuildUnavailable
        {
            add => _guildUnavailable.Hook(value);
            remove => _guildUnavailable.Unhook(value);
        }
        internal readonly AsynchronousEvent<GuildUnavailableEventArgs> _guildUnavailable;

        public event AsynchronousEventHandler<JoinedGuildEventArgs> JoinedGuild
        {
            add => _joinedGuild.Hook(value);
            remove => _joinedGuild.Unhook(value);
        }
        internal readonly AsynchronousEvent<JoinedGuildEventArgs> _joinedGuild;

        public event AsynchronousEventHandler<GuildUpdatedEventArgs> GuildUpdated
        {
            add => _guildUpdated.Hook(value);
            remove => _guildUpdated.Unhook(value);
        }
        internal readonly AsynchronousEvent<GuildUpdatedEventArgs> _guildUpdated;

        public event AsynchronousEventHandler<LeftGuildEventArgs> LeftGuild
        {
            add => _leftGuild.Hook(value);
            remove => _leftGuild.Unhook(value);
        }
        internal readonly AsynchronousEvent<LeftGuildEventArgs> _leftGuild;

        public event AsynchronousEventHandler<RoleCreatedEventArgs> RoleCreated
        {
            add => _roleCreated.Hook(value);
            remove => _roleCreated.Unhook(value);
        }
        internal readonly AsynchronousEvent<RoleCreatedEventArgs> _roleCreated;

        public event AsynchronousEventHandler<RoleUpdatedEventArgs> RoleUpdated
        {
            add => _roleUpdated.Hook(value);
            remove => _roleUpdated.Unhook(value);
        }
        internal readonly AsynchronousEvent<RoleUpdatedEventArgs> _roleUpdated;

        public event AsynchronousEventHandler<RoleDeletedEventArgs> RoleDeleted
        {
            add => _roleDeleted.Hook(value);
            remove => _roleDeleted.Unhook(value);
        }
        internal readonly AsynchronousEvent<RoleDeletedEventArgs> _roleDeleted;

        public event AsynchronousEventHandler<InviteCreatedEventArgs> InviteCreated
        {
            add => _inviteCreated.Hook(value);
            remove => _inviteCreated.Unhook(value);
        }
        internal readonly AsynchronousEvent<InviteCreatedEventArgs> _inviteCreated;

        public event AsynchronousEventHandler<InviteDeletedEventArgs> InviteDeleted
        {
            add => _inviteDeleted.Hook(value);
            remove => _inviteDeleted.Unhook(value);
        }
        internal readonly AsynchronousEvent<InviteDeletedEventArgs> _inviteDeleted;

        public event AsynchronousEventHandler<MemberBannedEventArgs> MemberBanned
        {
            add => _memberBanned.Hook(value);
            remove => _memberBanned.Unhook(value);
        }
        internal readonly AsynchronousEvent<MemberBannedEventArgs> _memberBanned;

        public event AsynchronousEventHandler<MemberUnbannedEventArgs> MemberUnbanned
        {
            add => _memberUnbanned.Hook(value);
            remove => _memberUnbanned.Unhook(value);
        }
        internal readonly AsynchronousEvent<MemberUnbannedEventArgs> _memberUnbanned;

        public event AsynchronousEventHandler<GuildEmojisUpdatedEventArgs> GuildEmojisUpdated
        {
            add => _guildEmojisUpdated.Hook(value);
            remove => _guildEmojisUpdated.Unhook(value);
        }
        internal readonly AsynchronousEvent<GuildEmojisUpdatedEventArgs> _guildEmojisUpdated;

        public event AsynchronousEventHandler<MemberJoinedEventArgs> MemberJoined
        {
            add => _memberJoined.Hook(value);
            remove => _memberJoined.Unhook(value);
        }
        internal readonly AsynchronousEvent<MemberJoinedEventArgs> _memberJoined;

        public event AsynchronousEventHandler<MemberLeftEventArgs> MemberLeft
        {
            add => _memberLeft.Hook(value);
            remove => _memberLeft.Unhook(value);
        }
        internal readonly AsynchronousEvent<MemberLeftEventArgs> _memberLeft;

        public event AsynchronousEventHandler<MemberUpdatedEventArgs> MemberUpdated
        {
            add => _memberUpdated.Hook(value);
            remove => _memberUpdated.Unhook(value);
        }
        internal readonly AsynchronousEvent<MemberUpdatedEventArgs> _memberUpdated;

        public event AsynchronousEventHandler<MessageAcknowledgedEventArgs> MessageAcknowledged
        {
            add => _messageAcknowledged.Hook(value);
            remove => _messageAcknowledged.Unhook(value);
        }
        internal readonly AsynchronousEvent<MessageAcknowledgedEventArgs> _messageAcknowledged;

        public event AsynchronousEventHandler<MessageReceivedEventArgs> MessageReceived
        {
            add => _messageReceived.Hook(value);
            remove => _messageReceived.Unhook(value);
        }
        internal readonly AsynchronousEvent<MessageReceivedEventArgs> _messageReceived;

        public event AsynchronousEventHandler<MessageDeletedEventArgs> MessageDeleted
        {
            add => _messageDeleted.Hook(value);
            remove => _messageDeleted.Unhook(value);
        }
        internal readonly AsynchronousEvent<MessageDeletedEventArgs> _messageDeleted;

        public event AsynchronousEventHandler<MessagesBulkDeletedEventArgs> MessagesBulkDeleted
        {
            add => _messagesBulkDeleted.Hook(value);
            remove => _messagesBulkDeleted.Unhook(value);
        }
        internal readonly AsynchronousEvent<MessagesBulkDeletedEventArgs> _messagesBulkDeleted;

        public event AsynchronousEventHandler<MessageUpdatedEventArgs> MessageUpdated
        {
            add => _messageUpdated.Hook(value);
            remove => _messageUpdated.Unhook(value);
        }
        internal readonly AsynchronousEvent<MessageUpdatedEventArgs> _messageUpdated;

        public event AsynchronousEventHandler<ReactionAddedEventArgs> ReactionAdded
        {
            add => _reactionAdded.Hook(value);
            remove => _reactionAdded.Unhook(value);
        }
        internal readonly AsynchronousEvent<ReactionAddedEventArgs> _reactionAdded;

        public event AsynchronousEventHandler<ReactionRemovedEventArgs> ReactionRemoved
        {
            add => _reactionRemoved.Hook(value);
            remove => _reactionRemoved.Unhook(value);
        }
        internal readonly AsynchronousEvent<ReactionRemovedEventArgs> _reactionRemoved;

        public event AsynchronousEventHandler<ReactionsClearedEventArgs> ReactionsCleared
        {
            add => _reactionsCleared.Hook(value);
            remove => _reactionsCleared.Unhook(value);
        }
        internal readonly AsynchronousEvent<ReactionsClearedEventArgs> _reactionsCleared;

        public event AsynchronousEventHandler<EmojiReactionsClearedEventArgs> EmojiReactionsCleared
        {
            add => _emojiReactionsCleared.Hook(value);
            remove => _emojiReactionsCleared.Unhook(value);
        }
        internal readonly AsynchronousEvent<EmojiReactionsClearedEventArgs> _emojiReactionsCleared;

        public event AsynchronousEventHandler<PresenceUpdatedEventArgs> PresenceUpdated
        {
            add => _presenceUpdated.Hook(value);
            remove => _presenceUpdated.Unhook(value);
        }
        internal readonly AsynchronousEvent<PresenceUpdatedEventArgs> _presenceUpdated;

        public event AsynchronousEventHandler<TypingStartedEventArgs> TypingStarted
        {
            add => _typingStarted.Hook(value);
            remove => _typingStarted.Unhook(value);
        }
        internal readonly AsynchronousEvent<TypingStartedEventArgs> _typingStarted;

        public event AsynchronousEventHandler<RelationshipCreatedEventArgs> RelationshipCreated
        {
            add => _relationshipCreated.Hook(value);
            remove => _relationshipCreated.Unhook(value);
        }
        internal readonly AsynchronousEvent<RelationshipCreatedEventArgs> _relationshipCreated;

        public event AsynchronousEventHandler<RelationshipDeletedEventArgs> RelationshipDeleted
        {
            add => _relationshipDeleted.Hook(value);
            remove => _relationshipDeleted.Unhook(value);
        }
        internal readonly AsynchronousEvent<RelationshipDeletedEventArgs> _relationshipDeleted;

        public event AsynchronousEventHandler<UserNoteUpdatedEventArgs> UserNoteUpdated
        {
            add => _userNoteUpdated.Hook(value);
            remove => _userNoteUpdated.Unhook(value);
        }
        internal readonly AsynchronousEvent<UserNoteUpdatedEventArgs> _userNoteUpdated;

        public event AsynchronousEventHandler<UserUpdatedEventArgs> UserUpdated
        {
            add => _userUpdated.Hook(value);
            remove => _userUpdated.Unhook(value);
        }
        internal readonly AsynchronousEvent<UserUpdatedEventArgs> _userUpdated;

        public event AsynchronousEventHandler<VoiceStateUpdatedEventArgs> VoiceStateUpdated
        {
            add => _voiceStateUpdated.Hook(value);
            remove => _voiceStateUpdated.Unhook(value);
        }
        internal readonly AsynchronousEvent<VoiceStateUpdatedEventArgs> _voiceStateUpdated;

        public event AsynchronousEventHandler<VoiceServerUpdatedEventArgs> VoiceServerUpdated
        {
            add => _voiceServerUpdated.Hook(value);
            remove => _voiceServerUpdated.Unhook(value);
        }
        internal readonly AsynchronousEvent<VoiceServerUpdatedEventArgs> _voiceServerUpdated;

        public event AsynchronousEventHandler<WebhooksUpdatedEventArgs> WebhooksUpdated
        {
            add => _webhooksUpdated.Hook(value);
            remove => _webhooksUpdated.Unhook(value);
        }
        internal readonly AsynchronousEvent<WebhooksUpdatedEventArgs> _webhooksUpdated;
    }
}
