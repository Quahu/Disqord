using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Disqord.Gateway.Api.Models;
using Disqord.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord.Gateway;

public abstract class CachedMessage : CachedSnowflakeEntity, IGatewayMessage,
    IJsonUpdatable<MessageReactionAddJsonModel>, IJsonUpdatable<MessageReactionRemoveJsonModel>,
    IJsonUpdatable<MessageReactionRemoveEmojiJsonModel>, IJsonUpdatable<MessageReactionRemoveAllJsonModel>
{
    /// <inheritdoc/>
    public Snowflake ChannelId { get; }

    /// <inheritdoc/>
    public Snowflake? GuildId { get; }

    /// <inheritdoc/>
    public IUser Author => _author;

    protected IUser _author;

    /// <inheritdoc/>
    public virtual string Content { get; protected set; } = null!;

    /// <inheritdoc/>
    public IReadOnlyList<IUser> MentionedUsers { get; protected set; } = null!;

    /// <inheritdoc/>
    public Optional<IReadOnlyDictionary<IEmoji, IMessageReaction>> Reactions { get; protected set; }

    /// <inheritdoc/>
    public MessageFlags Flags { get; protected set; }

    protected CachedMessage(IGatewayClient client, CachedMember? author, MessageJsonModel model)
        : base(client, model.Id)
    {
        ChannelId = model.ChannelId;
        GuildId = model.GuildId.GetValueOrNullable();
        if (author != null)
        {
            _author = author;
        }
        else
        {
            if (model.Member.HasValue)
            {
                model.Member.Value.User = model.Author;
                _author = new TransientMember(Client, GuildId!.Value, model.Member.Value);
            }
            else
            {
                _author = new TransientUser(Client, model.Author);
            }
        }

        Update(model);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public virtual void Update(MessageJsonModel model)
    {
        if (_author is TransientUser)
        {
            if (model.Member.HasValue)
            {
                model.Member.Value.User = model.Author;
                _author = new TransientMember(Client, GuildId!.Value, model.Member.Value);
            }
            else
            {
                _author = new TransientUser(Client, model.Author);
            }
        }

        Content = model.Content;
        MentionedUsers = model.Mentions.ToReadOnlyList(Client, (x, client) =>
        {
            var user = client.GetUser(x.Id);
            if (user != null)
                return user;

            return new TransientUser(client, x) as IUser;
        });

        Reactions = Optional.Convert(model.Reactions, models => models.ToReadOnlyDictionary(
            x => TransientEmoji.Create(x.Emoji),
            x => new TransientMessageReaction(x) as IMessageReaction));

        Flags = model.Flags.GetValueOrDefault();
    }

    public void Update(MessageReactionAddJsonModel model)
    {
        var reactions = Reactions;
        if (!reactions.HasValue)
        {
            var newReactions = new Dictionary<IEmoji, IMessageReaction>();
            var reaction = new TransientMessageReaction(model.Emoji, 1, model.UserId == Client.CurrentUser?.Id);
            newReactions.Add(reaction.Emoji, reaction);
            Reactions = new Optional<IReadOnlyDictionary<IEmoji, IMessageReaction>>(newReactions.ReadOnly());
        }
        else
        {
            var newReactions = new Dictionary<IEmoji, IMessageReaction>();
            TransientMessageReaction? reaction = null;
            foreach (var kvp in reactions.Value)
            {
                var emoji = kvp.Key;
                if (model.Emoji.Id != null)
                {
                    if (emoji is ICustomEmoji customEmoji && model.Emoji.Id == customEmoji.Id)
                        reaction = (kvp.Value as TransientMessageReaction)!;
                }
                else if (emoji is not ICustomEmoji && model.Emoji.Name == emoji.Name)
                {
                    reaction = (kvp.Value as TransientMessageReaction)!;
                }

                newReactions.Add(emoji, kvp.Value);
            }

            if (reaction == null)
            {
                reaction = new TransientMessageReaction(model.Emoji, 1, false);
                newReactions.Add(reaction.Emoji, reaction);
            }
            else
            {
                var reactionModel = reaction.Model;
                reactionModel.Count++;
                reactionModel.Me = reactionModel.Me || model.UserId == Client.CurrentUser?.Id;
            }

            Reactions = newReactions;
        }
    }

    public void Update(MessageReactionRemoveJsonModel model)
    {
        var reactions = Reactions;
        if (!reactions.HasValue)
            return;

        var emoji = TransientEmoji.Create(model.Emoji);
        if (reactions.Value.GetValueOrDefault(emoji) is not TransientMessageReaction reaction)
            return;

        var newReactions = new Dictionary<IEmoji, IMessageReaction>(reactions.Value);
        if (reaction.Count == 1)
        {
            newReactions.Remove(emoji);
        }
        else
        {
            var emojiModel = reaction.Model;
            emojiModel.Count--;

            if (reaction.HasOwnReaction && model.UserId != Client.CurrentUser?.Id)
                emojiModel.Me = false;
        }

        Reactions = newReactions;
    }

    public void Update(MessageReactionRemoveEmojiJsonModel model)
    {
        Reactions = Optional.Convert(Reactions, reactions => reactions.Where(kvp => !model.Emoji.Equals(kvp.Key)).ToReadOnlyDictionary(kvp => kvp.Key, kvp => kvp.Value));
    }

    public void Update(MessageReactionRemoveAllJsonModel model)
    {
        Reactions = default;
    }
}
