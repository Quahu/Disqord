using System.Collections.Generic;
using Disqord.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord;

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

            var users = new Dictionary<Snowflake, IUser>(Model.Users.Value.Count);
            if (Model.Members.HasValue)
            {
                foreach (var (id, memberModel) in Model.Members.Value)
                {
                    if (!Model.Users.Value.TryGetValue(id, out var userModel))
                        continue;

                    memberModel.User = userModel;
                    users.Add(id, new TransientMember(Client, _guildId!.Value, memberModel));
                }
            }
            else
            {
                foreach (var (id, userModel) in Model.Users.Value)
                    users.Add(id, new TransientUser(Client, userModel));
            }

            return _users = users.ReadOnly();
        }
    }
    internal IReadOnlyDictionary<Snowflake, IUser>? _users;

    /// <inheritdoc/>
    public IReadOnlyDictionary<Snowflake, IRole> Roles
    {
        get
        {
            if (!Model.Roles.HasValue)
                return ReadOnlyDictionary<Snowflake, IRole>.Empty;

            return _roles ??= Model.Roles.Value.ToReadOnlyDictionary((Client, _guildId!.Value),
                (kvp, _) => kvp.Key,
                (kvp, state) =>
                {
                    var (client, guildId) = state;
                    return new TransientRole(client, guildId, kvp.Value) as IRole;
                });
        }
    }
    private IReadOnlyDictionary<Snowflake, IRole>? _roles;

    /// <inheritdoc/>
    public IReadOnlyDictionary<Snowflake, IInteractionChannel> Channels
    {
        get
        {
            if (!Model.Channels.HasValue)
                return ReadOnlyDictionary<Snowflake, IInteractionChannel>.Empty;

            return _channels ??= Model.Channels.Value.ToReadOnlyDictionary(Client,
                (kvp, _) => kvp.Key,
                (kvp, client) => new TransientInteractionChannel(client, kvp.Value) as IInteractionChannel);
        }
    }
    private IReadOnlyDictionary<Snowflake, IInteractionChannel>? _channels;

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
    private IReadOnlyDictionary<Snowflake, IMessage>? _messages;

    /// <inheritdoc />
    public IReadOnlyDictionary<Snowflake, IAttachment> Attachments
    {
        get
        {
            if (!Model.Attachments.HasValue)
                return ReadOnlyDictionary<Snowflake, IAttachment>.Empty;

            return _attachments ??= Model.Attachments.Value.ToReadOnlyDictionary(
                kvp => kvp.Key,
                kvp => new TransientAttachment(kvp.Value) as IAttachment);
        }
    }
    private IReadOnlyDictionary<Snowflake, IAttachment>? _attachments;

    private readonly Snowflake? _guildId;

    public TransientApplicationCommandInteractionEntities(IClient client, Snowflake? guildId, ApplicationCommandInteractionDataResolvedJsonModel model)
        : base(client, model)
    {
        _guildId = guildId;
    }
}
