using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Models;
using Qommon.Collections;

namespace Disqord
{
    public abstract class CachedMessage : CachedSnowflakeEntity, IMessage
    {
        public CachedGuild Guild => (Channel as CachedTextChannel)?.Guild;

        public ICachedMessageChannel Channel { get; }

        public CachedUser Author { get; }

        public abstract string Content { get; }

        public DateTimeOffset Timestamp { get; }

        public IReadOnlyList<CachedUser> UserMentions { get; private set; }

        public IReadOnlyDictionary<IEmoji, ReactionData> Reactions { get; }

        internal readonly ConcurrentDictionary<IEmoji, ReactionData> _reactions;

        IUser IMessage.Author => Author;
        IReadOnlyList<IUser> IMessage.UserMentions => UserMentions;
        Snowflake IMessage.ChannelId => Channel.Id;

        internal CachedMessage(DiscordClient client, MessageModel model, ICachedMessageChannel channel, CachedUser author) : base(client, model.Id)
        {
            Channel = channel;
            Author = author;
            Timestamp = model.Timestamp.HasValue ? model.Timestamp.Value : Id.CreatedAt;
            _reactions = Extensions.CreateConcurrentDictionary<IEmoji, ReactionData>(model.Reactions.HasValue
                ? model.Reactions.Value.Length
                : 0);
            Reactions = new ReadOnlyConcurrentDictionary<IEmoji, ReactionData>(_reactions);
        }

        internal static CachedMessage Create(DiscordClient client, MessageModel model, ICachedMessageChannel channel, CachedUser author)
        {
            return model.Type switch
            {
                MessageType.Default => new CachedUserMessage(client, model, channel, author),
                _ => new CachedSystemMessage(client, model, channel, author),
            };
        }

        internal virtual void Update(MessageModel model)
        {
            if (model.Mentions.HasValue)
                UserMentions = model.Mentions.Value.Select(x => Client.GetUser(x.Id)).ToImmutableArray();
        }

        public Task AddReactionAsync(IEmoji emoji, RestRequestOptions options = null)
            => Client.AddReactionAsync(Channel.Id, Id, emoji, options);

        public Task RemoveOwnReactionAsync(IEmoji emoji, RestRequestOptions options = null)
            => Client.RemoveOwnReactionAsync(Channel.Id, Id, emoji, options);

        public Task RemoveMemberReactionAsync(Snowflake memberId, IEmoji emoji, RestRequestOptions options = null)
            => Client.RemoveMemberReactionAsync(Channel.Id, Id, memberId, emoji, options);

        public Task MarkAsReadAsync(RestRequestOptions options = null)
            => Client.MarkMessageAsReadAsync(Channel.Id, Id, options);

        public Task DeleteAsync(RestRequestOptions options = null)
            => Client.DeleteMessageAsync(Channel.Id, Id, options);
    }
}
