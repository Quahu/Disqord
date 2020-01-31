using System;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Events;
using Disqord.Logging;
using Disqord.Rest;
using Disqord.Serialization.Json;
using Disqord.Sharding;
using Qommon.Events;

namespace Disqord
{
    public abstract partial class DiscordClientBase : IRestDiscordClient, IAsyncDisposable
    {
        public RestFetchable<RestApplication> CurrentApplication => RestClient.CurrentApplication;

        /// <summary>
        ///     Gets the token type this client is using.
        /// </summary>
        public TokenType TokenType => RestClient.TokenType;

        public ILogger Logger { get; }

        public IJsonSerializer Serializer { get; }

        internal RestDiscordClient RestClient { get; }

        internal string Token => RestClient.ApiClient._token;

        internal bool IsBot => TokenType == TokenType.Bot;

        internal bool IsDisposed;

        internal Func<DiscordClientBase, ulong, DiscordClientGateway> _getGateway;

        RestFetchable<RestCurrentUser> IRestDiscordClient.CurrentUser => RestClient.CurrentUser;

        internal DiscordClientBase(RestDiscordClient restClient, DiscordClientBaseConfiguration configuration)
        {
            if (restClient == null)
                throw new ArgumentNullException(nameof(restClient));

            if (!restClient.HasAuthorization)
                throw new ArgumentException("Clients without authorization are not supported.", nameof(restClient));

            RestClient = restClient;
            State = new DiscordClientState(this, configuration.MessageCache.GetValueOrDefault(() => new DefaultMessageCache(100)));
            Logger = configuration.Logger.GetValueOrDefault() ?? restClient.Logger;
            Serializer = configuration.Serializer.GetValueOrDefault() ?? restClient.Serializer;
            _extensions = new LockedDictionary<Type, DiscordClientExtension>();

            _ready = new AsynchronousEvent<ReadyEventArgs>();
            _channelCreated = new AsynchronousEvent<ChannelCreatedEventArgs>();
            _channelUpdated = new AsynchronousEvent<ChannelUpdatedEventArgs>();
            _channelDeleted = new AsynchronousEvent<ChannelDeletedEventArgs>();
            _channelPinsUpdated = new AsynchronousEvent<ChannelPinsUpdatedEventArgs>();
            _guildAvailable = new AsynchronousEvent<GuildAvailableEventArgs>();
            _guildUnavailable = new AsynchronousEvent<GuildUnavailableEventArgs>();
            _joinedGuild = new AsynchronousEvent<JoinedGuildEventArgs>();
            _guildUpdated = new AsynchronousEvent<GuildUpdatedEventArgs>();
            _leftGuild = new AsynchronousEvent<LeftGuildEventArgs>();
            _roleCreated = new AsynchronousEvent<RoleCreatedEventArgs>();
            _roleUpdated = new AsynchronousEvent<RoleUpdatedEventArgs>();
            _roleDeleted = new AsynchronousEvent<RoleDeletedEventArgs>();
            _inviteCreated = new AsynchronousEvent<InviteCreatedEventArgs>();
            _inviteDeleted = new AsynchronousEvent<InviteDeletedEventArgs>();
            _memberBanned = new AsynchronousEvent<MemberBannedEventArgs>();
            _memberUnbanned = new AsynchronousEvent<MemberUnbannedEventArgs>();
            _guildEmojisUpdated = new AsynchronousEvent<GuildEmojisUpdatedEventArgs>();
            _memberJoined = new AsynchronousEvent<MemberJoinedEventArgs>();
            _memberLeft = new AsynchronousEvent<MemberLeftEventArgs>();
            _memberUpdated = new AsynchronousEvent<MemberUpdatedEventArgs>();
            _messageAcknowledged = new AsynchronousEvent<MessageAcknowledgedEventArgs>();
            _messageReceived = new AsynchronousEvent<MessageReceivedEventArgs>();
            _messageDeleted = new AsynchronousEvent<MessageDeletedEventArgs>();
            _messagesBulkDeleted = new AsynchronousEvent<MessagesBulkDeletedEventArgs>();
            _messageUpdated = new AsynchronousEvent<MessageUpdatedEventArgs>();
            _reactionAdded = new AsynchronousEvent<ReactionAddedEventArgs>();
            _reactionRemoved = new AsynchronousEvent<ReactionRemovedEventArgs>();
            _reactionsCleared = new AsynchronousEvent<ReactionsClearedEventArgs>();
            _emojiReactionsCleared = new AsynchronousEvent<EmojiReactionsClearedEventArgs>();
            _presenceUpdated = new AsynchronousEvent<PresenceUpdatedEventArgs>();
            _typingStarted = new AsynchronousEvent<TypingStartedEventArgs>();
            _relationshipCreated = new AsynchronousEvent<RelationshipCreatedEventArgs>();
            _relationshipDeleted = new AsynchronousEvent<RelationshipDeletedEventArgs>();
            _userNoteUpdated = new AsynchronousEvent<UserNoteUpdatedEventArgs>();
            _userUpdated = new AsynchronousEvent<UserUpdatedEventArgs>();
            _voiceStateUpdated = new AsynchronousEvent<VoiceStateUpdatedEventArgs>();
            _voiceServerUpdated = new AsynchronousEvent<VoiceServerUpdatedEventArgs>();
            _webhooksUpdated = new AsynchronousEvent<WebhooksUpdatedEventArgs>();

            if (this is IDiscordSharder sharder)
                sharder._shardReady = new AsynchronousEvent<ShardReadyEventArgs>();
        }

        internal readonly DiscordClientBase _client;

        internal DiscordClientBase(DiscordClientBase client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            _client = client;

            _getGateway = client._getGateway;
            State = client.State;
            // This is set, so that events give you the proper (bot) client, and not the underyling one.
            State._client = this;
            RestClient = client.RestClient;
            Logger = client.Logger;
            Serializer = client.Serializer;
            _extensions = client._extensions;

            _ready = client._ready;
            _channelCreated = client._channelCreated;
            _channelUpdated = client._channelUpdated;
            _channelDeleted = client._channelDeleted;
            _channelPinsUpdated = client._channelPinsUpdated;
            _guildAvailable = client._guildAvailable;
            _guildUnavailable = client._guildUnavailable;
            _joinedGuild = client._joinedGuild;
            _guildUpdated = client._guildUpdated;
            _leftGuild = client._leftGuild;
            _roleCreated = client._roleCreated;
            _roleUpdated = client._roleUpdated;
            _roleDeleted = client._roleDeleted;
            _inviteCreated = client._inviteCreated;
            _inviteDeleted = client._inviteDeleted;
            _memberBanned = client._memberBanned;
            _memberUnbanned = client._memberUnbanned;
            _guildEmojisUpdated = client._guildEmojisUpdated;
            _memberJoined = client._memberJoined;
            _memberLeft = client._memberLeft;
            _memberUpdated = client._memberUpdated;
            _messageAcknowledged = client._messageAcknowledged;
            _messageReceived = client._messageReceived;
            _messageDeleted = client._messageDeleted;
            _messagesBulkDeleted = client._messagesBulkDeleted;
            _messageUpdated = client._messageUpdated;
            _reactionAdded = client._reactionAdded;
            _reactionRemoved = client._reactionRemoved;
            _reactionsCleared = client._reactionsCleared;
            _emojiReactionsCleared = client._emojiReactionsCleared;
            _presenceUpdated = client._presenceUpdated;
            _typingStarted = client._typingStarted;
            _relationshipCreated = client._relationshipCreated;
            _relationshipDeleted = client._relationshipDeleted;
            _userNoteUpdated = client._userNoteUpdated;
            _userUpdated = client._userUpdated;
            _voiceStateUpdated = client._voiceStateUpdated;
            _voiceServerUpdated = client._voiceServerUpdated;
            _webhooksUpdated = client._webhooksUpdated;

            if (this is IDiscordSharder sharder && client is IDiscordSharder clientSharder)
                sharder._shardReady = clientSharder._shardReady;
        }

        internal void ThrowIfDisposed()
        {
            if (_client?.IsDisposed ?? IsDisposed)
                throw new ObjectDisposedException(null, "The client has been disposed.");
        }

        internal void Log(LogMessageSeverity severity, string message, Exception exception = null)
            => Logger.Log(this, new MessageLoggedEventArgs("Client", severity, message, exception));

        internal DiscordClientGateway GetGateway(ulong guildId)
            => _getGateway(_client ?? this, guildId);

        public void Dispose()
            => DisposeAsync().GetAwaiter().GetResult();

        public virtual async ValueTask DisposeAsync()
        {
            if (_client != null)
            {
                await _client.DisposeAsync().ConfigureAwait(false);
                return;
            }

            if (IsDisposed)
                return;

            IsDisposed = true;
            State.Reset();

            foreach (var extensionKvp in _extensions)
            {
                try
                {
                    await extensionKvp.Value.DisposeAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Log(LogMessageSeverity.Error, $"An exception occurred while disposing the {extensionKvp.Key} extension.", ex);
                }
            }

            RestClient.Dispose();
        }
    }
}
