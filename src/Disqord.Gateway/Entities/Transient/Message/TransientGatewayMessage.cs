using System.Collections.Generic;
using Disqord.Gateway;
using Disqord.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord;

public abstract class TransientGatewayMessage : TransientGatewayClientEntity<MessageJsonModel>, IGatewayMessage
{
    /// <inheritdoc/>
    public Snowflake Id => Model.Id;

    /// <inheritdoc/>
    public Snowflake ChannelId => Model.ChannelId;

    /// <inheritdoc/>
    public Snowflake? GuildId => Model.GuildId.GetValueOrNullable();

    /// <inheritdoc/>
    public IUser Author
    {
        get
        {
            var guildId = Model.GuildId;
            if (!guildId.HasValue || !Model.Member.HasValue)
            {
                var user = Client.GetUser(Model.Author.Id);
                if (user != null)
                    return user;

                return _author ??= new TransientUser(Client, Model.Author);
            }

            var member = Client.GetMember(guildId.Value, Model.Author.Id);
            if (member != null)
                return member;

            if (_author == null)
            {
                // Following trick lets us not have to special-case this scenario for TransientMember.
                Model.Member.Value.User = Model.Author;
                _author = new TransientMember(Client, guildId.Value, Model.Member.Value);
            }

            return (_author as IMember)!;
        }
    }
    private IUser? _author;

    /// <inheritdoc/>
    public virtual string Content => Model.Content;

    /// <inheritdoc/>
    public IReadOnlyList<IUser> MentionedUsers => _mentionedUsers ??= Model.Mentions.ToReadOnlyList(Client,
        (model, client) => client.GetUser(model.Id) as IUser ?? new TransientUser(client, model));

    private IReadOnlyList<IUser>? _mentionedUsers;

    /// <inheritdoc/>
    public Optional<IReadOnlyDictionary<IEmoji, IMessageReaction>> Reactions
    {
        get
        {
            if (!Model.Reactions.HasValue)
                return default;

            return new(_reactions ??= Model.Reactions.Value.ToReadOnlyDictionary(
                model => TransientEmoji.Create(model.Emoji),
                model => new TransientMessageReaction(model) as IMessageReaction));
        }
    }
    private IReadOnlyDictionary<IEmoji, IMessageReaction>? _reactions;

    /// <inheritdoc/>
    public MessageFlags Flags => Model.Flags.GetValueOrDefault();

    protected TransientGatewayMessage(IClient client, MessageJsonModel model)
        : base(client, model)
    { }

    /// <summary>
    ///     Creates either a <see cref="TransientGatewayUserMessage"/> or a <see cref="TransientSystemMessage"/> based on the type.
    /// </summary>
    /// <param name="client"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public static IGatewayMessage Create(IClient client, MessageJsonModel model)
    {
        switch (model.Type)
        {
            case UserMessageType.Default:
            case UserMessageType.Reply:
            case UserMessageType.SlashCommand:
            case UserMessageType.ThreadStarterMessage:
            case UserMessageType.ContextMenuCommand:
                return new TransientGatewayUserMessage(client, model);

            default:
                return new TransientGatewaySystemMessage(client, model);
        }
    }
}
