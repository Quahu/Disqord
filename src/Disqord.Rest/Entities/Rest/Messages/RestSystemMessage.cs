using Disqord.Models;

namespace Disqord.Rest
{
    public sealed class RestSystemMessage : RestMessage, ISystemMessage
    {
        public override string Content => Discord.Internal.GetSystemMessageContent(this, null);

        public SystemMessageType Type { get; }

        public string RawContent { get; }

        internal RestSystemMessage(RestDiscordClient client, MessageModel model) : base(client, model)
        {
            Type = (SystemMessageType) (model.Type - 1);
            RawContent = model.Content.GetValueOrDefault();
        }
    }
}
