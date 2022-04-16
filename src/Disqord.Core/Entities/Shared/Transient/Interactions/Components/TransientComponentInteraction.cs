using System.Collections.Generic;
using Disqord.Interactions;
using Disqord.Models;

namespace Disqord.Interaction
{
    public class TransientComponentInteraction : TransientInteraction, IComponentInteraction
    {
        /// <inheritdoc/>
        public string CustomId => Model.Data.Value.CustomId.Value;

        /// <inheritdoc/>
        public ComponentType ComponentType => Model.Data.Value.ComponentType.Value;

        /// <inheritdoc/>
        public IReadOnlyList<string> SelectedValues => Model.Data.Value.Values.Value;

        /// <inheritdoc/>
        public IUserMessage Message => _message ??= new TransientUserMessage(Client, Model.Message.Value);
        private IUserMessage _message;

        public TransientComponentInteraction(IClient client, InteractionJsonModel model)
            : base(client, model)
        { }
    }
}
