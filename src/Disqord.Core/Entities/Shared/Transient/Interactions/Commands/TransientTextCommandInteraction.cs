using System;
using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord
{
    public class TransientTextCommandInteraction : TransientApplicationCommandInteraction, ITextCommandInteraction
    {
        /// <inheritdoc/>
        public IReadOnlyDictionary<string, ITextCommandInteractionOption> Options
        {
            get
            {
                if (!Model.Data.Value.Options.HasValue)
                    return ReadOnlyDictionary<string, ITextCommandInteractionOption>.Empty;

                return _options ??= Model.Data.Value.Options.Value.ToReadOnlyDictionary(Client,
                    (model, _) => model.Name,
                    (model, client) => new TransientTextCommandInteractionOption(client, model) as ITextCommandInteractionOption, StringComparer.OrdinalIgnoreCase);
            }
        }
        private IReadOnlyDictionary<string, ITextCommandInteractionOption> _options;

        public TransientTextCommandInteraction(IClient client, InteractionJsonModel model)
            : base(client, model)
        { }
    }
}
