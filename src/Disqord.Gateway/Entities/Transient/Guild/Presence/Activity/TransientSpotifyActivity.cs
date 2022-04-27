using System;
using System.Collections.Generic;
using Disqord.Gateway.Api.Models;
using Qommon;
using Qommon.Collections;
using Qommon.Collections.ReadOnly;

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
                var assets = Model.Assets.GetValueOrDefault();
                if (assets == null)
                    return null;

                var largeImage = Model.Assets.Value.LargeImage.GetValueOrDefault();
                if (largeImage == null)
                    return null;

                if (_albumId == null)
                {
                    var span = largeImage.AsSpan();
                    if (span.Length > 9 && span[..8].Equals("spotify:", StringComparison.OrdinalIgnoreCase))
                        _albumId = new string(span[8..]);
                    else
                        _albumId = largeImage;
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
                var state = Model.State.GetValueOrDefault();
                if (state == null)
                    return null;

                return _artists ??= state.Split("; ", StringSplitOptions.RemoveEmptyEntries).ReadOnly();
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
