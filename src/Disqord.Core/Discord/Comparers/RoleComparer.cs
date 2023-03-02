using System.Collections.Generic;

namespace Disqord;

/// <summary>
///     Represents a comparer for <see cref="IRole"/> instances.
/// </summary>
public class RoleComparer : IdentifiableEntityComparer<IRole>, IComparer<IRole>
{
    /// <inheritdoc/>
    public int Compare(IRole? x, IRole? y)
    {
        if (x == null && y == null)
            return 0;

        if (x == null || y == null)
            return x == null ? -1 : 1;

        var positionCompare = x.Position.CompareTo(y.Position);
        if (positionCompare != 0)
            return positionCompare;

        return -x.Id.CompareTo(y.Id);
    }
}
