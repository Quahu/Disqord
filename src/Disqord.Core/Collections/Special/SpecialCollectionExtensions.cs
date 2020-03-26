using System.Collections.Generic;

namespace Disqord.Collections
{
    internal static class SpecialCollectionExtensions
    {
        public static IReadOnlyList<Snowflake> ToSnowflakeList(this ulong[] array)
            => new SnowflakeList(array);
    }
}
