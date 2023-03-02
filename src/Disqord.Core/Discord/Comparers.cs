namespace Disqord;

/// <summary>
///     Provides equality comparer instances in one place.
/// </summary>
public static class Comparers
{
    public static EmojiEqualityComparer Emoji => EmojiEqualityComparer.Instance;

    /// <summary>
    ///     Gets a comparer for comparing <see cref="IIdentifiableEntity"/> instances.
    /// </summary>
    /// <remarks>
    ///     Does not support sorting.
    /// </remarks>
    public static IdentifiableEntityComparer IdentifiableEntities => new();

    /// <summary>
    ///     Gets a comparer for comparing <see cref="IRole"/> instances.
    /// </summary>
    /// <remarks>
    ///     Supports sorting.
    /// </remarks>
    public static RoleComparer Roles => new();
}
