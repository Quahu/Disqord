using Disqord.Models;

namespace Disqord
{
    public sealed class CachedSystemMessage : CachedMessage, ISystemMessage
    {
        public override string Content => Discord.Internal.GetSystemMessageContent(this, Guild);

        public SystemMessageType Type { get; }

        public string RawContent { get; }

        internal CachedSystemMessage(ICachedMessageChannel channel, CachedUser author, MessageModel model) : base(channel, author, model)
        {
            Type = (SystemMessageType) (model.Type - 1);
            RawContent = model.Content.GetValueOrDefault();

            Update(model);
        }
    }
}
