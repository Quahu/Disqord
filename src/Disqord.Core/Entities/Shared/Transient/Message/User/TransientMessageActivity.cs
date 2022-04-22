using Disqord.Models;
using Qommon;

namespace Disqord
{
    public class TransientMessageActivity : TransientEntity<MessageActivityJsonModel>, IMessageActivity
    {
        /// <inheritdoc/>
        public MessageActivityType Type => Model.Type;

        /// <inheritdoc/>
        public string PartyId => Model.PartyId.GetValueOrDefault();

        public TransientMessageActivity(MessageActivityJsonModel model)
            : base(model)
        { }

        public override string ToString()
            => $"{Type}: {PartyId ?? "<no party id>"}";
    }
}
