using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord
{
    public class TransientGuildPreview : TransientEntity<GuildPreviewJsonModel>, IGuildPreview
    {
        public Snowflake GuildId { get; }
        
        public string Name => Model.Name;

        public Snowflake Id => Model.Id;

        public string IconHash => Model.Icon;

        public string SplashHash => Model.Splash;

        public string DiscoverySplashHash => Model.DiscoverySplash.GetValueOrDefault();

        public GuildFeatures Features => new(Model.Features);
        
        public IReadOnlyDictionary<Snowflake, IGuildEmoji> Emojis => _emojis ??= Model.Emojis.ToReadOnlyDictionary((Client, Id), (x, _) => x.Id.Value, (x, tuple) =>
        {
            var (client, guildId) = tuple;
            return new TransientGuildEmoji(client, guildId, x) as IGuildEmoji;
        });
        private IReadOnlyDictionary<Snowflake, IGuildEmoji> _emojis;

        public int ApproximateMemberCount => Model.ApproximateMemberCount;

        public int ApproximatePresenceCount => Model.ApproximatePresenceCount;

        public string Description => Model.Description;
        
        public TransientGuildPreview(IClient client, Snowflake guildId, GuildPreviewJsonModel model) 
            : base(client, model)
        {
            GuildId = guildId;
        }
    }
}