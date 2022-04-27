using System.Collections.Generic;
using Qommon.Collections;
using Disqord.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord
{
    public class TransientGuildPreview : TransientClientEntity<GuildPreviewJsonModel>, IGuildPreview
    {
        /// <inheritdoc/>
        public Snowflake GuildId => Model.Id;

        /// <inheritdoc/>
        public string Name => Model.Name;

        /// <inheritdoc/>
        public string IconHash => Model.Icon;

        /// <inheritdoc/>
        public string SplashHash => Model.Splash;

        /// <inheritdoc/>
        public string DiscoverySplashHash => Model.DiscoverySplash.GetValueOrDefault();

        /// <inheritdoc/>
        public IReadOnlyList<string> Features => Model.Features;

        /// <inheritdoc/>
        public IReadOnlyDictionary<Snowflake, IGuildEmoji> Emojis => _emojis ??= Model.Emojis.ToReadOnlyDictionary((Client, GuildId),
            (model, _) => model.Id.Value,
            (model, state) =>
            {
                var (client, guildId) = state;
                return new TransientGuildEmoji(client, guildId, model) as IGuildEmoji;
            });
        private IReadOnlyDictionary<Snowflake, IGuildEmoji> _emojis;

        /// <inheritdoc/>
        public int ApproximateMemberCount => Model.ApproximateMemberCount;

        /// <inheritdoc/>
        public int ApproximatePresenceCount => Model.ApproximatePresenceCount;

        /// <inheritdoc/>
        public string Description => Model.Description;

        public TransientGuildPreview(IClient client, GuildPreviewJsonModel model)
            : base(client, model)
        { }

        public override string ToString()
            => this.GetString();
    }
}
