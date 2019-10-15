using System;
using System.Collections.Generic;

namespace Disqord
{
    internal sealed class EmojiEqualityComparer : IEqualityComparer<IEmoji>
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

            return x.Equals(y);
        }

        public int GetHashCode(IEmoji obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            return obj.GetHashCode();
        }
    }
}
