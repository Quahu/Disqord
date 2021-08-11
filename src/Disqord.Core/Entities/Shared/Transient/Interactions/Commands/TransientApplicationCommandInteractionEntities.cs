using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord
{
    public class TransientApplicationCommandInteractionEntities : TransientEntity<ApplicationCommandInteractionDataResolvedJsonModel>, IApplicationCommandInteractionEntities
    {
        /// <inheritdoc/>
        public IReadOnlyDictionary<Snowflake, IUser> Users
        {
            get
            {
                if (!Model.Users.HasValue)
                    return ReadOnlyDictionary<Snowflake, IUser>.Empty;

                if (_users != null)
                    return _users;

                var dictionary = new Dictionary<Snowflake, IUser>(Model.Users.Value.Count);
                if (Model.Members.HasValue)
                {
                    foreach (var kvp in Model.Members.Value)
                    {
                        if (!Model.Users.Value.TryGetValue(kvp.Key, out var userModel))
                            continue;

                        kvp.Value.User = userModel;
                        dictionary.Add(kvp.Key, new TransientMember(Client, _guildId.Value, kvp.Value));
                    }

                    return dictionary;
                }

                foreach (var kvp in Model.Users.Value)
                    dictionary.Add(kvp.Key, new TransientUser(Client, kvp.Value));

                return _users = dictionary;
            }
        }
        private IReadOnlyDictionary<Snowflake, IUser> _users;

        /// <inheritdoc/>
        public IReadOnlyDictionary<Snowflake, IRole> Roles
        {
            get
            {
                if (!Model.Roles.HasValue)
                    return ReadOnlyDictionary<Snowflake, IRole>.Empty;

                return _roles ??= Model.Roles.Value.ToReadOnlyDictionary(Client, (kvp, _) => kvp.Key, (kvp, client) => new TransientRole(client, _guildId.Value, kvp.Value) as IRole);
            }
        }
        private IReadOnlyDictionary<Snowflake, IRole> _roles;

        /// <inheritdoc/>
        public IReadOnlyDictionary<Snowflake, IChannel> Channels
        {
            get
            {
                if (!Model.Channels.HasValue)
                    return ReadOnlyDictionary<Snowflake, IChannel>.Empty;

                return _channels ??= Model.Channels.Value.ToReadOnlyDictionary(Client, (kvp, _) => kvp.Key, (kvp, client) => TransientChannel.Create(client, kvp.Value));
            }
        }
        private IReadOnlyDictionary<Snowflake, IChannel> _channels;

        /// <inheritdoc/>
        public IReadOnlyDictionary<Snowflake, IMessage> Messages
        {
            get
            {
                if (!Model.Messages.HasValue)
                    return ReadOnlyDictionary<Snowflake, IMessage>.Empty;

                return _messages ??= Model.Messages.Value.ToReadOnlyDictionary(Client, (kvp, _) => kvp.Key, (kvp, client) => TransientMessage.Create(client, kvp.Value));
            }
        }
        private IReadOnlyDictionary<Snowflake, IMessage> _messages;

        private readonly Snowflake? _guildId;

        public TransientApplicationCommandInteractionEntities(IClient client, Snowflake? guildId, ApplicationCommandInteractionDataResolvedJsonModel model)
            : base(client, model)
        {
            _guildId = guildId;
        }
    }
}
