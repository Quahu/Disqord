using Disqord.Models;

namespace Disqord
{
    public class TransientSystemMessage : TransientMessage, ISystemMessage
    {
        public string RawContent => base.Content;

        public SystemMessageType Type { get; }

        public override string Content => Discord.Internal.GetSystemMessageContent(this, null);

        public TransientSystemMessage(IClient client, MessageJsonModel model)
            : base(client, model)
        {
            Type = (SystemMessageType) (model.Type - 1);
        }
    }
}
