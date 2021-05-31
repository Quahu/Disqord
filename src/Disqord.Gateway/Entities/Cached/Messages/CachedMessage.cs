using System.Collections.Generic;
using System.ComponentModel;
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
            // TODO
        }

        public void Update(MessageReactionRemoveJsonModel model)
        {
            // TODO
        }

        public void Update(MessageReactionRemoveEmojiJsonModel model)
        {
            // TODO
        }

        public void Update(MessageReactionRemoveAllJsonModel model)
        {
            Reactions = default;
        }
    }
}
