using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord.Rest
{
    // TODO: naming?
    public sealed class RestPreview : RestDiscordEntity
    {
        public Snowflake GuildId { get; }

        public RestFetchable<RestGuild> Guild { get; }

        public string Name { get; }

        public string Description { get; }

        public string IconHash { get; }

        public string SplashHash { get; }

        public string DiscoverySplashHash { get; }

        public IReadOnlyList<string> Features { get; }

        public IReadOnlyDictionary<Snowflake, RestGuildEmoji> Emojis { get; private set; }

        public int ApproximateOnlineMemberCount { get; private set; }

        public int ApproximateMemberCount { get; private set; }

        internal RestPreview(RestDiscordClient client, PreviewModel model) : base(client)
        {
            GuildId = model.Id;
            Guild = RestFetchable.Create(this, (@this, options) =>
                @this.Client.GetGuildAsync(@this.GuildId, options));
            Name = model.Name;
            Description = model.Description;
            IconHash = model.Icon;
            SplashHash = model.Splash;
            DiscoverySplashHash = model.DiscoverySplash;
            Features = model.Features.ReadOnly();

            // TODO: fetchable tying
            Emojis = model.Emojis.ToReadOnlyDictionary((x, _) => new Snowflake(x.Id.Value), (x, @this) => new RestGuildEmoji(@this.Client, @this.GuildId, x), this);

            ApproximateOnlineMemberCount = model.ApproximatePresenceCount;
            ApproximateMemberCount = model.ApproximateMemberCount;
        }

        public string GetIconUrl(ImageFormat format = default, int size = 2048)
            => Discord.Cdn.GetGuildIconUrl(GuildId, IconHash, format, size);

        public string GetSplashUrl(int size = 2048)
            => Discord.Cdn.GetGuildSplashUrl(GuildId, SplashHash, ImageFormat.Png, size);

        public string GetDiscoverySplashUrl(int size = 2048)
            => Discord.Cdn.GetGuildDiscoverySplashUrl(GuildId, DiscoverySplashHash, ImageFormat.Png, size);

        public override string ToString()
            => Name;
    }
}
