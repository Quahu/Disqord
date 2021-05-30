using Disqord.Models;

namespace Disqord
{
    public class CustomEmoji : Emoji, ICustomEmoji
    {
        public Snowflake Id { get; }

        public bool IsAnimated { get; }

        public string Tag => this.GetMessageFormat();

        public CustomEmoji(EmojiJsonModel model)
            : base(model)
        {
            Id = model.Id.Value;
            IsAnimated = model.Animated.GetValueOrDefault();
        }

        public bool Equals(ICustomEmoji other)
            => Comparers.Emoji.Equals(this, other);

        public override string ToString()
            => Tag;
    }
}
