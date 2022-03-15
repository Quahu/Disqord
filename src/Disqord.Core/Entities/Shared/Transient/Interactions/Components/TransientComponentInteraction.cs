using System.Collections.Generic;
using Disqord.Interactions;
using Disqord.Models;

namespace Disqord.Interaction
{
    public class TransientComponentInteraction : TransientIdentifiableInteraction, IComponentInteraction
    {
        public ComponentType ComponentType => Model.Data.Value.ComponentType.Value;

        public IReadOnlyList<string> SelectedValues => Model.Data.Value.Values.Value;

        public IUserMessage Message => _message ??= new TransientUserMessage(Client, Model.Message.Value);
        private IUserMessage _message;

        public TransientComponentInteraction(IClient client, InteractionJsonModel model)
            : base(client, model)
        { }
    }
}
