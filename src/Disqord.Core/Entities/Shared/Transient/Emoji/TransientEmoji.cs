using Disqord.Models;

namespace Disqord
{
    public class TransientEmoji : TransientEntity<EmojiJsonModel>, IEmoji
    {
        /// <inheritdoc/>
        public string Name => Model.Name;

        public TransientEmoji(EmojiJsonModel model)
            : base(model)
        { }

        public TransientEmoji(Snowflake? id, string name)
            : base(new EmojiJsonModel
            {
                Id = id,
                Name = name
            })
        { }

        public bool Equals(IEmoji other)
            => Comparers.Emoji.Equals(this, other);

        public override bool Equals(object obj)
            => obj is IEmoji emoji && Equals(emoji);

        public override int GetHashCode()
            => Comparers.Emoji.GetHashCode(this);

        public override string ToString()
            => this.GetString();

        public static IEmoji Create(EmojiJsonModel model)
        {
            if (model.Id.HasValue)
                return new TransientCustomEmoji(model);

            return new TransientEmoji(model);
        }
    }
}
