using System;
using Disqord.Models;

namespace Disqord
{
    public sealed class RichActivity : Activity
    {
        public string State { get; }

        public string Details { get; }

        public DateTimeOffset? StartedAt { get; }

        public DateTimeOffset? EndsAt { get; }

        public RichAsset LargeAsset { get; }

        public RichAsset SmallAsset { get; }

        public RichParty Party { get; }

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
                if (model.Assets.LargeImage != null)
                    LargeAsset = new RichAsset(model.ApplicationId, model.Assets.LargeImage, model.Assets.LargeText);

                if (model.Assets.SmallImage != null)
                    SmallAsset = new RichAsset(model.ApplicationId, model.Assets.SmallImage, model.Assets.SmallText);
            }

            if (model.Party != null)
            {
                var partyId = model.Party.Id;
                int? partySize = null;
                int? partyMaxSize = null;
                if (model.Party.Size != null)
                {
                    partySize = model.Party.Size[0];
                    partyMaxSize = model.Party.Size[1];
                }

                Party = new RichParty(partyId, partySize, partyMaxSize);
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

        public sealed class RichAsset
        {
            public string Id { get; }

            public string Text { get; }

            // TODO: move to CDN urls?
            public string Url => $"https://cdn.discordapp.com/app-assets/{_applicationId}/{Id}.png";

            private readonly string _applicationId;

            internal RichAsset(string applicationId, string id, string text)
            {
                _applicationId = applicationId;
                Id = id;
                Text = text;
            }
        }

        public sealed class RichParty
        {
            public string Id { get; }

            public int? Size { get; }

            public int? MaxSize { get; }

            internal RichParty(string id, int? size, int? maxSize)
            {
                Id = id;
                Size = size;
                MaxSize = maxSize;
            }
        }
    }
}
