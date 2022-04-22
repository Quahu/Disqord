using System.Collections.Generic;
using Disqord.Interaction;
using Disqord.Models;
using Qommon.Collections;
using Qommon.Collections.ReadOnly;

namespace Disqord.Interactions
{
    public class TransientModalSubmitInteraction : TransientInteraction, IModalSubmitInteraction
    {
        /// <inheritdoc/>
        public string CustomId => Model.Data.Value.CustomId.Value;

        /// <inheritdoc/>
        public IReadOnlyList<IComponent> Components
        {
            get
            {
                if (!Model.Data.Value.Components.HasValue)
                    return ReadOnlyList<IComponent>.Empty;

                return _components ??= Model.Data.Value.Components.Value.ToReadOnlyList(Client, (model, client) => TransientComponent.Create(client, model));
            }
        }
        private IReadOnlyList<IComponent> _components;

        public TransientModalSubmitInteraction(IClient client, InteractionJsonModel model)
            : base(client, model)
        { }
    }
}
