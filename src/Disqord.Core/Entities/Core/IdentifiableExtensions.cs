using System;
using System.ComponentModel;

namespace Disqord
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class IdentifiableExtensions
    {
        /// <summary>
        ///     Gets the creation date of this entity. Short for <see cref="Snowflake.CreatedAt"/> from <see cref="ISnowflakeEntity.Id"/>.
        /// </summary>
        /// <returns></returns>
        public static DateTimeOffset CreatedAt(this IIdentifiable entity)
            => entity.Id.CreatedAt;
    }
}
