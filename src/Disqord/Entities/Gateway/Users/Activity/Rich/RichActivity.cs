using System;
using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents a user's rich activity.
    /// </summary>
    public sealed partial class RichActivity : Activity
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

        public Snowflake? ApplicationId { get; }

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

            IsInstance = model.Instance.GetValueOrDefault();
            ApplicationId = model.ApplicationId;
            SyncId = model.SyncId;
            SessionId = model.SessionId;
            Flags = model.Flags;
        }
    }
}
