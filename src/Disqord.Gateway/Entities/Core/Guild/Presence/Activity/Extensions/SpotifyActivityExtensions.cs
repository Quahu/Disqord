using System;
using System.ComponentModel;

namespace Disqord.Gateway
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class SpotifyActivityExtensions
    {
        public static string GetTrackUrl(this ISpotifyActivity activity)
            => $"https://open.spotify.com/track/{activity.TrackId}";

        public static string GetAlbumCoverUrl(this ISpotifyActivity activity)
            => activity.AlbumId != null
                ? $"https://i.scdn.co/image/{activity.AlbumId}"
                : null;

        public static TimeSpan GetDuration(this ISpotifyActivity activity)
            => activity.EndsAt - activity.StartedAt;

        public static TimeSpan GetElapsedDuration(this ISpotifyActivity activity)
            => DateTimeOffset.UtcNow - activity.StartedAt;

        public static TimeSpan GetRemainingDuration(this ISpotifyActivity activity)
            => activity.EndsAt - DateTimeOffset.UtcNow;
    }
}
