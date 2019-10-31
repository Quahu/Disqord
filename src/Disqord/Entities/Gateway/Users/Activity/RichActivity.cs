using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Disqord.Models;

namespace Disqord
{
    public sealed class RichActivity : Activity
    {
        public string State { get; }

        public string Details { get; }

        public DateTimeOffset? StartedAt { get; }

        public DateTimeOffset? EndsAt { get; }

        public string LargeImageUrl { get; }

        public string LargeText { get; }

        public string SmallImageUrl { get; }

        public string SmallText { get; }

        public string PartyId { get; }

        public IReadOnlyList<int> Size { get; }

        public string MatchSecret { get; }

        public string JoinSecret { get; }

        public string SpectateSecret { get; }

        public bool IsInstance { get; }

        public string ApplicationId { get; }

        public string SyncId { get; }

        public string SessionId { get; }

        public ActivityFlags? Flags { get; }

        internal RichActivity(ActivityModel model) : base(model)
        {
            State = model.State;
            Details = model.Details;

            if (model.Timestamps != null)
            {
                if (model.Timestamps.Start != null)
                    StartedAt = DateTimeOffset.FromUnixTimeMilliseconds(model.Timestamps.Start.Value);

                if (model.Timestamps.End != null)
                    EndsAt = DateTimeOffset.FromUnixTimeMilliseconds(model.Timestamps.End.Value);
            }

            if (model.Assets != null)
            {
                LargeImageUrl = model.Assets.LargeImage;
                LargeText = model.Assets.LargeText;
                SmallImageUrl = model.Assets.SmallImage;
                SmallText = model.Assets.SmallText;
            }

            if (model.Party != null)
            {
                PartyId = model.Party.Id;
                Size = model.Party.Size?.ToImmutableArray();
            }

            if (model.Secrets != null)
            {
                MatchSecret = model.Secrets.Match;
                JoinSecret = model.Secrets.Join;
                SpectateSecret = model.Secrets.Spectate;
            }

            IsInstance = model.Instance;
            ApplicationId = model.ApplicationId;
            SyncId = model.SyncId;
            SessionId = model.SessionId;
            Flags = model.Flags;
        }
    }
}
