using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Disqord.Collections;
using Disqord.Gateway.Api.Models;
using Disqord.Models;

namespace Disqord.Gateway
{
    public abstract class CachedMessage : CachedSnowflakeEntity, IGatewayMessage,
        IJsonUpdatable<MessageReactionAddJsonModel>, IJsonUpdatable<MessageReactionRemoveJsonModel>,
        IJsonUpdatable<MessageReactionRemoveEmojiJsonModel>, IJsonUpdatable<MessageReactionRemoveAllJsonModel>
    {
        public Snowflake ChannelId { get; }

        public Snowflake? GuildId { get; }

        public IUser Author
        {
            get
            {
                if (_cachedAuthor != null)
                    return _cachedAuthor;

                return _transientAuthor;
            }
        }
        protected TransientUser _transientAuthor;
        private CachedMember _cachedAuthor;

        public virtual string Content { get; protected set; }

        public IReadOnlyList<IUser> MentionedUsers { get; protected set; }

        public Optional<IReadOnlyDictionary<IEmoji, MessageReaction>> Reactions { get; protected set; }

        protected CachedMessage(IGatewayClient client, CachedMember author, MessageJsonModel model)
            : base(client, model.Id)
        {
            ChannelId = model.ChannelId;
            GuildId = model.GuildId.GetValueOrNullable();
            if (author != null)
            {
                _cachedAuthor = author;
            }
            else
            {
                if (model.Member.HasValue)
                {
                    model.Member.Value.User = model.Author;
                    _transientAuthor = new TransientMember(Client, GuildId.Value, model.Member.Value);
                }
                else
                {
                    _transientAuthor = new TransientUser(Client, model.Author);
                }
            }

            Update(model);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void Update(MessageJsonModel model)
        {
            if (_transientAuthor != null)
            {
                if (model.Member.HasValue)
                {
                    model.Member.Value.User = model.Author;
                    _transientAuthor = new TransientMember(Client, GuildId.Value, model.Member.Value);
                }
                else
                {
                    _transientAuthor = new TransientUser(Client, model.Author);
                }
            }
            Content = model.Content;
            MentionedUsers = model.Mentions.ToReadOnlyList(Client, (x, client) =>
            {
                var user = client.GetUser(x.Id);
                if (user != null)
                    return user;

                return new TransientUser(client, x) as IUser;
            });
            Reactions = Optional.Convert(model.Reactions, x => x.ToReadOnlyDictionary(x => Emoji.Create(x.Emoji), x => new MessageReaction(x)));
        }

        public void Update(MessageReactionAddJsonModel model)
        {
            var emoji = Emoji.Create(model.Emoji);
            var reactions = Reactions;
            if (!reactions.HasValue)
            {
                var newReactions = new Dictionary<IEmoji, MessageReaction>();
                newReactions.Add(emoji, new MessageReaction(emoji, 1, model.UserId == Client.CurrentUser.Id));
                Reactions = new Optional<IReadOnlyDictionary<IEmoji, MessageReaction>>(newReactions.ReadOnly());
            }
            else
            {
                var newReactions = new Dictionary<IEmoji, MessageReaction>(reactions.Value);
                var reaction = newReactions.GetValueOrDefault(emoji);
                newReactions[emoji] = new MessageReaction(emoji, (reaction?.Count ?? 0) + 1, (reaction?.HasOwnReaction ?? false) || model.UserId == Client.CurrentUser.Id);
                Reactions = newReactions;
            }
        }

        public void Update(MessageReactionRemoveJsonModel model)
        {
            var reactions = Reactions;
            if (reactions.HasValue)
            {
                var emoji = Emoji.Create(model.Emoji);
                if (reactions.Value.TryGetValue(emoji, out var reaction))
                {
                    var newReactions = new Dictionary<IEmoji, MessageReaction>(reactions.Value);
                    if (reaction.Count == 1)
                        newReactions.Remove(emoji);
                    else
                        newReactions[emoji] = new MessageReaction(emoji, reaction.Count - 1, reaction.HasOwnReaction && model.UserId == Client.CurrentUser.Id
                            ? false
                            : reaction.HasOwnReaction);
                    Reactions = newReactions;
                }
            }
        }

        public void Update(MessageReactionRemoveEmojiJsonModel model)
        {
            var emoji = Emoji.Create(model.Emoji);
            Reactions = Optional.Convert(Reactions, x => x.Where(x => !x.Key.Equals(emoji)).ToReadOnlyDictionary(x => x.Key, x => x.Value));
        }

        public void Update(MessageReactionRemoveAllJsonModel model)
        {
            Reactions = default;
        }
    }
}
