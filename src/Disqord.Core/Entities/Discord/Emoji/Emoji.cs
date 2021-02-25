using System;
using Disqord.Models;

namespace Disqord
{
    public class Emoji : IEmoji
    {
        public string Name { get; }

        IClient IEntity.Client => throw new EntityNotManagedException();

        protected Emoji(EmojiJsonModel model)
        {
            Name = model.Name;
        }

        public bool Equals(IEmoji? other)
            => Discord.Comparers.Emoji.Equals(this, other);

        public override bool Equals(object? obj)
            => obj is IEmoji emoji && Equals(emoji);

        public override int GetHashCode()
            => Discord.Comparers.Emoji.GetHashCode(this);

        public override string ToString()
            => this.GetMessageFormat();

        void IJsonUpdatable<EmojiJsonModel>.Update(EmojiJsonModel model)
            => throw new NotSupportedException();

        public static IEmoji Create(EmojiJsonModel model)
        {
            if (model.Id.HasValue)
                return new CustomEmoji(model);

            return new Emoji(model);
        }
    }
}
