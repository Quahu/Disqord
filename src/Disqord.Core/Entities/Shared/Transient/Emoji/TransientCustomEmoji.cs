using Disqord.Models;
using Qommon;

namespace Disqord
{
    public class TransientCustomEmoji : TransientEmoji, ICustomEmoji
    {
        public Snowflake Id => Model.Id.Value;

        public bool IsAnimated => Model.Animated.GetValueOrDefault();

        public string Tag => ToString();

        public TransientCustomEmoji(EmojiJsonModel model)
            : base(model)
        { }

        public bool Equals(ICustomEmoji other)
            => Comparers.Emoji.Equals(this, other);
    }
}
