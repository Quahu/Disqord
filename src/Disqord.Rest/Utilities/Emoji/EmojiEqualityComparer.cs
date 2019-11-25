using System;
using System.Collections.Generic;

namespace Disqord
{
    public sealed class EmojiEqualityComparer : IEqualityComparer<IEmoji>, IEqualityComparer<ICustomEmoji>
    {
        public static readonly EmojiEqualityComparer Instance = new EmojiEqualityComparer();

        private EmojiEqualityComparer()
        { }

        public bool Equals(IEmoji x, IEmoji y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            if (x is ICustomEmoji customEmoji && y is ICustomEmoji otherCustomEmoji)
                return customEmoji.Id == otherCustomEmoji.Id;

            return x.Name == y.Name;
        }

        public bool Equals(ICustomEmoji x, ICustomEmoji y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.Id == y.Id;
        }

        public int GetHashCode(IEmoji obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            if (obj is ICustomEmoji customEmoji)
                return customEmoji.Id.GetHashCode();

            return obj.Name.GetHashCode();
        }

        public int GetHashCode(ICustomEmoji obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            return obj.Id.GetHashCode();
        }
    }
}
