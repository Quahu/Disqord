using Disqord.Models;

namespace Disqord
{
    public class MessageActivity
    {
        public MessageActivityType Type { get; }

        public string? PartyId { get; }

        public MessageActivity(MessageActivityJsonModel model)
        {
            Type = model.Type;
            PartyId = model.PartyId.GetValueOrDefault();
        }

        public override string ToString()
            => $"{Type}: {PartyId ?? "<no party id>"}";
    }
}
