using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Disqord.Models;
using Qommon.Collections;

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
            MutualGuilds = new ReadOnlyDictionary<Snowflake, RestMutualGuild>(
                model.MutualGuilds.ToDictionary(x => new Snowflake(x.Id), x => new RestMutualGuild(Client, x)));
            if (User != null)
                User.Update(model.User);
            else
                User = new RestUser(Client, model.User);
            Flags = model.User.Flags.Value;
            ConnectedAccounts = model.ConnectedAccounts.Select(x => new RestConnectedAccount(Client, x)).ToImmutableArray();
        }
    }
}
