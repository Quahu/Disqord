using System;
using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents a user's Spotify activity.
    /// </summary>
    public sealed class SpotifyActivity : Activity
    {
        public override string Name => "Spotify";

        public string PartyId { get; }

        public string TrackTitle { get; }

        public string TrackId { get; }

        public string TrackUrl => $"https://open.spotify.com/track/{TrackId}";

        public string AlbumTitle { get; }

        public string AlbumId { get; }

        public string AlbumCoverUrl => AlbumId != null
            ? $"https://i.scdn.co/image/{AlbumId}"
            : null;

        public IReadOnlyList<string> Artists { get; }

        public DateTimeOffset StartedAt { get; }

        public DateTimeOffset EndsAt { get; }

        public TimeSpan Duration => EndsAt - StartedAt;

        public TimeSpan Elapsed => DateTimeOffset.UtcNow - StartedAt;

        public TimeSpan Remaining => EndsAt - DateTimeOffset.UtcNow;

        internal SpotifyActivity(ActivityModel model) : base(model)
        {
            PartyId = model.Party?.Id;
            TrackTitle = model.Details;
            TrackId = model.SyncId;

            if (model.Assets != null)
            {
                AlbumTitle = model.Assets.LargeText;
                var span = model.Assets.LargeImage.AsSpan();
                if (span.Length > 9 && span.Slice(0, 8).Equals("spotify:", StringComparison.OrdinalIgnoreCase))
                {
                    AlbumId = new string(span.Slice(8));
                }
            }

            var artists = model.State?.Split("; ", StringSplitOptions.RemoveEmptyEntries);
            Artists = artists?.ReadOnly() ?? ReadOnlyList<string>.Empty;

            if (model.Timestamps != null)
            {
                if (model.Timestamps.Start != null)
                    StartedAt = DateTimeOffset.FromUnixTimeMilliseconds(model.Timestamps.Start.Value);

                if (model.Timestamps.End != null)
                    EndsAt = DateTimeOffset.FromUnixTimeMilliseconds(model.Timestamps.End.Value);
            }
        }

    }
}
