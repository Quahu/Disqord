using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Disqord.Models;
using Qommon.Collections;

namespace Disqord.Rest
{
    public sealed class RestUserProfile : RestDiscordEntity
    {
        public DateTimeOffset? NitroSince { get; private set; }

        public IReadOnlyDictionary<Snowflake, RestMutualGuild> MutualGuilds { get; private set; }

        public RestUser User { get; private set; }

        public IReadOnlyList<RestConnectedAccount> ConnectedAccounts { get; private set; }

        internal RestUserProfile(RestDiscordClient client, ProfileModel model) : base(client)
        {
            Update(model);
        }

        internal void Update(ProfileModel model)
        {
            NitroSince = model.PremiumSince;
            MutualGuilds = new ReadOnlyDictionary<Snowflake, RestMutualGuild>(model.MutualGuilds.ToDictionary(x => new Snowflake(x.Id), x => new RestMutualGuild(Client, x)));
            if (User != null)
                User.Update(model.User);
            else
                User = new RestUser(Client, model.User);
            ConnectedAccounts = model.ConnectedAccounts.Select(x => new RestConnectedAccount(Client, x)).ToImmutableArray();
        }
    }
}
