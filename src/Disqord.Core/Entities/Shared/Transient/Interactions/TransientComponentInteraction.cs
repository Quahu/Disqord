using System.Collections.Generic;
using Disqord.Models;

namespace Disqord.Interaction
{
    public class TransientComponentInteraction : TransientInteraction, IComponentInteraction
    {
        public string CustomId => Model.Data.Value.CustomId.Value;

        public ComponentType ComponentType => Model.Data.Value.ComponentType.Value;

        public IReadOnlyList<string> SelectedValues => Model.Data.Value.Values.Value;

        public IUserMessage Message => _message ??= new TransientUserMessage(Client, Model.Message.Value);
        private IUserMessage _message;

        public TransientComponentInteraction(IClient client, InteractionJsonModel model)
            : base(client, model)
        { }
    }
}
