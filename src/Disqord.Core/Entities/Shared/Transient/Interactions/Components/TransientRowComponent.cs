using System.Collections;
using System.Collections.Generic;
using Qommon.Collections;
using Disqord.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord
{
    /// <inheritdoc cref="IRowComponent"/>
    public class TransientRowComponent : TransientComponent, IRowComponent
    {
        /// <inheritdoc/>
        public IReadOnlyList<IComponent> Components => _components ??= Model.Components.Value.ToReadOnlyList(Client, (model, client) => Create(client, model));
        private IReadOnlyList<IComponent> _components;

        public TransientRowComponent(IClient client, ComponentJsonModel model)
            : base(client, model)
        { }

        public IEnumerator<IComponent> GetEnumerator()
            => Components.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
