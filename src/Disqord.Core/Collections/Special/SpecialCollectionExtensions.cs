using System.Collections.Generic;

namespace Disqord.Collections
{
    internal static class SpecialCollectionExtensions
    {
        public static IReadOnlyList<Snowflake> Snowflakes(this ulong[] array)
            => new SnowflakeList(array);
    }
}
