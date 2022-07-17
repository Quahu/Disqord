using System;
using System.ComponentModel;

namespace Disqord.Gateway;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class SpotifyActivityExtensions
{
    public static string? GetTrackUrl(this ISpotifyActivity activity)
    {
        return activity.TrackId != null
            ? $"https://open.spotify.com/track/{activity.TrackId}"
            : null;
    }

    public static string? GetAlbumCoverUrl(this ISpotifyActivity activity)
    {
        return activity.AlbumId != null
            ? $"https://i.scdn.co/image/{activity.AlbumId}"
            : null;
    }

    public static TimeSpan GetDuration(this ISpotifyActivity activity)
    {
        return activity.EndsAt - activity.StartedAt;
    }

    public static TimeSpan GetElapsedDuration(this ISpotifyActivity activity)
    {
        return DateTimeOffset.UtcNow - activity.StartedAt;
    }

    public static TimeSpan GetRemainingDuration(this ISpotifyActivity activity)
    {
        return activity.EndsAt - DateTimeOffset.UtcNow;
    }
}
