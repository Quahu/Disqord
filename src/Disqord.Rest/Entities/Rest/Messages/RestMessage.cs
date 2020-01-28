using System.Collections.Generic;
using System.Linq;
using Disqord.Collections;
using Disqord.Models;

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
                MentionedUsers = model.Mentions.Value.ToReadOnlyList(this, (x, @this) => new RestUser(@this.Client, x));

            // TODO: still no idea when this is present and when not
            Reactions = model.Reactions.HasValue && model.Reactions.Value != null
                ? model.Reactions.Value.Select(x => new ReactionData(x)).ToReadOnlyDictionary(x => x.Emoji, x => x)
                : ReadOnlyDictionary<IEmoji, ReactionData>.Empty;
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
