using System;
using System.Collections.Generic;
using Disqord.Models;
using Qommon.Collections;

namespace Disqord
{
    public class TransientSlashCommandAutoCompleteInteraction : TransientApplicationCommandInteraction, ISlashCommandAutoCompleteInteraction
    {
        /// <inheritdoc/>
        public IReadOnlyDictionary<string, ISlashCommandAutoCompleteInteractionOption> Options
        {
            get
            {
                if (!Model.Data.Value.Options.HasValue)
                    return ReadOnlyDictionary<string, ISlashCommandAutoCompleteInteractionOption>.Empty;

                return _options ??= Model.Data.Value.Options.Value.ToReadOnlyDictionary(Client,
                    (model, _) => model.Name,
                    (model, client) => new TransientSlashCommandInteractionOption(client, model) as ISlashCommandAutoCompleteInteractionOption, StringComparer.OrdinalIgnoreCase);
            }
        }
        private IReadOnlyDictionary<string, ISlashCommandAutoCompleteInteractionOption> _options;

        public TransientSlashCommandAutoCompleteInteraction(IClient client, InteractionJsonModel model)
            : base(client, model)
        { }
    }
}
