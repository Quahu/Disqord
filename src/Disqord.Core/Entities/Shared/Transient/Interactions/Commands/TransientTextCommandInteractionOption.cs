using System;
using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord
{
    public class TransientTextCommandInteractionOption : TransientEntity<ApplicationCommandInteractionDataOptionJsonModel>, ITextCommandInteractionOption
    {
        /// <inheritdoc/>
        public ApplicationCommandOptionType Type => Model.Type;

        /// <inheritdoc/>
        public object Value => Model.Value.GetValueOrDefault()?.Value;

        /// <inheritdoc/>
        public IReadOnlyDictionary<string, ITextCommandInteractionOption> Options
        {
            get
            {
                if (!Model.Options.HasValue)
                    return ReadOnlyDictionary<string, ITextCommandInteractionOption>.Empty;

                return _options ??= Model.Options.Value.ToReadOnlyDictionary(Client,
                    (model, _) => model.Name,
                    (model, client) => new TransientTextCommandInteractionOption(client, model) as ITextCommandInteractionOption, StringComparer.OrdinalIgnoreCase);
            }
        }
        private IReadOnlyDictionary<string, ITextCommandInteractionOption> _options;

        public TransientTextCommandInteractionOption(IClient client, ApplicationCommandInteractionDataOptionJsonModel model)
            : base(client, model)
        { }
    }
}
