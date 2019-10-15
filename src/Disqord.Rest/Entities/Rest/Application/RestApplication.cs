using System.Collections.Generic;
using System.Collections.Immutable;
using Disqord.Models;

namespace Disqord.Rest
{
    public sealed class RestApplication : RestSnowflakeEntity
    {
        public string Name { get; }

        public string IconHash { get; }

        public string Description { get; }

        public IReadOnlyList<string> RpcOrigins { get; }

        public bool IsBotPublic { get; }

        public bool BotRequiresCodeGrant { get; }

        public RestUser Owner { get; }

        public RestTeam Team { get; }

        public string Summary { get; }

        public string VerifyKey { get; }

        public Snowflake GuildId { get; }

        public RestDownloadable<RestGuild> Guild { get; }

        public Snowflake PrimarySkuId { get; }

        public string SlugUrl { get; }

        public string CoverImageHash { get; }

        internal RestApplication(RestDiscordClient client, ApplicationModel model) : base(client, model.Id)
        {
            Name = model.Name;
            IconHash = model.Icon;
            Description = model.Description;
            RpcOrigins = RpcOrigins != null
                ? model.RpcOrigins.ToImmutableArray()
                : ImmutableArray<string>.Empty;
            IsBotPublic = model.BotPublic;
            BotRequiresCodeGrant = model.BotRequireCodeGrant;

            if (model.Owner != null)
                Owner = new RestUser(Client, model.Owner);

            Summary = model.Summary;
            VerifyKey = model.VerifyKey;

            GuildId = model.GuildId;
            Guild = new RestDownloadable<RestGuild>(options => Client.GetGuildAsync(GuildId, options));

            PrimarySkuId = model.PrimarySkuId;
            SlugUrl = model.Slug;
            CoverImageHash = model.CoverImage;
        }
    }
}
