using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord.Rest
{
    public sealed class RestApplication : RestSnowflakeEntity
    {
        public string Name { get; private set; }

        public string IconHash { get; private set; }

        public string Description { get; private set; }

        public IReadOnlyList<string> RpcOrigins { get; private set; }

        public bool IsBotPublic { get; private set; }

        public bool BotRequiresCodeGrant { get; private set; }

        public RestUser Owner { get; private set; }

        public RestTeam Team { get; private set; }

        public string Summary { get; private set; }

        public string VerifyKey { get; private set; }

        public Snowflake GuildId { get; }

        public RestFetchable<RestGuild> Guild { get; }

        public Snowflake PrimarySkuId { get; private set; }

        public string SlugUrl { get; private set; }

        public string CoverImageHash { get; private set; }

        internal RestApplication(RestDiscordClient client, ApplicationModel model) : base(client, model.Id)
        {
            GuildId = model.GuildId;
            Guild = RestFetchable.Create(this, (@this, options) =>
                @this.Client.GetGuildAsync(@this.GuildId, options));

            Update(model);
        }

        internal void Update(ApplicationModel model)
        {
            Name = model.Name;
            IconHash = model.Icon;
            Description = model.Description;
            RpcOrigins = RpcOrigins != null
                ? model.RpcOrigins.ReadOnly()
                : ReadOnlyList<string>.Empty;
            IsBotPublic = model.BotPublic;
            BotRequiresCodeGrant = model.BotRequireCodeGrant;

            if (model.Owner != null)
            {
                if (Owner == null)
                    Owner = new RestUser(Client, model.Owner);
                else
                    Owner.Update(model.Owner);
            }

            if (model.Team != null)
                Team = new RestTeam(Client, model.Team);

            Summary = model.Summary;
            VerifyKey = model.VerifyKey;

            PrimarySkuId = model.PrimarySkuId;
            SlugUrl = model.Slug;
            CoverImageHash = model.CoverImage;
        }
    }
}
