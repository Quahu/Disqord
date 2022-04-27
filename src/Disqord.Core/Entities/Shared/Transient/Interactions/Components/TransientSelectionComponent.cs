using System.Collections.Generic;
using Qommon.Collections;
using Disqord.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord
{
    /// <inheritdoc cref="ISelectionComponent"/>
    public class TransientSelectionComponent : TransientComponent, ISelectionComponent
    {
        /// <inheritdoc/>
        public string CustomId => Model.CustomId.Value;

        /// <inheritdoc/>
        public string Placeholder => Model.Placeholder.GetValueOrDefault();

        /// <inheritdoc/>
        public int MinimumSelectedOptions => Model.MinValues.Value;

        /// <inheritdoc/>
        public int MaximumSelectedOptions => Model.MaxValues.Value;

        /// <inheritdoc/>
        public IReadOnlyList<ISelectionComponentOption> Options => _options ??= Model.Options.Value.ToReadOnlyList(Client, (x, client) => new TransientSelectionComponentOption(client, x));
        private IReadOnlyList<ISelectionComponentOption> _options;

        /// <inheritdoc/>
        public bool IsDisabled => Model.Disabled.GetValueOrDefault();

        public TransientSelectionComponent(IClient client, ComponentJsonModel model)
            : base(client, model)
        { }
    }
}
