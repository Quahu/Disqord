using System;
using System.ComponentModel;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class IdentifiableEntityExtensions
{
    /// <summary>
    ///     Gets the creation date of this entity.
    ///     Short for <see cref="Snowflake.CreatedAt"/> from <see cref="IIdentifiableEntity.Id"/>.
    /// </summary>
    /// <returns>
    ///     The date at which the entity was created.
    /// </returns>
    public static DateTimeOffset CreatedAt(this IIdentifiableEntity entity)
    {
        return entity.Id.CreatedAt;
    }
}
