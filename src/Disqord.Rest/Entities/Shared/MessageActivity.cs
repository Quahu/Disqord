using Disqord.Models;

namespace Disqord
{
    public sealed class MessageActivity
    {
        public MessageActivityType Type { get; }

        public string PartyId { get; }

        internal MessageActivity(MessageActivityModel model)
        {
            Type = model.Type;
            PartyId = model.PartyId.GetValueOrDefault();
        }

        public override string ToString()
            => $"{Type}: {PartyId ?? "<no party id>"}";
    }
}
