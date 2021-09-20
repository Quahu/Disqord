using System;
using System.Collections.Generic;
using Qommon.Collections;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway
{
    public class TransientSpotifyActivity : TransientActivity, ISpotifyActivity
    {
        /// <inheritdoc/>
        public string PartyId => Model.Party.GetValueOrDefault()?.Id.GetValueOrDefault();

        /// <inheritdoc/>
        public string TrackTitle => Model.Details.GetValueOrDefault();

        /// <inheritdoc/>
        public string TrackId => Model.SyncId.GetValueOrDefault();

        /// <inheritdoc/>
        public string AlbumTitle => Model.Assets.GetValueOrDefault()?.LargeText.GetValueOrDefault();

        /// <inheritdoc/>
        public string AlbumId
        {
            get
            {
                if (!Model.Assets.HasValue)
                    return null;

                if (_albumId == null)
                {
                    var span = Model.Assets.Value.LargeImage.Value.AsSpan();
                    if (span.Length > 9 && span.Slice(0, 8).Equals("spotify:", StringComparison.OrdinalIgnoreCase))
                        _albumId = new string(span.Slice(8));
                    else
                        _albumId = Model.Assets.Value.LargeImage.Value;
                }

                return _albumId;
            }
        }
        private string _albumId;

        /// <inheritdoc/>
        public IReadOnlyList<string> Artists
        {
            get
            {
                if (Model.State.GetValueOrDefault() == null)
                    return null;

                if (_artists == null)
                    _artists = Model.State.Value.Split("; ", StringSplitOptions.RemoveEmptyEntries).ReadOnly();

                return _artists;
            }
        }
        private IReadOnlyList<string> _artists;

        /// <inheritdoc/>
        public DateTimeOffset StartedAt => DateTimeOffset.FromUnixTimeMilliseconds(Model.Timestamps.Value.Start.Value);

        /// <inheritdoc/>
        public DateTimeOffset EndsAt => DateTimeOffset.FromUnixTimeMilliseconds(Model.Timestamps.Value.End.Value);

        public TransientSpotifyActivity(IClient client, ActivityJsonModel model)
            : base(client, model)
        { }

        public override string ToString()
            => $"{TrackTitle} on {AlbumTitle} by {Model.State}";
    }

}
