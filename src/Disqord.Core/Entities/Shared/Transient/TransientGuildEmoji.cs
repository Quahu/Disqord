using System;
using System.Collections.Generic;
using Disqord.Models;

namespace Disqord
{
    public class TransientGuildEmoji : TransientEntity<EmojiJsonModel>, IGuildEmoji
    {
        public Snowflake Id => Model.Id.Value;

        public DateTimeOffset CreatedAt => Id.CreatedAt;

        public Snowflake GuildId { get; }

        public string Name => Model.Name;

        public bool IsAnimated => Model.Animated.Value;

        public IReadOnlyList<Snowflake> RoleIds => Model.Roles.Value;

        public IUser Creator
        {
            get
            {
                if (_creator == null)
                    _creator = new TransientUser(Client, Model.User.Value);

                return _creator;
            }
        }
        private IUser _creator;

        public bool RequiresColons => Model.RequireColons.Value;

        public bool IsManaged => Model.Managed.Value;

        public bool IsAvailable => Model.Available.Value;

        public string Tag => this.GetMessageFormat();

        public TransientGuildEmoji(IClient client, Snowflake guildId, EmojiJsonModel model)
            : base(client, model)
        {
            GuildId = guildId;
        }

        public bool Equals(IEmoji other)
            => Discord.Comparers.Emoji.Equals(this, other);

        public bool Equals(ICustomEmoji other)
            => Discord.Comparers.Emoji.Equals(this, other);

        public override bool Equals(object obj)
            => obj is IEmoji emoji && Equals(emoji);

        public override int GetHashCode()
            => Discord.Comparers.Emoji.GetHashCode(this);

        public override string ToString()
            => Tag;
    }
}
