using System.Collections.Generic;
using System.Collections.Immutable;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord.Interaction
{
    public class TransientApplicationCommandInteractionResolvedData : TransientEntity<ApplicationCommandInteractionDataResolvedJsonModel>, IApplicationCommandInteractionResolvedData
    {

        public IReadOnlyDictionary<Snowflake, IUser> Users => _users ??= Optional.ConvertOrDefault(Model.Users, c =>
            c.ToReadOnlyDictionary(
                Client,
                (x, _) => x.Key,
                (y, client) => new TransientUser(client, y.Value) as IUser
            ), ImmutableDictionary<Snowflake, IUser>.Empty);
        private IReadOnlyDictionary<Snowflake, IUser> _users;

        // TODO: Map these
        public IReadOnlyDictionary<Snowflake, IMember> Members { get; }

        public IReadOnlyDictionary<Snowflake, IRole> Roles { get; }

        public IReadOnlyDictionary<Snowflake, IPartialChannel> Channels => _channels ??= Optional.ConvertOrDefault(Model.Channels, c =>
            c.ToReadOnlyDictionary(
                Client,
                (x, _) => x.Key,
                (y, client) => new TransientPartialChannel(client, y.Value) as IPartialChannel
            ), ImmutableDictionary<Snowflake, IPartialChannel>.Empty);
        private IReadOnlyDictionary<Snowflake, IPartialChannel> _channels;

        public TransientApplicationCommandInteractionResolvedData(IClient client, ApplicationCommandInteractionDataResolvedJsonModel model)
            : base(client, model)
        { }
    }
}