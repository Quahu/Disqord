using System.Collections.Generic;
using System.ComponentModel;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord.Gateway
{
    public abstract class CachedMessage : CachedSnowflakeEntity, IGatewayMessage
    {
        public Snowflake ChannelId { get; }

        public Snowflake? GuildId { get; }

        public IUser Author
        {
            get
            {
                if (_transientAuthor != null)
                    return _transientAuthor;

                if (GuildId != null)
                    return Client.GetMember(GuildId.Value, _authorId);
                else
                    return Client.GetUser(_authorId);
            }
        }
        protected TransientUser _transientAuthor;
        private Snowflake _authorId;

        public virtual string Content { get; protected set; }

        public IReadOnlyList<IUser> MentionedUsers { get; protected set; }

        public Optional<IReadOnlyDictionary<IEmoji, IReaction>> Reactions { get; protected set; }

        protected CachedMessage(IGatewayClient client, MessageJsonModel model)
            : base(client, model.Id)
        {
            ChannelId = model.ChannelId;
            GuildId = model.GuildId.GetValueOrNullable();
            _authorId = model.Author.Id;
            if (GuildId != null && !Client.CacheProvider.Supports<CachedMember>()
                || GuildId == null && !Client.CacheProvider.Supports<CachedSharedUser>())
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
            Reactions = Optional.Convert(model.Reactions, x => x.ToReadOnlyDictionary(x => Emoji.Create(x.Emoji), x => new Reaction(x) as IReaction));
        }
    }
}
