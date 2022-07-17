using System.Collections.Generic;
using Qommon;

namespace Disqord;

public sealed class EmojiEqualityComparer : IEqualityComparer<IEmoji>, IEqualityComparer<ICustomEmoji>
{
    public static readonly EmojiEqualityComparer Instance = new();

    private EmojiEqualityComparer()
    { }

    public bool Equals(IEmoji? x, IEmoji? y)
    {
        if (x == null && y == null)
            return true;

        if (x == null || y == null)
            return false;

        if (x is ICustomEmoji customEmoji && y is ICustomEmoji otherCustomEmoji)
            return customEmoji.Id == otherCustomEmoji.Id;

        if (x is ICustomEmoji || y is ICustomEmoji)
            return false;

        return x.Name == y.Name;
    }

    public bool Equals(ICustomEmoji? x, ICustomEmoji? y)
    {
        if (x == null && y == null)
            return true;

        if (x == null || y == null)
            return false;

        return x.Id == y.Id;
    }

    public int GetHashCode(IEmoji obj)
    {
        Guard.IsNotNull(obj);

        if (obj is ICustomEmoji customEmoji)
            return customEmoji.Id.GetHashCode();

        return obj.Name?.GetHashCode() ?? -1;
    }

    public int GetHashCode(ICustomEmoji obj)
    {
        Guard.IsNotNull(obj);

        return obj.Id.GetHashCode();
    }
}
