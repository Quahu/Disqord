using Disqord.Interaction;
using Disqord.Models;

namespace Disqord.Interactions
{
    public class TransientIdentifiableInteraction : TransientInteraction, IIdentifiableInteraction
    {
        public string CustomId => Model.Data.Value.CustomId.Value;

        public TransientIdentifiableInteraction(IClient client, InteractionJsonModel model)
            : base(client, model)
        { }
    }
}
