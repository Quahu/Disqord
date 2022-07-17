namespace Disqord;

/// <summary>
///     Provides equality comparer instances in one place.
/// </summary>
public static class Comparers
{
    public static EmojiEqualityComparer Emoji => EmojiEqualityComparer.Instance;
}