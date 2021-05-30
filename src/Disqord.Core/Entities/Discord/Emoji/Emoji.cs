using Disqord.Models;

namespace Disqord
{
    public class Emoji : IEmoji
    {
        public string Name { get; }

        protected Emoji(EmojiJsonModel model)
        {
            Name = model.Name;
        }

        public bool Equals(IEmoji other)
            => Comparers.Emoji.Equals(this, other);

        public override bool Equals(object obj)
            => obj is IEmoji emoji && Equals(emoji);

        public override int GetHashCode()
            => Comparers.Emoji.GetHashCode(this);

        public override string ToString()
            => this.GetMessageFormat();

        public static IEmoji Create(EmojiJsonModel model)
        {
            if (model.Id.HasValue)
                return new CustomEmoji(model);

            return new Emoji(model);
        }
    }
}
