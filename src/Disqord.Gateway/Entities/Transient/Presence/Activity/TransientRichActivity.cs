using System;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway
{
    public class TransientRichActivity : TransientActivity, IRichActivity
    {
        /// <inheritdoc/>
        public string State => Model.Name;

        /// <inheritdoc/>
        public string Details => Model.Details.GetValueOrDefault();

        /// <inheritdoc/>
        public DateTimeOffset? StartedAt
        {
            get
            {
                if (!Model.Timestamps.HasValue || !Model.Timestamps.Value.Start.HasValue)
                    return null;

                try
                {
                    return DateTimeOffset.FromUnixTimeMilliseconds(Model.Timestamps.Value.Start.Value);
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <inheritdoc/>
        public DateTimeOffset? EndsAt
        {
            get
            {
                if (!Model.Timestamps.HasValue || !Model.Timestamps.Value.End.HasValue)
                    return null;

                try
                {
                    return DateTimeOffset.FromUnixTimeMilliseconds(Model.Timestamps.Value.End.Value);
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <inheritdoc/>
        public RichActivityAsset LargeAsset
        {
            get
            {
                if (!Model.Assets.HasValue)
                    return null;

                var assets = Model.Assets.Value;
                if (!assets.LargeImage.HasValue && !assets.LargeText.HasValue)
                    return null;

                return _largeAsset ??= new RichActivityAsset(ApplicationId, assets.LargeImage, assets.LargeText);

            }
        }
        private RichActivityAsset _largeAsset;

        /// <inheritdoc/>
        public RichActivityAsset SmallAsset
        {
            get
            {
                if (!Model.Assets.HasValue)
                    return null;

                var assets = Model.Assets.Value;
                if (!assets.SmallImage.HasValue && !assets.SmallText.HasValue)
                    return null;

                return _smallAsset ??= new RichActivityAsset(ApplicationId, assets.SmallImage, assets.SmallText);

            }
        }
        private RichActivityAsset _smallAsset;

        /// <inheritdoc/>
        public RichActivityParty Party
        {
            get
            {
                if (!Model.Party.HasValue)
                    return null;

                return _party ??= new RichActivityParty(Model.Party.Value);

            }
        }
        private RichActivityParty _party;

        /// <inheritdoc/>
        public string MatchSecret => Model.Secrets.GetValueOrDefault()?.Match;

        /// <inheritdoc/>
        public string JoinSecret => Model.Secrets.GetValueOrDefault()?.Join;

        /// <inheritdoc/>
        public string SpectateSecret => Model.Secrets.GetValueOrDefault()?.Spectate;

        /// <inheritdoc/>
        public bool IsInstanced => Model.Instance.GetValueOrDefault();

        /// <inheritdoc/>
        public Snowflake? ApplicationId => Model.ApplicationId.GetValueOrNullable();

        /// <inheritdoc/>
        public string SyncId => Model.SyncId.GetValueOrDefault();

        /// <inheritdoc/>
        public string SessionId => Model.SessionId.GetValueOrDefault();

        /// <inheritdoc/>
        public ActivityFlags? Flags => Model.Flags.GetValueOrNullable();

        public TransientRichActivity(IClient client, ActivityJsonModel model)
            : base(client, model)
        { }

        public override string ToString()
            => State;
    }
}
