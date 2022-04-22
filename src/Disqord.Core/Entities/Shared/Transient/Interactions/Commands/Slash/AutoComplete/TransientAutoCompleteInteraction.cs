using System;
using System.Collections.Generic;
using Disqord.Models;
using Qommon.Collections;
using Qommon.Collections.ReadOnly;

namespace Disqord
{
    public class TransientAutoCompleteInteraction : TransientApplicationCommandInteraction, IAutoCompleteInteraction
    {
        /// <inheritdoc/>
        public IReadOnlyDictionary<string, IAutoCompleteInteractionOption> Options
        {
            get
            {
                if (!Model.Data.Value.Options.HasValue)
                    return ReadOnlyDictionary<string, IAutoCompleteInteractionOption>.Empty;

                return _options ??= Model.Data.Value.Options.Value.ToReadOnlyDictionary(Client,
                    (model, _) => model.Name,
                    (model, client) => new TransientAutoCompleteInteractionOption(client, model) as IAutoCompleteInteractionOption, StringComparer.OrdinalIgnoreCase);
            }
        }
        private IReadOnlyDictionary<string, IAutoCompleteInteractionOption> _options;

        public TransientAutoCompleteInteraction(IClient client, InteractionJsonModel model)
            : base(client, model)
        { }
    }
}
