using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Disqord.Models;
using Qommon.Collections;

namespace Disqord.Rest
{
    public abstract partial class RestMessage : RestSnowflakeEntity, IMessage
    {
        public Snowflake ChannelId { get; }

        public Snowflake? GuildId { get; }

        public RestUser Author { get; }

        public abstract string Content { get; }

        public IReadOnlyList<RestUser> MentionedUsers { get; private set; }

        public IReadOnlyDictionary<IEmoji, ReactionData> Reactions { get; private set; }

        IUser IMessage.Author => Author;
        IReadOnlyList<IUser> IMessage.MentionedUsers => MentionedUsers;

        internal RestMessage(RestDiscordClient client, MessageModel model) : base(client, model.Id)
        {
            ChannelId = model.ChannelId;
            GuildId = model.GuildId;
            Author = new RestUser(client, model.Author.Value);
        }

        internal virtual void Update(MessageModel model)
        {
            if (model.Author.HasValue)
                Author.Update(model.Author.Value);

            if (model.Mentions.HasValue)
                MentionedUsers = model.Mentions.Value.Select(x => new RestUser(Client, x)).ToImmutableArray();

            // TODO: still no idea when this is present and when not
            Reactions = model.Reactions.HasValue && model.Reactions.Value != null
                ? new ReadOnlyDictionary<IEmoji, ReactionData>(model.Reactions.Value.Select(x => new ReactionData(x)).ToDictionary(x => x.Emoji, x => x))
                : ImmutableDictionary<IEmoji, ReactionData>.Empty as IReadOnlyDictionary<IEmoji, ReactionData>;
        }

        internal static RestMessage Create(RestDiscordClient client, MessageModel model)
        {
            return model.Type switch
            {
                MessageType.Default => new RestUserMessage(client, model),
                _ => new RestSystemMessage(client, model),
            };
        }
    }
}
