using System.Collections.Generic;
using Disqord.Models;

namespace Disqord
{
    public class TransientGuildEmoji : TransientEntity<EmojiJsonModel>, IGuildEmoji
    {
        /// <inheritdoc/>
        public Snowflake Id => Model.Id.Value;

        /// <inheritdoc/>
        public Snowflake GuildId { get; }

        /// <inheritdoc/>
        public string Name => Model.Name;

        /// <inheritdoc/>
        public bool IsAnimated => Model.Animated.Value;

        /// <inheritdoc/>
        public IReadOnlyList<Snowflake> RoleIds => Model.Roles.Value;

        /// <inheritdoc/>
        public IUser Creator => _creator ??= new TransientUser(Client, Model.User.Value);
        private IUser _creator;

        /// <inheritdoc/>
        public bool RequiresColons => Model.RequireColons.Value;

        /// <inheritdoc/>
        public bool IsManaged => Model.Managed.Value;

        /// <inheritdoc/>
        public bool IsAvailable => Model.Available.Value;

        /// <inheritdoc/>
        public string Tag => this.GetMessageFormat();

        public TransientGuildEmoji(IClient client, Snowflake guildId, EmojiJsonModel model)
            : base(client, model)
        {
            GuildId = guildId;
        }

        public bool Equals(IEmoji other)
            => Comparers.Emoji.Equals(this, other);

        public bool Equals(ICustomEmoji other)
            => Comparers.Emoji.Equals(this, other);

        public override bool Equals(object obj)
            => obj is IEmoji emoji && Equals(emoji);

        public override int GetHashCode()
            => Comparers.Emoji.GetHashCode(this);

        public override string ToString()
            => Tag;
    }
}
