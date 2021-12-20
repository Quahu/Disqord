using System.Collections.Generic;
using Qommon.Collections;
using Disqord.Models;

namespace Disqord
{
    public class TransientApplicationCommandInteractionEntities : TransientClientEntity<ApplicationCommandInteractionDataResolvedJsonModel>, IApplicationCommandInteractionEntities
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
                    foreach (var (id, memberModel) in Model.Members.Value)
                    {
                        if (!Model.Users.Value.TryGetValue(id, out var userModel))
                            continue;

                        memberModel.User = userModel;
                        dictionary.Add(id, new TransientMember(Client, _guildId.Value, memberModel));
                    }

                    return dictionary;
                }

                foreach (var (id, userModel) in Model.Users.Value)
                    dictionary.Add(id, new TransientUser(Client, userModel));

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

                return _roles ??= Model.Roles.Value.ToReadOnlyDictionary((Client, _guildId.Value),
                    (kvp, _) => kvp.Key,
                    (kvp, state) =>
                    {
                        var (client, guildId) = state;
                        return new TransientRole(client, guildId, kvp.Value) as IRole;
                    });
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

                return _channels ??= Model.Channels.Value.ToReadOnlyDictionary(Client,
                    (kvp, _) => kvp.Key,
                    (kvp, client) => TransientChannel.Create(client, kvp.Value));
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

                return _messages ??= Model.Messages.Value.ToReadOnlyDictionary(Client,
                    (kvp, _) => kvp.Key,
                    (kvp, client) => TransientMessage.Create(client, kvp.Value));
            }
        }
        private IReadOnlyDictionary<Snowflake, IMessage> _messages;
        
        /// <inheritdoc /> 
        public IReadOnlyDictionary<Snowflake, IAttachment> Attachments
        {
            get
            {
                if (!Model.Attachments.HasValue)
                    return ReadOnlyDictionary<Snowflake, IAttachment>.Empty;
                return _attachments ??= Model.Attachments.Value.ToReadOnlyDictionary(Client,
                    (kvp, _) => kvp.Key,
                    (kvp, _) => TransientAttachment.Create(kvp.Value));
            }
        }

        private IReadOnlyDictionary<Snowflake, IAttachment> _attachments;

        private readonly Snowflake? _guildId;

        public TransientApplicationCommandInteractionEntities(IClient client, Snowflake? guildId, ApplicationCommandInteractionDataResolvedJsonModel model)
            : base(client, model)
        {
            _guildId = guildId;
        }
    }
}
