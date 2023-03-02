using System.Collections.Generic;
using Qommon;

namespace Disqord;

/// <summary>
///     Represents a comparer for <typeparamref name="TIdentifiableEntity"/> instances.
/// </summary>
/// <typeparam name="TIdentifiableEntity"> The entity type. </typeparam>
public class IdentifiableEntityComparer<TIdentifiableEntity> : IEqualityComparer<TIdentifiableEntity>
    where TIdentifiableEntity : class, IIdentifiableEntity
{
    /// <inheritdoc/>
    public bool Equals(TIdentifiableEntity? x, TIdentifiableEntity? y)
    {
        if (x == null && y == null)
            return true;

        if (x == null || y == null)
            return false;

        return x.Id == y.Id;
    }

    /// <inheritdoc/>
    public int GetHashCode(TIdentifiableEntity obj)
    {
        Guard.IsNotNull(obj);

        return obj.Id.GetHashCode();
    }
}
