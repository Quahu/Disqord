using System.Collections.Generic;
using System.Linq;
using Disqord.Models;
using Qommon.Collections;

namespace Disqord.Rest
{
    public sealed class RestTeam : RestSnowflakeEntity
    {
        public string Name { get; }

        public string IconHash { get; }

        public Snowflake OwnerId { get; }

        public RestTeamMember Owner => Members.TryGetValue(OwnerId, out var owner) ? owner : null;

        public IReadOnlyDictionary<Snowflake, RestTeamMember> Members { get; }

        internal RestTeam(RestDiscordClient client, TeamModel model) : base(client, model.Id)
        {
            Name = model.Name;
            IconHash = model.Icon;
            OwnerId = model.OwnerUserId;
            Members = new ReadOnlyDictionary<Snowflake, RestTeamMember>(
                model.Members.ToDictionary(x => new Snowflake(x.Id), x => new RestTeamMember(client, x)));
        }
    }
}