namespace Disqord;

public static class LocalSpoilerableExtensions
{
    public static TSpoilerable WithIsSpoiler<TSpoilerable>(this TSpoilerable spoilerable, bool isSpoiler = true)
        where TSpoilerable : ILocalSpoilerable
    {
        spoilerable.IsSpoiler = isSpoiler;
        return spoilerable;
    }
}
