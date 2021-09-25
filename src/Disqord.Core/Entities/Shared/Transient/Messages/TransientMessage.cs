using System.Collections.Generic;
using Disqord.Models;
using Qommon.Collections;

namespace Disqord
{
    public abstract class TransientMessage : TransientEntity<MessageJsonModel>, IMessage
    {
        /// <inheritdoc/>
        public Snowflake Id => Model.Id;

        /// <inheritdoc/>
        public Snowflake ChannelId => Model.ChannelId;

        /// <inheritdoc/>
        public virtual IUser Author
        {
            get
            {
                if (_author == null)
                    _author = new TransientUser(Client, Model.Author);

                return _author;
            }
        }
        protected IUser _author;

        /// <inheritdoc/>
        public virtual string Content => Model.Content;

        /// <inheritdoc/>
        public IReadOnlyList<IUser> MentionedUsers
        {
            get
            {
                if (_mentionedUsers == null)
                    _mentionedUsers = Model.Mentions.ToReadOnlyList(Client, (x, client) => new TransientUser(client, x));

                return _mentionedUsers;
            }
        }
        private IReadOnlyList<IUser> _mentionedUsers;

        /// <inheritdoc/>
        public Optional<IReadOnlyDictionary<IEmoji, MessageReaction>> Reactions
        {
            get
            {
                if (!Model.Reactions.HasValue)
                    return default;

                if (_reactions == null)
                    _reactions = Optional.Convert(Model.Reactions, x => x.ToReadOnlyDictionary(
                        x => Emoji.Create(x.Emoji),
                        x => new MessageReaction(x)));

                return _reactions.Value;
            }
        }
        private Optional<IReadOnlyDictionary<IEmoji, MessageReaction>>? _reactions;

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
}
