using System.Collections.Generic;
using Disqord.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord;
// If you update any members of this class, make sure to do the same for the gateway equivalent.

public abstract class TransientMessage : TransientClientEntity<MessageJsonModel>, IMessage
{
    /// <inheritdoc/>
    public Snowflake Id => Model.Id;

    /// <inheritdoc/>
    public Snowflake ChannelId => Model.ChannelId;

    /// <inheritdoc/>
    public IUser Author => _author ??= new TransientUser(Client, Model.Author);

    private IUser? _author;

    /// <inheritdoc/>
    public virtual string Content => Model.Content;

    /// <inheritdoc/>
    public IReadOnlyList<IUser> MentionedUsers => _mentionedUsers ??= Model.Mentions.ToReadOnlyList(Client, (model, client) => new TransientUser(client, model));

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

    protected TransientMessage(IClient client, MessageJsonModel model)
        : base(client, model)
    { }

    /// <summary>
    ///     Creates either a <see cref="TransientUserMessage"/> or a <see cref="TransientSystemMessage"/> based on the type.
    /// </summary>
    /// <param name="client"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public static IMessage Create(IClient client, MessageJsonModel model)
    {
        switch (model.Type)
        {
            case UserMessageType.Default:
            case UserMessageType.Reply:
            case UserMessageType.SlashCommand:
            case UserMessageType.ThreadStarterMessage:
            case UserMessageType.ContextMenuCommand:
                return new TransientUserMessage(client, model);

            default:
                return new TransientSystemMessage(client, model);
        }
    }
}
