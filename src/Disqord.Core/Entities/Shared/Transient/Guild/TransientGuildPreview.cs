using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord
{
    public class TransientGuildPreview : TransientEntity<GuildPreviewJsonModel>, IGuildPreview
    {
        /// <inheritdoc/>
        public Snowflake Id => Model.Id;
        
        /// <inheritdoc/>
        public string Name => Model.Name;

        /// <inheritdoc/>
        public string IconHash => Model.Icon;

        /// <inheritdoc/>
        public string SplashHash => Model.Splash;

        /// <inheritdoc/>
        public string DiscoverySplashHash => Model.DiscoverySplash.GetValueOrDefault();

        /// <inheritdoc/>
        public GuildFeatures Features => new(Model.Features);
        
        /// <inheritdoc/>
        public IReadOnlyDictionary<Snowflake, IGuildEmoji> Emojis => _emojis ??= Model.Emojis.ToReadOnlyDictionary((Client, Id), (x, _) => x.Id.Value, (x, tuple) =>
        {
            var (client, guildId) = tuple;
            return new TransientGuildEmoji(client, guildId, x) as IGuildEmoji;
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
    }
}