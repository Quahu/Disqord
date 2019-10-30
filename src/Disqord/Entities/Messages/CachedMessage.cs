using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Disqord.Collections;
using Disqord.Models;
using Qommon.Collections;

namespace Disqord
{
    public abstract partial class CachedMessage : CachedSnowflakeEntity, IMessage
    {
        public CachedGuild Guild => (Channel as CachedTextChannel)?.Guild;

        public ICachedMessageChannel Channel { get; }

        public CachedUser Author { get; }

        public abstract string Content { get; }

        public IReadOnlyList<CachedUser> UserMentions { get; private set; }

        public IReadOnlyDictionary<IEmoji, ReactionData> Reactions { get; }

        public string JumpUrl => Guild != null
            ? $"https://discordapp.com/channels/{Guild.Id}/{Channel.Id}/{Id}"
            : $"https://discordapp.com/channels/@me/{Channel.Id}/{Id}";

        internal readonly LockedDictionary<IEmoji, ReactionData> _reactions;

        IUser IMessage.Author => Author;
        IReadOnlyList<IUser> IMessage.UserMentions => UserMentions;
        Snowflake IMessage.ChannelId => Channel.Id;

        internal CachedMessage(DiscordClient client, MessageModel model, ICachedMessageChannel channel, CachedUser author) : base(client, model.Id)
        {
            Channel = channel;
            Author = author;
            _reactions = new LockedDictionary<IEmoji, ReactionData>(model.Reactions.HasValue
                ? model.Reactions.Value.Length
                : 0);
            Reactions = new ReadOnlyDictionary<IEmoji, ReactionData>(_reactions);
        }

        internal static CachedMessage Create(DiscordClient client, MessageModel model, ICachedMessageChannel channel, CachedUser author)
        {
            return model.Type switch
            {
                MessageType.Default => new CachedUserMessage(client, model, channel, author),
                _ => new CachedSystemMessage(client, model, channel, author),
            };
        }

        internal virtual void Update(MessageModel model)
        {
            if (model.Mentions.HasValue)
                UserMentions = model.Mentions.Value.Select(x => Client.GetUser(x.Id)).ToImmutableArray();
        }
    }
}
