using System;
using System.Collections.Generic;
using Disqord.Models;
using Qommon.Collections;

namespace Disqord
{
    public class TransientAutoCompleteInteractionOption : TransientClientEntity<ApplicationCommandInteractionDataOptionJsonModel>, IAutoCompleteInteractionOption
    {
        /// <inheritdoc/>
        public string Name => Model.Name;

        /// <inheritdoc/>
        public SlashCommandOptionType Type => Model.Type;

        /// <inheritdoc/>
        public object Value => Model.Value.GetValueOrDefault()?.Value;

        /// <inheritdoc/>
        public IReadOnlyDictionary<string, IAutoCompleteInteractionOption> Options
        {
            get
            {
                if (!Model.Options.HasValue)
                    return ReadOnlyDictionary<string, IAutoCompleteInteractionOption>.Empty;

                return _options ??= Model.Options.Value.ToReadOnlyDictionary(Client,
                    (model, _) => model.Name,
                    (model, client) => new TransientAutoCompleteInteractionOption(client, model) as IAutoCompleteInteractionOption, StringComparer.OrdinalIgnoreCase);
            }
        }
        private IReadOnlyDictionary<string, IAutoCompleteInteractionOption> _options;

        /// <inheritdoc/>
        public bool IsFocused => Model.Focused.GetValueOrDefault();

        public TransientAutoCompleteInteractionOption(IClient client, ApplicationCommandInteractionDataOptionJsonModel model)
            : base(client, model)
        { }
    }
}
