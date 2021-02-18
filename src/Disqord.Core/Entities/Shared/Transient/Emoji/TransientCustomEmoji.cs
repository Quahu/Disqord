using System;
using Disqord.Models;

namespace Disqord
{
    public class TransientCustomEmoji : TransientEmoji, ICustomEmoji
    {
        public Snowflake Id => Model.Id.Value;

        public DateTimeOffset CreatedAt => Id.CreatedAt;

        public bool IsAnimated => Model.Animated.GetValueOrDefault();

        public string Tag => Discord.GetMessageFormat(this);

        public TransientCustomEmoji(IClient client, EmojiJsonModel model)
            : base(client, model)
        { }

        public override int GetHashCode()
            => Id.GetHashCode();

        public bool Equals(ICustomEmoji other)
            => Discord.Comparers.Emoji.Equals(this, other);

        public override string ToString()
            => Tag;
    }
}
