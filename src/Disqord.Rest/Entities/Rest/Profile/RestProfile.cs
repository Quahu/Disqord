using System;
using System.Collections.Generic;
using System.Linq;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord.Rest
{
    public sealed class RestProfile : RestDiscordEntity
    {
        public DateTimeOffset? NitroSince { get; private set; }

        public DateTimeOffset? BoostingSince { get; private set; }

        public IReadOnlyDictionary<Snowflake, RestMutualGuild> MutualGuilds { get; private set; }

        public RestUser User { get; private set; }

        public UserFlags Flags { get; private set; }

        public IReadOnlyList<RestConnectedAccount> ConnectedAccounts { get; private set; }

        internal RestProfile(RestDiscordClient client, ProfileModel model) : base(client)
        {
            NitroSince = model.PremiumSince;
            BoostingSince = model.PremiumGuildSince;
            MutualGuilds = model.MutualGuilds.ToDictionary(
                x => new Snowflake(x.Id), x => new RestMutualGuild(Client, x)).ReadOnly();
            if (User == null)
                User = new RestUser(Client, model.User);
            else
                User.Update(model.User);
            Flags = model.User.Flags.Value;
            ConnectedAccounts = model.ConnectedAccounts.ToReadOnlyList(
                this, (x, @this) => new RestConnectedAccount(@this.Client, x));
        }
    }
}
