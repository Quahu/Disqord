using Disqord.Models;

namespace Disqord
{
    public sealed class CachedSystemMessage : CachedMessage, ISystemMessage
    {
        public override string Content => Discord.GetSystemMessageContent(this, Guild);

        public SystemMessageType Type { get; }

        public string RawContent { get; }

        internal CachedSystemMessage(DiscordClient client, MessageModel model, ICachedMessageChannel channel, CachedUser author) : base(client, model, channel, author)
        {
            Type = (SystemMessageType) (model.Type - 1);
            RawContent = model.Content.GetValueOrDefault();
        }
    }
}
