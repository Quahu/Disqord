using System;
using Disqord.Models;

namespace Disqord
{
    public class LocalEmoji : IEmoji
    {
        public string Name { get; }

        IClient IEntity.Client => throw new NotSupportedException("A local emoji is not bound to a client.");

        public LocalEmoji(string unicode)
        {
            Name = unicode;
        }

        public bool Equals(IEmoji other)
            => Comparers.Emoji.Equals(this, other);

        public override bool Equals(object obj)
            => obj is IEmoji emoji && Equals(emoji);

        public override int GetHashCode()
            => Comparers.Emoji.GetHashCode(this);

        public override string ToString()
            => Name;

        void IJsonUpdatable<EmojiJsonModel>.Update(EmojiJsonModel model)
            => throw new NotSupportedException();
    }
}
